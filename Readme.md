.NET Flowchart Workflow Designer
==========================

This project was built to showcase how easy it is to Re-host the .NET Windows Workflow Foundation WorkflowDesigner. 

![Alt text](flowchart-workflow-designer.png?raw=true ".NET Flowchart Workflow Designer")

The solution contains:

### WPF Desktop Application
* Workflow Designer - Rehosting in a WPF Aplication
* ToolboxControl - Loading Workflow Activities from Assemblies
* Workflow Execution - retrieve Execution Log (TrackData) and Execution Output(s)
* Workflow Management - New / Open / Save / Run

### Activity Library - Custom Activities
* ShowMessageBox - displays in a MessageBox the Value of the InputData argument
* GetGroupMembers - retrieves the Member Names and Count for a specified Meetup.Com Group
* GetRSVPmembers - retrieves the Member Names and Count for a specified Meetup.Com Event

### Demo Workflow - meetup.xaml
* InArguments - Meetup.COM REST API Key and RSVP (true / false)
* If RSVP = false - the Workflow outputs a list with the Members of a Meetup.Com Group
* If RSVP = true - the Workflow outputs a list with the Attending Members of a Meetup.Com Event

***

### A few links..
* Introduction to Windows Workflow Foundation  http://www.slideshare.net/orosandrei/introduction-to-windows-workflow-foundation-4-3 
* Windows Workflow Foundation ( WWF ) http://msdn.microsoft.com/en-us/library/dd489441(v=vs.110).aspx 
* Windows Presentation Foundation ( WPF ) http://msdn.microsoft.com/en-us/library/ms754130(v=vs.110).aspx 
* Meetup.Com REST API http://www.meetup.com/meetup_api/ 

***

* Twitter: http://twitter.com/orosandrei
* E-mail: hello@andreioros.com
* Project Showcased at Timisoara .NET Meetup 2 http://www.meetup.com/Timisoara-NET-Meetup/events/186254642/

***

Enjoy!
