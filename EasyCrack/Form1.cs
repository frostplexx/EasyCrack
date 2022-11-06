using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
namespace EasyCrack
{
    public partial class EasyCrack : Form
    {
        bool mouseDown;
        private System.Drawing.Point offset;


        public Game game;
        Dictionary<string, string> searchedGames = new Dictionary<string, string>();
        string usernamePlaceHolder = "Player";
        string gameSearchPlaceHolder = "Search for game here...";
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

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox2.Text.Equals(gameSearchPlaceHolder))
            {
                comboBox2.Text = "";
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text.Equals(""))
            {
                comboBox2.Text = gameSearchPlaceHolder;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(usernamePlaceHolder))
            {
                textBox2.Text = "";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                textBox2.Text = usernamePlaceHolder;
            }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button3.PerformClick();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //move the window to mouse pos
            this.Location = new System.Drawing.Point(Cursor.Position.X - this.Width / 2, Cursor.Position.Y - this.Height / 2);
        }

        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                System.Drawing.Point curScreenPos = PointToScreen(e.Location);
                Location = new System.Drawing.Point(curScreenPos.X - offset.X, curScreenPos.Y - offset.Y);

            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
