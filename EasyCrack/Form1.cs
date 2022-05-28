using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace EasyCrack
{
    public partial class EasyCrack : Form
    {
        public Game game;
        Dictionary<string, string> searchedGames = new Dictionary<string, string>();
        public EasyCrack()
        {
            InitializeComponent();
            game = new Game(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            game.crack();
        }


        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string messageBoxText = "1. Select the folder where the game .exe file is located\n2.Search your game to automatically find the App ID or go to http://steamdb.com and search your game.\n3. Enter the App ID from steamdb into Easy Crack if you did not choose to let EasyCrack search the game for you.\n4. Enter player name, language and if you want to generate a mods folder.\n5. Click on \"Patch\" and watch the magic happen!\n\nFor more info please see: https://gitlab.com/Mr_Goldberg/goldberg_emulator and ";
            string caption = "Help";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            game.createModsFolder = checkBox1.Checked;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void browse_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                game.gamePath = openFileDialog1.FileName;
            }
        }

        private void appid_TextChanged_1(object sender, EventArgs e)
        {
            game.appID = appid.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            game.playerName = textBox2.Text;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            game.gamePath = textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            searchedGames = game.SearchGame(comboBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            game.updateAppID(comboBox2.Items[comboBox2.SelectedIndex].ToString(), searchedGames);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
