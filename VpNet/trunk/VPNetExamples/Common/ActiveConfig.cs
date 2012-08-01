using System;
using System.IO;

namespace VPNetExamples.Common
{
    public class ActiveConfig<T> where T: new()
    {
        private T _config;
        private readonly FileInfo _configFile;

        public delegate void ConfigChanged(object sender, EventArgs e);

        public event ConfigChanged OnConfigChanged;

        public T Config
        {
            get { return _config; }
        }

        public ActiveConfig(FileInfo configFile)
        {
            if (configFile == null || configFile.Directory == null) throw new ArgumentNullException("configFile");
            _configFile = configFile;
            InitializeActiveConfig();
        }


        private void LoadConfig()
        {
            _config = SerializationHelpers.Deserialize<T>(_configFile);
        }

        private void InitializeActiveConfig()
        {
            LoadConfig(); 
            var watcher = new FileSystemWatcher(_configFile.Directory.FullName, _configFile.Name);
            watcher.Changed += new FileSystemEventHandler(WatcherChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void WatcherChanged(object sender, FileSystemEventArgs e)
        {
            LoadConfig();
            if (OnConfigChanged!=null)
            {
                OnConfigChanged(this, null);
            }
        }
    }
}
