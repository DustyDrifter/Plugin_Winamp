using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Un4seen.Bass.AddOn.WaDsp;
using rdjInterface;
namespace Plugin_Winamp
{
    public partial class Config : Form
    {
        public bool active = false;
        private TextBox textBox1;
        private Button button1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button4;
        private Button button2;
        private ComboBox comboBox1;
        private Label label1;
        private ListBox listBox1;
        private Button button5;
        private Button button3;
        public Config()
        {
            this.InitializeComponent();
            this.LoadLang();
        }
        public void LoadLang()
        {
            this.Text = Lang.GetString(Var.PluginFileName, "Config");
            this.groupBox1.Text = Lang.GetString(Var.PluginFileName, "groupBox1");
            this.button1.Text = Lang.GetString(Var.PluginFileName, "button1");
            this.groupBox2.Text = Lang.GetString(Var.PluginFileName, "groupBox2");
            this.label1.Text = Lang.GetString(Var.PluginFileName, "label1");
            this.button4.Text = Lang.GetString(Var.PluginFileName, "button4");
            this.button5.Text = Lang.GetString(Var.PluginFileName, "button5");
            this.button3.Text = Lang.GetString(Var.PluginFileName, "button3");
            this.button2.Text = Lang.GetString(Var.PluginFileName, "button2");
        }
        private void Config_Load(object sender, EventArgs e)
        {
            this.active = true;
            this.textBox1.Text = Var.dsppath;
            this.button2.PerformClick();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!(Var.dsppath == ""))
            {
                //Class1.FindPlugins(Var.dsppath);
                Class1.FindPlugins(string.Concat(Application.StartupPath, "\\Plugins"));
                this.listBox1.Items.Clear();
                Class1.plugins.ForEach(delegate(WINAMP_DSP plugin)
                {
                    this.listBox1.Items.Add(plugin);
                });
                this.listBox1.SelectedIndex = Var.selectedPlugin;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = this.textBox1.Text;
            folderBrowserDialog.Description = "Winamp Plugins Folder";
            folderBrowserDialog.ShowNewFolderButton = true;
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.textBox1.Text = folderBrowserDialog.SelectedPath;
                Var.dsppath = folderBrowserDialog.SelectedPath;
                this.button2.PerformClick();
                Var.Save();
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != Var.selectedPlugin || Class1.pluginhandle == 0)
            {
                if (Class1.pluginhandle != 0 && this.listBox1.SelectedIndex != Var.selectedPlugin)
                {
                    BassWaDsp.BASS_WADSP_ChannelRemoveDSP(Class1.pluginhandle);
                    BassWaDsp.BASS_WADSP_Stop(Class1.pluginhandle);
                    BassWaDsp.BASS_WADSP_FreeDSP(Class1.pluginhandle);
                    Class1.pluginhandle = 0;
                    Class1.dsphandle = 0;
                }
                Var.selectedPlugin = this.listBox1.SelectedIndex;
                this.comboBox1.Items.Clear();
                string[] moduleNames = Class1.plugins[this.listBox1.SelectedIndex].ModuleNames;
                string[] array = moduleNames;
                for (int i = 0; i < array.Length; i++)
                {
                    string item = array[i];
                    this.comboBox1.Items.Add(item);
                }
                if (Var.selectedModule > this.comboBox1.Items.Count - 1)
                {
                    this.comboBox1.SelectedIndex = 0;
                }
                else
                {
                    this.comboBox1.SelectedIndex = Var.selectedModule;
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Class1.pluginhandle != 0)
            {
                BassWaDsp.BASS_WADSP_ChannelRemoveDSP(Class1.pluginhandle);
                BassWaDsp.BASS_WADSP_Stop(Class1.pluginhandle);
            }
            Var.selectedModule = this.comboBox1.SelectedIndex;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (Class1.pluginhandle != 0)
            {
                BassWaDsp.BASS_WADSP_Config(Class1.pluginhandle);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (Class1.pluginhandle != 0)
            {
                BassWaDsp.BASS_WADSP_ChannelRemoveDSP(Class1.pluginhandle);
                BassWaDsp.BASS_WADSP_Stop(Class1.pluginhandle);
                BassWaDsp.BASS_WADSP_FreeDSP(Class1.pluginhandle);
                Class1.pluginhandle = 0;
                Class1.dsphandle = 0;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Class1.pluginhandle == 0)
            {
                try
                {
                    Class1.pluginhandle = BassWaDsp.BASS_WADSP_Load(Class1.plugins[Var.selectedPlugin].File, 0, 0, 10, 10, null);
                    if (Class1.pluginhandle != 0)
                    {
                        BassWaDsp.BASS_WADSP_Start(Class1.pluginhandle, Var.selectedModule, Vars.MainMixer);
                        Class1.dsphandle = BassWaDsp.BASS_WADSP_ChannelSetDSP(Class1.pluginhandle, Vars.MainMixer, 10);
                        if (Var.nowPlaying != null)
                        {
                            BassWaDsp.BASS_WADSP_SetSongTitle(Class1.pluginhandle, Var.nowPlaying.ToString());
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("There was an error loading the plugin.");
                }
            }
        }
        private void Config_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Var.Save();
            base.Hide();
            this.active = false;
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            this.button5.PerformClick();
        }
        
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(331, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(343, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Parcourir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 50);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dossier des plug-ins Winamp";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(425, 292);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DSP Winamp";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Location = new System.Drawing.Point(9, 263);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "Lancer";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(90, 263);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Stopper";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(11, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(403, 199);
            this.listBox1.TabIndex = 6;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(339, 236);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Options";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(315, 263);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Actualiser";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(107, 238);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(226, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Module sélectionné:";
            // 
            // Config
            // 
            this.ClientSize = new System.Drawing.Size(449, 368);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Config";
            this.ShowIcon = false;
            this.Text = "Winamp DSP - Paramètres";
            this.Load += new System.EventHandler(this.Config_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Config_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
