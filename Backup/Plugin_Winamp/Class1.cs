using rdjInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Un4seen.Bass.AddOn.WaDsp;
namespace Plugin_Winamp
{
    public class Class1 : IPlugin
    {
        private List<EventAction> MyEvents;
        private Config config;
        public static List<WINAMP_DSP> plugins;
        public static int pluginhandle = 0;
        public static int dsphandle = 0;
        public static IntPtr win;
        string IPlugin.PluginName
        {
            get
            {
                return Var.PluginName;
            }
        }
        string IPlugin.PluginTitle
        {
            get
            {
                return "Winamp DSP";
            }
        }
        string IPlugin.PluginDescription
        {
            get
            {
                return "Utiliser des DSP Winamp dans RadioDJ";
            }
        }
        string IPlugin.PluginVersion
        {
            get
            {
                return "0.1";
            }
        }
        int IPlugin.PluginZone
        {
            get
            {
                return 0;
            }
        }
        bool IPlugin.NotifyOnTrackChange
        {
            get
            {
                return true;
            }
        }
        bool IPlugin.NotifyOnUIChange
        {
            get
            {
                return false;
            }
        }
        bool IPlugin.NotifyOnPlaylistChange
        {
            get
            {
                return false;
            }
        }
        bool IPlugin.HasActions
        {
            get
            {
                return false;
            }
        }
        public Class1()
        {
            this.MyEvents = new List<EventAction>();
        }
        ~Class1()
        {
            Var.Save();
            if (Class1.dsphandle != 0)
            {
                BassWaDsp.BASS_WADSP_ChannelRemoveDSP(Class1.pluginhandle);
            }
            if (Class1.pluginhandle != 0)
            {
                BassWaDsp.BASS_WADSP_Stop(Class1.pluginhandle);
                BassWaDsp.BASS_WADSP_FreeDSP(Class1.pluginhandle);
            }
            BassWaDsp.BASS_WADSP_Free();
            BassWaDsp.FreeMe();
        }
        void IPlugin.ShowMain()
        {
        }
        void IPlugin.ShowConfig()
        {
            this.config.LoadLang();
            if (this.config != null && this.config.active)
            {
                this.config.Focus();
            }
            else
            {
                this.config.Show();
            }
        }
        void IPlugin.ShowAbout()
        {
            int num = (int)MessageBox.Show("Plugin Winamp DSP pour Radio DJ - Copyright 2012 Stéphane Lepin");
        }
        void IPlugin.ReloadLanguage()
        {
            MessageBox.Show("Reload !");
        }
        void IPlugin.ReloadCategories()
        {
        }
        public void Initialize(IHost Host)
        {
            Var.MyHost = Host;
            Var.PluginFileName = Assembly.GetExecutingAssembly().GetName().Name;
            Var.Load();
            Class1.win = Process.GetCurrentProcess().MainWindowHandle;
            BassWaDsp.LoadMe();
            BassWaDsp.BASS_WADSP_Init(Class1.win);
            this.config = new Config();
        }
        void IPlugin.TrackChanged(SongData dtr)
        {
            Var.nowPlaying = dtr;
            if (Class1.pluginhandle != 0)
            {
                BassWaDsp.BASS_WADSP_SetSongTitle(Class1.pluginhandle, dtr.ToString());
            }
        }
        public void AddTrack2Plugin(int trackID, long TriggerOn)
        {
            AddTrack2Plugin(trackID, TriggerOn, -1);
        }


        public void AddTrack2Plugin(int trackID, long TriggerOn, int Position)
        {

        }


        public void AddTrack2Plugin(SongData trackID, long TriggerOn)
        {
            AddTrack2Plugin(trackID, TriggerOn, -1);
        }


        public void AddTrack2Plugin(SongData trackID, long TriggerOn, int Position)
        {

        }
        void IPlugin.PlaylistChanged()
        {
        }
        void IPlugin.AutoDJStateChanged(bool newState)
        {
        }
        void IPlugin.AssistedStateChanged(bool newState)
        {
        }
        void IPlugin.InputStateChanged(bool newState)
        {
        }
        List<EventAction> IPlugin.AvailableActions()
        {
            return this.MyEvents;
        }
        bool IPlugin.RunAction(string ActionName, string[] args)
        {
            return true;
        }
        public UserControl LoadGUI()
        {
            return null;
        }
        public void KeyDown(object sender, KeyEventArgs e)
        {
            //keyboard key presses with CTRL are processed here

        }
        public static void FindPlugins(string path)
        {
            WINAMP_DSP.FindPlugins(path);
            Class1.plugins = WINAMP_DSP.PlugIns;
        }
    }
}
