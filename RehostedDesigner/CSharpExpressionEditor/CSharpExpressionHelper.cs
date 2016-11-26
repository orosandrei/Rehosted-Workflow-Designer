using Microsoft.CSharp.Activities;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace RehostedWorkflowDesigner.CSharpExpressionEditor
{
    public static class CSharpExpressionHelper
    {
        private static readonly Type CSharpValueType = typeof(CSharpValue<>);
        private static readonly Type CSharpReferenceType = typeof(CSharpReference<>);
        private static readonly Type CodeActivityType = typeof(CodeActivity);
        private static readonly Type AsyncCodeActivityType = typeof(AsyncCodeActivity);
        private static readonly Type GenericAsyncCodeActivityType = typeof(AsyncCodeActivity<>);
        private static readonly Type VariablesCollectionType = typeof(Collection<Variable>);

        internal static ActivityWithResult CreateCSharpExpression(string expressionText, bool isLocationExpression, Type returnType)
        {
            Type expressionType;
            if (isLocationExpression)
            {
                expressionType = CSharpReferenceType.MakeGenericType(returnType);
            }
            else
            {
                expressionType = CSharpValueType.MakeGenericType(returnType);
            }

            return Activator.CreateInstance(expressionType, expressionText) as ActivityWithResult;
        }

        public static ActivityWithResult CreateExpressionFromString(string expressionText, bool useLocationExpression, Type resultType)
        {
            ActivityWithResult expression;
            if (!useLocationExpression)
            {
                if (!CSharpExpressionHelper.TryCreateLiteral(resultType, expressionText, out expression))
                {
                    expression = CSharpExpressionHelper.CreateCSharpExpression(expressionText, useLocationExpression, resultType);
                }
            }
            else
            {
                expression = CSharpExpressionHelper.CreateCSharpExpression(expressionText, useLocationExpression, resultType);
            }

            return expression;
        }

        internal static bool TryCreateLiteral(Type type, string expressionText, out ActivityWithResult literalExpression)
        {
            literalExpression = null;

            // try easy way first - look if there is a type conversion which supports conversion between expression type and string
            TypeConverter literalValueConverter = null;
            bool isQuotedString = false;
            if (IsLiteralExpressionSupported(type))
            {
                bool shouldBeQuoted;
                if (typeof(char) == type)
                {
                    shouldBeQuoted = true;
                    isQuotedString = expressionText.StartsWith("'", StringComparison.CurrentCulture) &&
                            expressionText.EndsWith("'", StringComparison.CurrentCulture) &&
                            expressionText.IndexOf("'", 1, StringComparison.CurrentCulture) == expressionText.Length - 1;
                }
                else
                {
                    shouldBeQuoted = typeof(string) == type;

                    // whether string begins and ends with quotes '"'. also, if there are
                    // more quotes within than those begining and ending ones, do not bother with literal - assume this is an expression.
                    isQuotedString = shouldBeQuoted &&
                            expressionText.StartsWith("\"", StringComparison.CurrentCulture) &&
                            expressionText.EndsWith("\"", StringComparison.CurrentCulture) &&
                            expressionText.IndexOf("\"", 1, StringComparison.CurrentCulture) == expressionText.Length - 1 &&
                            expressionText.IndexOf("\\", StringComparison.CurrentCulture) == -1;
                }

                // if expression is a string, we must ensure it is quoted, in case of other types - just get the converter
                if ((shouldBeQuoted && isQuotedString) || !shouldBeQuoted)
                {
                    literalValueConverter = TypeDescriptor.GetConverter(type);
                }
            }

            // if there is converter - try to convert
            if (null != literalValueConverter && literalValueConverter.CanConvertFrom(null, typeof(string)))
            {
                try
                {
                    var valueToConvert = isQuotedString ? expressionText.Substring(1, expressionText.Length - 2) : expressionText;
                    var convertedValue = literalValueConverter.ConvertFrom(null, CultureInfo.CurrentCulture, valueToConvert);

                    // ok, succeeded - create literal of given type
                    Type concreteExpType = typeof(Literal<>).MakeGenericType(type);
                    literalExpression = (ActivityWithResult)Activator.CreateInstance(concreteExpType, convertedValue);

                    // C# expression is case sensitive, if it's not exactly "true"/"false" with case matching, we don't generate Literal<bool>
                    if (type == typeof(bool) && (valueToConvert != "true") && (valueToConvert != "false"))
                    {
                        literalExpression = null;
                    }
                }
                catch
                {
                    // conversion failed - do nothing, let it continue to generate C# expression instead
                }
            }

            return literalExpression != null;
        }

        internal static bool IsLiteralExpressionSupported(Type type)
        {
            // type must be set and cannot be object
            if (null == type || typeof(object) == type)
            {
                return false;
            }

            return type.IsPrimitive || type == typeof(string) || type == typeof(TimeSpan) || type == typeof(DateTime);
        }

        internal static List<ModelItem> GetVariablesInScope(ModelItem ownerActivity)
        {
            List<ModelItem> variablesInScope = new List<ModelItem>();
            if (ownerActivity != null)
            {
                HashSet<string> variableNames = new HashSet<string>();
                ModelItem currentItem = ownerActivity;
                Func<ModelItem, bool> filterDelegate = new Func<ModelItem, bool>((variable) =>
                {
                    string variableName = (string)variable.Properties["Name"].ComputedValue;
                    if (!string.IsNullOrWhiteSpace(variableName))
                    {
                        return !variableNames.Contains(variableName);
                    }
                    else
                    {
                        return false;
                    }
                });

                while (currentItem != null)
                {
                    List<ModelItem> variables = new List<ModelItem>();
                    ModelItemCollection variablesCollection = GetVariableCollection(currentItem);
                    if (variablesCollection != null)
                    {
                        variables.AddRange(variablesCollection);
                    }

                    variables.AddRange(FindActivityDelegateArguments(currentItem));

                    // For the variables defined at the same level, shadowing doesn't apply. If there're multiple variables defined at the same level
                    // have duplicate names when case is ignored, all of these variables should bee added as variables in scope and let validation reports
                    // ambiguous reference error. So that we need to scan all variables defined at the same level first and then add names to the HashSet.                                        
                    IEnumerable<ModelItem> filteredVariables = variables.Where<ModelItem>(filterDelegate);
                    variablesInScope.AddRange(filteredVariables);
                    foreach (ModelItem variable in filteredVariables)
                    {
                        variableNames.Add(((string)variable.Properties["Name"].ComputedValue));
                    }

                    currentItem = currentItem.Parent;
                }
            }

            return variablesInScope;
        }

        private static ModelItemCollection GetVariableCollection(ModelItem currentItem)
        {
            if (null != currentItem)
            {
                Type elementType = currentItem.ItemType;
                if (!(CodeActivityType.IsAssignableFrom(elementType) || GenericAsyncCodeActivityType.IsAssignableFrom(elementType) ||
                    AsyncCodeActivityType.IsAssignableFrom(elementType) || GenericAsyncCodeActivityType.IsAssignableFrom(elementType)))
                {
                    ModelProperty variablesProperty = currentItem.Properties["Variables"];
                    if ((variablesProperty != null) && (variablesProperty.PropertyType == VariablesCollectionType))
                    {
                        return variablesProperty.Collection;
                    }
                }
            }

            return null;
        }

        private static IEnumerable<ModelItem> FindActivityDelegateArguments(ModelItem currentItem)
        {
            List<ModelItem> delegateArguments = new List<ModelItem>();
            if (currentItem.GetCurrentValue() is ActivityDelegate)
            {
                delegateArguments.AddRange(currentItem.Properties
                    .Where<ModelProperty>(p => (typeof(DelegateArgument).IsAssignableFrom(p.PropertyType) && null != p.Value))
                    .Select<ModelProperty, ModelItem>(p => p.Value));
            }

            return delegateArguments;
        }
    }
}
