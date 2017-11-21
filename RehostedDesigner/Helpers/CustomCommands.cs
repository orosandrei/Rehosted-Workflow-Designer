using System.Windows.Input;

namespace RehostedWorkflowDesigner.Helpers
{
    /// <summary>
    /// Custom Commands
    /// </summary>
    public static class CustomCommands
    {
        public static ICommand CmdWfNewCSharp = new RoutedCommand("CmdWfNewCSharp", typeof(CustomCommands));
        public static ICommand CmdWfNewVB = new RoutedCommand("CmdWfNewVB", typeof(CustomCommands));
        public static ICommand CmdWfNew = new RoutedCommand("CmdWfNew", typeof(CustomCommands));
        public static ICommand CmdWfRun = new RoutedCommand("CmdWfRun", typeof(CustomCommands));
        public static ICommand CmdWfStop = new RoutedCommand("CmdWfStop", typeof(CustomCommands));
    }
}
