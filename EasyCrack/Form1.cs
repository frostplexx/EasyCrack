using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                game.gamePath = folderBrowserDialog1.SelectedPath;
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

    }
}
