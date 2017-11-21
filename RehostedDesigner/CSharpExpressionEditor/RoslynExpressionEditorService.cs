using Microsoft.CodeAnalysis;
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace RehostedWorkflowDesigner.CSharpExpressionEditor
{
    public class RoslynExpressionEditorService : IExpressionEditorService
    {
        private static RoslynExpressionEditorService instance = new RoslynExpressionEditorService();
        private MetadataReference[] baseAssemblies = new MetadataReference[0];
        private string usingNamespaces = string.Empty;

        public static RoslynExpressionEditorService Instance
        {
            get { return instance; }
        }

        internal string UsingNamespaces
        {
            get { return usingNamespaces; }
        }

        internal MetadataReference[] BaseAssemblies
        {
            get { return baseAssemblies; }
        }

        public void CloseExpressionEditors()
        {
            
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text)
        {
            return CreateExpressionEditor(assemblies, importedNamespaces, variables, text, null, Size.Empty);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Size initialSize)
        {
            return CreateExpressionEditor(assemblies, importedNamespaces, variables, text, null, initialSize);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType)
        {
            return CreateExpressionEditor(assemblies, importedNamespaces, variables, text, expressionType, Size.Empty);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType, Size initialSize)
        {
            UpdateContext(assemblies, importedNamespaces);
            var editor = new RoslynExpressionEditorInstance(initialSize);
            editor.UpdateInstance(variables, text);
            return editor;
        }

        public void UpdateContext(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces)
        {
            var references = new List<MetadataReference>();            
            
            foreach (var assembly in assemblies.AllAssemblyNamesInContext)
            {
                System.Reflection.Assembly asm = null;
                try
                {
                    asm = System.Reflection.Assembly.Load(assembly);
                }
                catch (Exception ex) {
                    //Console.WriteLine(ex.ToString());
                }
                if(asm != null)
                    if(File.Exists(asm.Location))
                        references.Add(MetadataReference.CreateFromFile(asm.Location));
            }

            baseAssemblies = references.ToArray();

            usingNamespaces = string.Join("", importedNamespaces.ImportedNamespaces.Select(ns => "using " + ns + ";\n").ToArray());
        }
    }
}
