using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;

namespace RehostedWorkflowDesigner.CSharpExpressionEditor
{
    internal class ExpressionSelection : ContextItem
    {
        ModelItem modelItem;

        public ExpressionSelection()
        {
        }

        public ExpressionSelection(ModelItem modelItem)
        {
            this.modelItem = modelItem;
        }

        public ModelItem ModelItem
        {
            get { return this.modelItem; }
        }

        public override Type ItemType
        {
            get
            {
                return typeof(ExpressionSelection);
            }
        }
    }
}