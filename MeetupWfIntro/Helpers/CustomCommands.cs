using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetupWfIntro.Helpers
{
    /// <summary>
    /// Custom Commands
    /// </summary>
    public static class CustomCommands
    {
        public static ICommand CmdWfRun = new RoutedCommand("CmdWfRun", typeof(CustomCommands));
    }
}
