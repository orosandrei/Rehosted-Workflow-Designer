using System;
using System.Collections.Generic;
using System.Text;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.Toolbox;
using System.Activities.Statements;
using System.ServiceModel.Activities;
using System.Activities.Presentation.Validation;
using Microsoft.CSharp.Activities;
using System.Activities.XamlIntegration;
using System.Activities.Tracking;
using System.Windows.Forms;
using System.ComponentModel;

namespace MeetupWfIntro.MeetupActivityLibrary.Misc
{
    /// <summary>
    /// Custom Activity that displays in a MessageBox the Value of the InputData argument
    /// </summary>
    public sealed class ShowMessageBox : CodeActivity
    {
        #region Arguments
        
        public InArgument<Object> InputData { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ShowMessageBox():base()
        {
            this.DisplayName = "Message";
        }

        /// <summary>
        /// Execution Logic
        /// </summary>
        protected override void Execute(CodeActivityContext context)
        {
            MessageBox.Show(this.InputData.Get(context).ToString());
        }
    }
}
