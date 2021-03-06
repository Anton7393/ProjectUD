﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ProjectUD
{
    public partial class AddDownloads : Form
    {
        public Manager FormManager;
        private YouTubeContext mYouTubeContext;
        private List<string> mResolutionList;
        private bool isFileNameCorrect;

        public AddDownloads()
        {
            InitializeComponent();
            mYouTubeContext = new YouTubeContext();
            mYouTubeContext.Name = "Video";
            comboBoxQuality.Enabled = false;
            textBoxName.Enabled = false;
            buttonAddDownload.Enabled = false;
            textBoxPath.Text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + @"\Downloads";
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            this.textBoxName.Focus();
            textBoxLink.Text = Clipboard.GetText();
            pictureBox2.Image = Properties.Resources.YouTube_logo_full_color;
            InspectionURL();
        }

        private void buttonAddDownload_Click(object sender, EventArgs e)
        {
            bool flsg = true;
            mYouTubeContext.Path = textBoxPath.Text;
            mYouTubeContext.Name = textBoxName.Text;
            mYouTubeContext.Date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mYouTubeContext.pathBuilder();
            if (File.Exists(this.mYouTubeContext.Path))
            {
                flsg = false;
                if (System.Windows.Forms.MessageBox.Show(
                    "Файл с таким именем уже существует.\n Желаете перезаписать?", "",
                    System.Windows.Forms.MessageBoxButtons.YesNo, 
                    System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Delete(this.mYouTubeContext.Path);
                    flsg = true;
                }
            }
            if (flsg)
            {
                if (textBoxName.Text != "")
                {
                    if (CorrectFileName(textBoxName.Text))
                        MessageBox.Show("Имя файла не должно содержать:\n" + @"               \ / : ? " + '"' + @" < > |");
                    else
                    {
                        Manager main = this.Owner as Manager;
                        if (main != null)
                        {
                            //main.AddItemListViewEx(textBoxName.Text, textBoxPath.Text, textBoxLink.Text, 0);

                        }

                        if (!Directory.Exists(textBoxPath.Text))
                        {
                            Directory.CreateDirectory(textBoxPath.Text);
                        }

                        mYouTubeContext.Path = textBoxPath.Text;
                        mYouTubeContext.Name = textBoxName.Text;
                        mYouTubeContext.pathBuilder();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Пустое имя файла");
                }
            }
        }

        private void buttonPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void textBoxLink_Leave(object sender, EventArgs e)
        {
            //InspectionURL();
        }

        private void textBoxLink_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { InspectionURL(); }
        }

        private void textBoxName_Enter(object sender, EventArgs e)
        {
            if (CorrectFileName(textBoxName.Text))
            {
                mYouTubeContext.Name = textBoxName.Text;
            }
        }

        private void comboBoxQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            mYouTubeContext.selectVideoQualuty(comboBoxQuality.SelectedIndex);
            textBoxName.Text = mYouTubeContext.Name;
            buttonAddDownload.Enabled = true;
        }

        public YouTubeContext returnContext()
        {
            return mYouTubeContext;
        }

        public void InspectionURL()
        {
            youTubeLinkConstructor();
            try
            {
                mYouTubeContext.extractYouTubeMeta(textBoxLink.Text);
                mResolutionList = mYouTubeContext.ResolutionList;
                
                if (comboBoxQuality.Items.Count == 0)
                {
                    foreach (var item in mResolutionList)
                    {
                        comboBoxQuality.Items.Add(item);
                    }
                }

                comboBoxQuality.SelectedIndex = 0;
                pictureBox2.Image = Properties.Resources.YouTube_logo_full_color;
                if (!Helper.isValidUrl(textBoxLink.Text) || !textBoxLink.Text.ToLower().Contains("www.youtube.com/watch?"))
                {
                    comboBoxQuality.Enabled = false;
                    textBoxName.Enabled = false;
                    buttonAddDownload.Enabled = false;
                    MessageBox.Show(this, "Вы ввели неверную ссылку YouTube, пожалуйста исправте её.\r\n\nПодсказка:Ссылка должна начинаться с:\r\nhttp://www.youtube.com/watch?",
                       "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                }
                else
                {
                    pictureBox2.ImageLocation = string.Format("http://i3.ytimg.com/vi/{0}/default.jpg", Helper.GetVideoIDFromUrl(textBoxLink.Text));
                    if (pictureBox2.Image != Properties.Resources.YouTubeError || pictureBox2.Image != Properties.Resources.YouTube_logo_full_color)
                    {
                        comboBoxQuality.Enabled = true;
                        textBoxName.Enabled = true;
                        
                    }
                    else
                    {
                        textBoxName.Enabled = false;
                        comboBoxQuality.Enabled = false;
                        buttonAddDownload.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxName.Enabled = false;
                comboBoxQuality.Enabled = false;
                buttonAddDownload.Enabled = false;
                MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        public bool CorrectFileName(string fileName)
        {
            if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                return true;
            }
            else
                return false;
        }

        private void youTubeLinkConstructor()
        {
            string id = "";
            string url = "";
            string origin = textBoxLink.Text;
            string filterfull = @"www.youtube.com/watch?v=";
            string filtershort = @"youtu.be/";
            Regex regfull = new Regex(filterfull);
            Regex regshort = new Regex(filtershort);
            bool isFull = regfull.IsMatch(origin);
            bool isShort = regshort.IsMatch(origin);

            if (isFull)
            {
                string pattern = @"v=(\S*)[^&]";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(origin);

                id = match.Groups[0].Value;
                url = "https://www.youtube.com/watch?v=" + id;
                textBoxLink.Text = url;
            }
            else if (isShort)
            {
                string pattern = @"be/(\S*)[^\&]";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(origin);

                var buffer = match.Groups[0].Value;
                var ids = buffer.Split('/', '?');
                id = ids[1];
                url = "https://www.youtube.com/watch?v=" + id;
                textBoxLink.Text = url;
            }
        }

        //Удалить!

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddDownloads_Load(object sender, EventArgs e)
        {

            //textBoxLink.Text = "https://www.youtube.com/watch?v=SsRYekfVxgo";
            //pictureBox2.Image = Properties.Resources.YouTube_logo_full_color;
            //InspectionURL();
            //this.textBoxName.Focus();
        }    
    }
}
