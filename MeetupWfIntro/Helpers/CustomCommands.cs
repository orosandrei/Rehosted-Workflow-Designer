using System.Windows.Input;

namespace MeetupWfIntro.Helpers
{
    /// <summary>
    /// Custom Commands
    /// </summary>
    public static class CustomCommands
    {
        public static ICommand CmdWfNewCSharp = new RoutedCommand("CmdWfNewCSharp", typeof(CustomCommands));
        public static ICommand CmdWfRun = new RoutedCommand("CmdWfRun", typeof(CustomCommands));
        public static ICommand CmdWfStop = new RoutedCommand("CmdWfStop", typeof(CustomCommands));
    }
}
