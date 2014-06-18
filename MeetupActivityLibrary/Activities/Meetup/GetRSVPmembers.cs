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
using System.Net;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace MeetupWfIntro.MeetupActivityLibrary.MeetupAPI
{
    /// <summary>
    /// Custom Activity that retrieves the Member Names and Count for a specified Meetup.Com Event
    /// </summary>
    public class GetRSVPmembers : CodeActivity
    {
        #region Arguments
        /// <summary>
        /// Meetup.Com API Key
        /// </summary>
        [RequiredArgument]
        public InArgument<string> ApiKey { get; set; }

        /// <summary>
        /// ID of the Meetup.Com event
        /// </summary>
        [RequiredArgument]
        public InArgument<string> EventID { get; set; }

        /// <summary>
        /// Total Number of Members of the specified Meetup Group
        /// </summary>
        public OutArgument<Int32> MembersCount { get; set; }

        /// <summary>
        /// Member Names of the specified Meetup Group
        /// </summary>
        public OutArgument<Collection<String>> MembersNames { get; set; }
        
        /// <summary>
        /// Raw response from the Meetup REST API
        /// </summary>
        public OutArgument<string> RawResponse { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public GetRSVPmembers()
            : base()
        {
            EventID = "186254642";
        }

        /// <summary>
        /// Execution Logic
        /// </summary>
        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                MembersCount.Set(context, 0);
                MembersNames.Set(context, new Collection<String>() { });

                string host = "https://api.meetup.com/2/rsvps?event_id={0}&sign=true&key={1}";
                string url = String.Format(host, EventID.Get(context), ApiKey.Get(context));
                string response = String.Empty;

                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

                if (null == request)
                {
                    throw new ApplicationException("HttpWebRequest failed");
                }
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                }

                RawResponse.Set(context, response);

                var names = new Collection<String>() { };

                JObject o = JObject.Parse(response);
                JArray a = (JArray)o["results"];

                foreach (var item in a)
                {
                    names.Add(item["member"]["name"].ToString());
                }

                MembersCount.Set(context, a.Count);
                MembersNames.Set(context, names);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
