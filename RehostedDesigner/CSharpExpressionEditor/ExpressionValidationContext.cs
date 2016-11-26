using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Collections.Generic;

namespace RehostedWorkflowDesigner.CSharpExpressionEditor
{
    public class ExpressionValidationContext
    {
        private RoslynExpressionEditor etb;

        public ExpressionValidationContext(RoslynExpressionEditor etb)
        {
            this.etb = etb;
        }

        internal ParserContext ParserContext { get; set; }
        internal Type ExpressionType { get; set; }
        internal String ExpressionText { get; set; }
        internal EditingContext EditingContext { get; set; }
        internal String ValidatedExpressionText { get; set; }
        internal bool UseLocationExpression { get; set; }

        internal void Update(RoslynExpressionEditor etb)
        {
            this.EditingContext = etb.OwnerActivity.GetEditingContext();

            //setup ParserContext
            this.ParserContext = new ParserContext(etb.OwnerActivity)
            {
                //callee is a ExpressionTextBox
                Instance = etb,
                //pass property descriptor belonging to epression's model property (if one exists)
                //TODO: etb should have expressionModelProperty and the propertyDescriptor should be used here instead of passing null
                PropertyDescriptor = null,
            };

            if (etb.ExpressionType != null)
            {
                this.ExpressionType = etb.ExpressionType;
            }

            this.ValidatedExpressionText = this.ExpressionText;
            if (etb.ExpressionEditorInstance != null)
            {
                this.ExpressionText = etb.ExpressionEditorInstance.Text;
            }
            else if (etb.EditingTextBox != null)
            {
                this.ExpressionText = etb.EditingTextBox.Text;
            }
            else
            {
                this.ExpressionText = etb.Text;
            }
            this.UseLocationExpression = etb.UseLocationExpression;
        }

        internal IEnumerable<string> ReferencedAssemblies
        {
            get
            {
                AssemblyContextControlItem assemblyContext = this.EditingContext.Items.GetValue<AssemblyContextControlItem>();
                if (assemblyContext != null)
                {
                    return assemblyContext.AllAssemblyNamesInContext;
                }
                return null;
            }
        }

    }
}
