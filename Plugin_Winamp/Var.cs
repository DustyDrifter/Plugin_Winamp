using rdjInterface;
using System;
namespace Plugin_Winamp
{
    public sealed class Var
    {
        internal static IHost MyHost;
        internal static string PluginFileName;
        internal static string PluginName = "Winamp DSP";
        internal static string dsppath = "";
        internal static int selectedPlugin = -1;
        internal static int selectedModule = -1;
        internal static SongData nowPlaying = null;
        public static void Save()
        {
            Var.MyHost.SaveSetting(Var.PluginFileName, "dsppath", Var.dsppath);
            Var.MyHost.SaveSetting(Var.PluginFileName, "selectedPlugin", Var.selectedPlugin.ToString());
            Var.MyHost.SaveSetting(Var.PluginFileName, "selectedModule", Var.selectedModule.ToString());
        }
        public static void Load()
        {
            Var.dsppath = Var.MyHost.GetSetting(Var.PluginFileName, "dsppath","0");
            Var.selectedPlugin = Var.StringToInt(Var.MyHost.GetSetting(Var.PluginFileName, "selectedPlugin", "0"));
            Var.selectedModule = Var.StringToInt(Var.MyHost.GetSetting(Var.PluginFileName, "selectedModule", "0"));
        }
        private static int StringToInt(string text)
        {
            int result;
            if (string.IsNullOrEmpty(text))
            {
                result = 0;
            }
            else
            {
                result = int.Parse(text);
            }
            return result;
        }
    }
}
