using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public class ShelveSettingsContainer
    {
        private readonly ISettingsSource _container;

        public ShelveSettingsContainer(ISettingsSource container)
        {
            _container = container;
        }

        public string Name
        {
            get { return _container.GetValue(SettingKeys.ShelvesetName, string.Empty, x => x); }
            set { _container.SetValue(SettingKeys.ShelvesetName, value, x => x); }
        }

        public bool Overwrite
        {
            get
            {
                bool result;
                return bool.TryParse(SettingKeys.OverwriteShelveset, out result) && result;
            }
            set
            {
                _container.SetValue(SettingKeys.OverwriteShelveset, value, x => x.ToString());
            }
        }
    }
}
