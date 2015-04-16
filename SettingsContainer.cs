using System;
using System.Linq;
using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public class SettingsContainer
    {
        private readonly ISettingsSource _container;

        public SettingsContainer(ISettingsSource container)
        {
            _container = container;
        }

        public string TfsRemote
        {
            get { return _container.GetValue(SettingKeys.TfsRemote, null , x => x); }
            set { _container.SetValue(SettingKeys.TfsRemote, value, x => x); }
        }

        public PullSetting? PullSetting
        {
            get { return GetEnumSettingValue<PullSetting>(SettingKeys.Pull); }
            set { SetEnumSettingValue(SettingKeys.Pull, value); }
        }

        public PushSetting? PushSetting
        {
            get { return GetEnumSettingValue<PushSetting>(SettingKeys.Push); }
            set { SetEnumSettingValue(SettingKeys.Push, value); }
        }

        public ShelveSettingsContainer ShelveSettings
        {
            get { return new ShelveSettingsContainer(_container); }
        }

        private T? GetEnumSettingValue<T>(string key)
            where T : struct
        {
            var type = typeof (T);
            return _container.GetValue(key, default(T), x => (from name in Enum.GetNames(type)
                                                                   where name == x
                                                                   select (T?) Enum.Parse(type, name)).FirstOrDefault());
        }

        private void SetEnumSettingValue<T>(string key, T? value)
            where T : struct
        {
            _container.SetValue(key, value, x => x.HasValue ? x.ToString() : string.Empty);
        }
    }
}
