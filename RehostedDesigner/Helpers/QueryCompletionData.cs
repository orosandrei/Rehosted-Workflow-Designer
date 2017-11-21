using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Microsoft.CodeAnalysis;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RehostedWorkflowDesigner.Helpers
{
    public class QueryCompletionData : ICompletionData
    {
        private static ImageSource MethodIcon;
        private static ImageSource PropertyIcon;
        private static ImageSource FieldIcon;
        private static ImageSource EventIcon;

        private IconType iconType;

        static QueryCompletionData()
        {
            MethodIcon = GetImageSourceFromResource("Method.png");
            PropertyIcon = GetImageSourceFromResource("Property.png");
            FieldIcon = GetImageSourceFromResource("Field.png");
            EventIcon = GetImageSourceFromResource("Event.png");
        }

        static internal ImageSource GetImageSourceFromResource(string resourceName)
        {
            return BitmapFrame.Create(typeof(QueryCompletionData).Assembly.GetManifestResourceStream("RehostedWorkflowDesigner.Resources.ExpressionEditor." + resourceName));
        }

        public QueryCompletionData(string name, ISymbol[] symbols)
        {
            this.Text = name;
            switch (symbols[0].Kind)
            {
                case SymbolKind.Event: iconType = IconType.Event; break;
                case SymbolKind.Field: iconType = IconType.Field; break;
                case SymbolKind.Method: iconType = IconType.Method; break;
                case SymbolKind.Property: iconType = IconType.Property; break;
            }
        }

        public ImageSource Image
        {
            get
            {
                switch (iconType)
                {
                    case IconType.Event: return EventIcon;
                    case IconType.Field: return FieldIcon;
                    case IconType.Property: return PropertyIcon;
                    default: return MethodIcon;
                }
            }
        }

        public string Text { get; private set; }
        public string HintText { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return "Description for " + this.Text; }
        }

        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }

        public double Priority
        {
            get { return 1.0; }
        }

        enum IconType
        {
            Property,
            Field,
            Method,
            Event,
        }
    }
}
