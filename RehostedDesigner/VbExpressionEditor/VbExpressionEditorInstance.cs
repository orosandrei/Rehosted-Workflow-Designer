using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic;
using RehostedWorkflowDesigner.Helpers;
using System;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RehostedWorkflowDesigner.VbExpressionEditor
{
    public class VbExpressionEditorInstance : TextEditor, IExpressionEditorInstance
    {
        private CompletionWindow completionWindow;

        private TextBox textBox = new TextBox();

        public VbExpressionEditorInstance()
        {
            this.TextArea.TextEntering += TextArea_TextEntering;
            this.TextArea.TextEntered += TextArea_TextEntered;
            this.TextArea.LostKeyboardFocus += TextArea_LostFocus;

            this.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("VB");
            this.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            this.FontSize = 12;
        }

        private void TextArea_TextEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                try
                {
                    string startString = @"
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace SomeNamespace

Public Class NotAProgram 
Private Sub SomeMethod()
"
+ "var blah = ";
           
         string endString = @" 

        End Sub
    End Class
End Namespace                       
";
                    string codeString = startString + this.Text.Substring(0, this.CaretOffset) + endString;

                    var tree = VisualBasicSyntaxTree.ParseText(codeString);

                    var root = (Microsoft.CodeAnalysis.VisualBasic.Syntax.CompilationUnitSyntax)tree.GetRoot();

                    var compilation = VisualBasicCompilation.Create("CustomIntellisense")
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(tree);

                    var model = compilation.GetSemanticModel(tree);

                    var exprString = root.FindToken(codeString.LastIndexOf('.') - 1).Parent;

                    var literalInfo = model.GetTypeInfo(exprString);

                    var stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;
                    IList<ISymbol> symbols = new List<ISymbol>() { };
                    foreach (var s in (from method in stringTypeSymbol.GetMembers() where method.DeclaredAccessibility == Accessibility.Public select method).Distinct())
                        symbols.Add(s);

                    if (symbols != null && symbols.Count > 0)
                    {
                        completionWindow = new CompletionWindow(this.TextArea);
                        IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                        data.Clear();
                        var distinctSymbols = from s in symbols group s by s.Name into g select new { Name = g.Key, Symbols = g };
                        foreach (var g in distinctSymbols.OrderBy(s => s.Name))
                        {
                            data.Add(new QueryCompletionData(g.Name, g.Symbols.ToArray()));
                        }

                        completionWindow.Show();
                        completionWindow.Closed += delegate
                        {
                            completionWindow = null;
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void TextArea_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.LostAggregateFocus != null)
            {
                this.LostAggregateFocus(sender, e);
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
