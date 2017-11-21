using Microsoft.CodeAnalysis;
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace RehostedWorkflowDesigner.VbExpressionEditor
{
    public class VbExpressionEditorService : IExpressionEditorService
    {
        public void CloseExpressionEditors()
        {

        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, System.Windows.Size initialSize)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType, System.Windows.Size initialSize)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public void UpdateContext(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces)
        {

        }
    }
}