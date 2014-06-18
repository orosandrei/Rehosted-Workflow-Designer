using System;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupWfIntro.Helpers
{
    /// <summary>
    /// Workflow Designer Wrapper
    /// </summary>
    static class CustomWfDesigner
    {
        private static WorkflowDesigner _wfDesigner;
        private const String _defaultWorkflow = "defaultWorkflow.xaml";

        /// <summary>
        /// Gets the current WorkflowDesigner Instance
        /// </summary>
        public static WorkflowDesigner Instance
        {
            get
            {
                if (_wfDesigner == null)
                    NewInstance(_defaultWorkflow);
                return _wfDesigner;
            }
        }

        /// <summary>
        /// Creates a new Workflow Designer instance
        /// </summary>
        /// <param name="sourceFile">Workflow FileName</param>
        public static void NewInstance(string sourceFile = _defaultWorkflow)
        {
            _wfDesigner = new WorkflowDesigner();
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;

            //associates all of the basic activities with their designers
            (new DesignerMetadata()).Register();

            //load Workflow Xaml
            _wfDesigner.Load(sourceFile);
        }
    }
}
