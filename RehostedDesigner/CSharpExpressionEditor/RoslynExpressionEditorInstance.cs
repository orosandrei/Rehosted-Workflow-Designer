using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RehostedWorkflowDesigner.CSharpExpressionEditor
{
    public class RoslynExpressionEditorInstance : TextEditor, IExpressionEditorInstance
    {
        private CompletionWindow completionWindow;
        private string variableDeclarations;

        public RoslynExpressionEditorInstance()
        {
            this.TextArea.TextEntering += TextArea_TextEntering;
            this.TextArea.TextEntered += TextArea_TextEntered;
            this.TextArea.LostKeyboardFocus += TextArea_LostFocus; // Need to detach events.

            this.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            this.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            this.FontSize = 12;
        }

        public RoslynExpressionEditorInstance(Size initialSize) : this()
        {
            this.Width = initialSize.Width;
            this.Height = initialSize.Height;
        }

        public void UpdateInstance(List<ModelItem> variables, string text)
        {
            this.Text = text;

            variableDeclarations = string.Join("", variables.Select(v => {
                var c = v.GetCurrentValue() as System.Activities.Variable;
                return c.Type.FullName + " " + c.Name + ";\n";
            }).ToArray());
        }

        private void TextArea_TextEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                try
                {
                    string startString = RoslynExpressionEditorService.Instance.UsingNamespaces 
                        + "namespace SomeNamespace { public class NotAProgram { private void SomeMethod() { " 
                        + variableDeclarations + "var blah = ";
                    //string endString = " } } }";

                    var tree = CSharpSyntaxTree.ParseText(startString + this.Text.Substring(0, this.CaretOffset));
                    var compilation = CSharpCompilation.Create(
                        "MyCompilation",
                        syntaxTrees: new[] { tree },
                        references: RoslynExpressionEditorService.Instance.BaseAssemblies);
                    var semanticModel = compilation.GetSemanticModel(tree);

                    // Ask for symbols at the caret position.
                    var position = this.CaretOffset + startString.Length - 1;
                    var token = tree.GetRoot().FindToken(position);
                    var identifier = token.Parent;
                    IList<ISymbol> symbols = null;
                    if (identifier is QualifiedNameSyntax)
                    {
                        var semanticInfo = semanticModel.GetTypeInfo((identifier as QualifiedNameSyntax).Left);
                        var type = semanticInfo.Type;
                        symbols = semanticModel.LookupSymbols(position, container: type, includeReducedExtensionMethods: true);
                    }
                    else if (identifier is MemberAccessExpressionSyntax)
                    {
                        var semanticInfo = semanticModel.GetTypeInfo((identifier as MemberAccessExpressionSyntax).Expression);
                        var type = semanticInfo.Type;
                        symbols = semanticModel.LookupSymbols(position, container: type, includeReducedExtensionMethods: true);
                    }
                    else if (identifier is IdentifierNameSyntax)
                    {
                        var semanticInfo = semanticModel.GetTypeInfo(identifier as IdentifierNameSyntax);
                        var type = semanticInfo.Type;
                        symbols = semanticModel.LookupSymbols(position, container: type, includeReducedExtensionMethods: true);
                    }

                    if (symbols != null && symbols.Count > 0)
                    {
                        completionWindow = new CompletionWindow(this.TextArea);
                        IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                        //var distinctSymbols = (from s in symbols select s.Name).Distinct();
                        var distinctSymbols = from s in symbols group s by s.Name into g select new { Name = g.Key, Symbols = g };
                        foreach (var group in distinctSymbols.OrderBy(s => s.Name))
                        {
                            data.Add(new QueryCompletionData(group.Name, group.Symbols.ToArray()));
                        }

                        completionWindow.Show();
                        completionWindow.Closed += delegate
                        {
                            completionWindow = null;
                        };
                    }
                }
                catch { }
            }
        }

        private void TextArea_TextEntering(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void TextArea_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.LostAggregateFocus != null)
            {
                this.LostAggregateFocus(sender, e);
            }
        }

        #region IExpressionEditorInstance implicit

        public bool AcceptsReturn { get; set; }

        public bool AcceptsTab { get; set; }

        public bool HasAggregateFocus
        {
            get
            {
                return true;
            }
        }

        public Control HostControl
        {
            get
            {
                return this;
            }
        }

        public int MaxLines { get; set; }

        public int MinLines { get; set; }

        public event EventHandler Closing;
        public event EventHandler GotAggregateFocus;
        public event EventHandler LostAggregateFocus;

        public bool CanCompleteWord()
        {
            return true;
        }

        public bool CanCopy()
        {
            return true;
        }

        public bool CanCut()
        {
            return true;
        }

        public bool CanDecreaseFilterLevel()
        {
            return true;
        }

        public bool CanGlobalIntellisense()
        {
            return true;
        }

        public bool CanIncreaseFilterLevel()
        {
            return true;
        }

        public bool CanParameterInfo()
        {
            return true;
        }

        public bool CanPaste()
        {
            return true;
        }

        public bool CanQuickInfo()
        {
            return true;
        }

        public void ClearSelection()
        {

        }

        public void Close()
        {

        }

        public bool CompleteWord()
        {
            return true;
        }

        public string GetCommittedText()
        {
            return this.Text;
        }

        public bool GlobalIntellisense()
        {
            return true;
        }
        public bool DecreaseFilterLevel()
        {
            return true;
        }

        public bool IncreaseFilterLevel()
        {
            return true;
        }

        public bool ParameterInfo()
        {
            return true;
        }

        public bool QuickInfo()
        {
            return true;
        }

        #endregion

        #region IExpressionEditorInstance explicit

        void IExpressionEditorInstance.Focus()
        {
            base.Focus();
        }

        bool IExpressionEditorInstance.Cut()
        {
            base.Cut();
            return true;
        }

        bool IExpressionEditorInstance.Copy()
        {
            base.Copy();
            return true;
        }

        bool IExpressionEditorInstance.Paste()
        {
            base.Paste();
            return true;
        }

        bool IExpressionEditorInstance.Undo()
        {
            return base.Undo();
        }

        bool IExpressionEditorInstance.Redo()
        {
            return base.Redo();
        }

        bool IExpressionEditorInstance.CanUndo()
        {
            return base.CanUndo;
        }

        bool IExpressionEditorInstance.CanRedo()
        {
            return base.CanRedo;
        }

        event EventHandler IExpressionEditorInstance.TextChanged
        {
            add
            {
                base.TextChanged += value;
            }

            remove
            {
                base.TextChanged -= value;
            }
        }

        string IExpressionEditorInstance.Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
            }
        }

        ScrollBarVisibility IExpressionEditorInstance.VerticalScrollBarVisibility
        {
            get
            {
                return base.VerticalScrollBarVisibility;
            }

            set
            {
                base.VerticalScrollBarVisibility = value;
            }
        }

        ScrollBarVisibility IExpressionEditorInstance.HorizontalScrollBarVisibility
        {
            get
            {
                return base.HorizontalScrollBarVisibility;
            }

            set
            {
                base.HorizontalScrollBarVisibility = value;
            }
        }


        #endregion
    }
}
