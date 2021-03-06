﻿using GitUIPluginInterfaces;
using ResourceManager;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GitTfs.GitExtensions.Plugin
{
    public class GitTfsPlugin : GitPluginBase, IGitPluginForRepository
    {

        public GitTfsPlugin()
        {
            SetNameAndDescription("Git-TFS");
            Translate();
        }

        public override bool Execute(GitUIBaseEventArgs gitUiCommands)
        {
            if (string.IsNullOrEmpty(gitUiCommands.GitModule.WorkingDir))
            {
                return false;
            }

            var remotes = GetTfsRemotes(gitUiCommands.GitUICommands);

            if (remotes.Any())
            {
                new GitTfsDialog(gitUiCommands.GitUICommands, PluginSettings, remotes).ShowDialog();
                return true;
            }

            MessageBox.Show("The active repository has no TFS remotes.", "Git-TFS Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private static IEnumerable<string> GetTfsRemotes(IGitUICommands commands)
        {
            var result = commands.GitCommand("config --get-regexp tfs-remote");
            var matches = Regex.Matches(result, @"tfs-remote\.(.+)\.repository ");
            return matches.Count > 0
                       ? matches.Cast<Match>().Select(g => g.Groups[1].Value).Distinct()
                       : Enumerable.Empty<string>();
        }

        public SettingsContainer PluginSettings
        {
            get { return new SettingsContainer(Settings); }
        }
    }
}
