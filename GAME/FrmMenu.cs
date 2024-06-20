using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAME
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            FrmGame frmGame = new FrmGame();
            frmGame.Show();

            // Đóng form hiện tại
            this.Hide();
        }
        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
                Application.Exit();
            }
            else return;
        }
        private void frmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            if (e.Cancel == false)
            {
                this.Dispose();
                Application.Exit();
            }
        }
        private void btnHighScore_Click(object sender, EventArgs e)
        {
            // Read the top 5 scores from the file
            List<int> scores = new List<int>();
            string path = @"D:\scores.txt"; // Replace with the path to your file
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    int score;
                    if (int.TryParse(line, out score))
                    {
                        scores.Add(score);
                    }
                }
            }
            scores.Sort((x, y) => y.CompareTo(x));
            scores = scores.Take(5).ToList(); // Take the top 5 scores

            // Display the scores in a message box
            string message = "Top 5 High Scores:\n\n";
            for (int i = 0; i < scores.Count; i++)
            {
                message += $"Top{i + 1}.     {scores[i]}\n";
            }
            MessageBox.Show(message, "High Scores", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
