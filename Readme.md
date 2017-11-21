# .NET Rehosted Workflow Designer #

![Alt text](https://github.com/orosandrei/Rehosted-Workflow-Designer/raw/master/rehosted-workflow-designer.png?raw=true ".NET Rehosted Workflow Designer")

The solution contains:

## WPF Desktop Application ##
* Workflow Designer - Rehosting in a WPF Aplication 
* ToolboxControl - Loading Workflow Activities from Assemblies
* Workflow Execution - retrieve real-time Execution Log (TrackData) and Execution Output(s)
* Workflow Management - New / Open / Save / Run / Stop

## Activity Library - Custom Activities ##
* ShowMessageBox - displays in a MessageBox the Value of the InputData argument
* GetGroupMembers - retrieves the Member Names and Count for a specified Meetup.Com Group
* GetRSVPmembers - retrieves the Member Names and Count for a specified Meetup.Com Event

## Demo Workflows ##
### AzureVmPowerOperations.xaml ###
* InArguments - VM & Service names
* OutArguments - ActionPerformed
* the workflow connects to Azure & changes the VM power state: if Powered On it will be power off, else powered on

### LocalWinServicesCSV.xaml ###
* InArguments - Status (default is "running")
* the workflow retrieves the local windows services with the status defined by the inargument, writes the list to a file & opens it

### SvcMonitoring.xaml ###
* InArguments - Service
* OutArguments - Log
* the state machine workflows monitors the state of the specified windows service; if the state changes, the user gets notified via SMS

### Meetup.xaml ###
* InArguments - Meetup.COM REST API Key and RSVP (true / false)
* If RSVP = false - the Workflow outputs a list with the Members of a Meetup.Com Group
* If RSVP = true - the Workflow outputs a list with the Attending Members of a Meetup.Com Event

***

## Links ##
* (My presentation at Microsoft Summit 2015) [Introduction to Windows Workflow Foundation](http://www.slideshare.net/orosandrei/windows-workflow-foundation-54773529)
* Blog post about the WF Designer demo &amp; [Windows Workflow Foundation](http://andreioros.com/blog/windows-workflow-foundation-rehosted-designer/)
* Project Showcased at [Microsoft Summit 2015](http://andreioros.com/blog/workflow-foundation-microsoft-summit/#more-92) & [Timisoara .NET Meetup 2](http://www.meetup.com/Timisoara-NET-Meetup/events/186254642/)
* Twitter [@orosandrei](http://twitter.com/orosandrei)

***

* MSDN [Windows Workflow Foundation](http://msdn.microsoft.com/en-us/library/dd489441(v=vs.110).aspx)
* MSDN [What's new in WF 4.5](https://msdn.microsoft.com/en-us/library/hh305677.aspx)
* [Roslyn C# Expression Editor](https://github.com/dmetzgar/wf-rehost-roslyn)
* [Custom Expression Editor](https://blogs.msdn.microsoft.com/cathyk/2009/11/05/implementing-a-custom-expression-editor/)
* [Expression Editing Mechanics](https://blogs.msdn.microsoft.com/cathyk/2009/11/09/expression-editing-mechanics/)
* [Avalon Edit](https://github.com/icsharpcode/AvalonEdit)
