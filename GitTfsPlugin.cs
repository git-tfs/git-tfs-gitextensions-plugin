using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public class GitTfsPlugin : GitPluginBase
    {
        public override bool Execute(GitUIBaseEventArgs gitUiCommands)
        {
            if (string.IsNullOrEmpty(gitUiCommands.GitModule.WorkingDir))
            {
                return true;
            }

            var remotes = GetTfsRemotes(gitUiCommands.GitUICommands);

            if (remotes.Any())
            {
                new GitTfsDialog(gitUiCommands.GitUICommands, PluginSettings, remotes).ShowDialog();
                return false;
            }

            MessageBox.Show("The active repository has no TFS remotes.", "git-tfs Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            return true;
        }

        private static IEnumerable<string> GetTfsRemotes(IGitUICommands commands)
        {
            var result = commands.GitCommand("config --get-regexp tfs-remote");
            var matches = Regex.Matches(result, @"tfs-remote\.(.+)\.repository ");
            return matches.Count > 0
                       ? matches.Cast<Match>().Select(g => g.Groups[1].Value).Distinct()
                       : Enumerable.Empty<string>();
        }

        public override string Description
        {
            get { return "Git-Tfs"; }
        }

        public SettingsContainer PluginSettings
        {
            get { return new SettingsContainer(Settings); }
        }
    }
}
