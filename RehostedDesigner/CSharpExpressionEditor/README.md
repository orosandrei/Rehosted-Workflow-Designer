# WF Rehosted Designer with Roslyn C# Expression Editor

The rehostable designer in Workflow Foundation is limited to only allow Visual Basic expressions. This project
is to show how it is possible to use Roslyn for C# expressions. Roslyn is used to parse the code into syntax 
trees that can be used to fill completion windows. The user interface is powered by AvalonEdit.

High level steps needed to implement the custom expression editor:

1.	Implement the IExpressionEditorService interface. This interface manages the creation and destruction of expression editors.
2.	Implement the IExpressionEditorInstance interface. This interface implements the UI for expression editing UI.
3.	Publish the IExpressionEditorService in your rehosted workflow application.
4.	Implement a custom Expression Activity Editor with C# expression support a WPF based text editor component like AvalonEdit (https://github.com/icsharpcode/AvalonEdit)  and register it to the ExpressionTextBox used by workflow designer.
5.	Use Roslyn to parse the code into syntax trees that can be used for IntelliSense. 

While it is not a fully working custom expression editor implementation, it implements a custom Expression Activity Editor and shows how to work around below .NET workflow runtime issues you hit attempting above steps. 
1)	Expression Editor defaults to VB expressions when implementing custom expression editor. 
	When rehosting the workflow designer, the workflow runtime uses .NET framework 4.0 by default and this should be set to .NET 4.5 or higher to use the C# expression support. You can do this by using DesignerConfigurationService class as shown below.

		DesignerConfigurationService configurationService = wd.Context.Services.GetService<DesignerConfigurationService>();
		configurationService.TargetFrameworkName = new FrameworkName(".NETFramework", new System.Version(4, 5));


2)	By default, workflow runtime does not allow loading XAML from untrusted source. 
	You can again use the DesignerConfigurationService class to configure the workflow designer as shown below.

		configurationService.LoadingFromUntrustedSourceEnabled = true;

3)	When loading custom XAML, workflow runtime defaults to VB in rehosted designer. 
	Set the below XAML attached property to indicate workflow runtime that you are using C# expressions in your workflow.

		sap2010:ExpressionActivityEditor.ExpressionActivityEditor="C#"

## Links to useful resources

[Rehosting the Workflow Designer](https://msdn.microsoft.com/en-us/library/dd489451.aspx)

[Using a Custom Expression Editor](https://msdn.microsoft.com/en-us/library/ff521564.aspx)

[AvalonEdit on GitHub](https://github.com/icsharpcode/AvalonEdit)
