using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace EasyCrack
{
    public partial class EasyCrack : Form
    {
        public Game game = new Game();
        public EasyCrack()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void browse_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                game.gamePath = openFileDialog1.FileName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            game.gamePath = textBox1.Text;
        }

        private void appid_TextChanged(object sender, EventArgs e)
        {
            game.appID = appid.Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            game.crack();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            game.playerName = textBox2.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            game.createModsFolder = checkBox1.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            game.language = comboBox1.Text;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string messageBoxText = "1. Select the folder where the game .exe file is located\n2. Go to http://steamdb.com and search your game.\n3. Enter the App ID from steamdb into Easy Crack.\n4. Enter player name, language and if you want to generate a mods folder.\n5. Click on \"Patch\" and watch the magic happen!\n\nFor more info please see: https://gitlab.com/Mr_Goldberg/goldberg_emulator";
            string caption = "Help";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
