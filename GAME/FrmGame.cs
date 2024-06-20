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
    public partial class FrmGame : Form
    {
        bool jumping = false, dodging = false, speedIncrease = false;
        int jumpSpeed;
        int force = 12;
        int score = 0;
        int obstacleSpeed = 10;
        Random rand = new Random();
        bool isGameOver = false;

        public FrmGame()
        {
            InitializeComponent();

            GameReset();
        }
        // Hàm game chính
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            bear.Top += jumpSpeed;

            txtScore.Text = "Score: " + score;

            // Nếu nhảy quá giới hạn sẽ dừng việc nhảy
            if (jumping == true && force < 0)
            {
                bear.Image = Properties.Resources.bearrun;
                jumping = false;
            }
            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }
            // Giữ cho vị trí người chơi luôn ổn định
            if (bear.Top > 332 && jumping == false && dodging == false)
            {
                force = 12;
                bear.Top = 333;
                jumpSpeed = 0;
            }
            if (bear.Top > 356 && dodging == true)
            {
                force = 12;
                bear.Top = 355;
                jumpSpeed = 0;
            }
            // Duyệt qua từng Control: Nếu là PictureBox với tag tương ứng sẽ random vị trí
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle1")
                {
                    x.Left -= obstacleSpeed;

                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(100, 200) + (x.Width * 5);
                        score++;
                        speedIncrease = false;
                    }
                    if (bear.Bounds.IntersectsWith(x.Bounds))
                    {
                        isGameOver = true;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "obstacle2")
                {
                    x.Left -= obstacleSpeed;

                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(100, 300) + (x.Width * 10);
                        score++;
                        speedIncrease = false;
                    }
                    if (bear.Bounds.IntersectsWith(x.Bounds))
                    {
                        isGameOver = true;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "bird1")
                {
                    x.Left -= obstacleSpeed + 5;

                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(100, 300);
                    }
                }
                if (x is PictureBox && (string)x.Tag == "bird2")
                {
                    x.Left -= obstacleSpeed + 2;

                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(100, 300);
                    }
                }
                if (score > 10)
                {
                    if (x is PictureBox && (string)x.Tag == "bee")
                    {
                        x.Left -= obstacleSpeed + 3;

                        if (x.Left < -100)
                        {
                            x.Left = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 30);
                            x.Top = rand.Next(280, 370);
                            score++;
                            speedIncrease = false;
                        }
                        if (bear.Bounds.IntersectsWith(x.Bounds))
                        {
                            isGameOver = true;
                        }
                    }
                }
            }
            // Tăng tốc độ nếu điểm là 10, 20, 30, ... và tăng tối đa đến 20
            if (score != 0 && score % 10 == 0 && speedIncrease == false && obstacleSpeed < 20)
            {
                obstacleSpeed += 2;
                speedIncrease = true;
            }
            if (isGameOver == true)
            {
                gameTimer.Stop();
                bear.Size = new Size(79, 40);
                bear.Image = Properties.Resources.deadbear;
                bear.Top = 370;
                txtScore.Text += " Press R to restart the game!";
                SaveTopScores(score); // Save the score to file
            }
        }
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (isGameOver == false)
            {
                if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) && jumping == false)
                {
                    bear.Image = Properties.Resources.bearjump;
                    jumping = true;
                }
                if (e.KeyCode == Keys.Down && dodging == false)
                {
                    bear.Size = new Size(40, 55);
                    bear.Image = Properties.Resources.beardodge;
                    bear.Top = 355;

                    dodging = true;
                }
            }
        }
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (isGameOver == false)
            {
                if (jumping == true)
                {
                    if (bear.Top < 333)
                    {
                        bear.Image = Properties.Resources.bearjump;
                    }
                    bear.Image = Properties.Resources.bearrun;
                    jumping = false;
                }
                if (dodging == true)
                {
                    bear.Size = new Size(40, 79);
                    bear.Image = Properties.Resources.bearrun;
                    bear.Top = 333;

                    dodging = false;
                }
            }
            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();
            }
        }
        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Score: " + score;
            bear.Size = new Size(40, 79);
            bear.Image = Properties.Resources.bearrun;
            isGameOver = false;
            bear.Top = 333;
            // Duyệt qua từng Control: Nếu là PictureBox với tag tương ứng sẽ random vị trí
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle1")
                {
                    x.Left = this.ClientSize.Width + rand.Next(100, 250) + (x.Width * 5);
                }
                if (x is PictureBox && (string)x.Tag == "obstacle2")
                {
                    x.Left = this.ClientSize.Width + rand.Next(100, 300) + (x.Width * 20);
                }
                if (x is PictureBox && (string)x.Tag == "bee")
                {
                    x.Left = this.ClientSize.Width + rand.Next(600, 900) + (x.Width * 35);
                    x.Top = rand.Next(280, 370);
                }
                if (x is PictureBox && (string)x.Tag == "bird1")
                {
                    x.Left = this.ClientSize.Width;
                }
                if (x is PictureBox && (string)x.Tag == "bird2")
                {
                    x.Left = this.ClientSize.Width + 400;
                }
            }
            gameTimer.Start();
        }
        // Lưu điểm
        private void SaveTopScores(int newScore)
        {
            // Đọc điểm trước đó từ file
            List<int> scores = new List<int>();
            string path = @"D:\scores.txt"; // Thay thế bằng đường dẫn đến tệp
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    int score;
                    if (int.TryParse(line, out score) && !scores.Contains(score))
                    {
                        scores.Add(score);
                    }
                }
            }

            // Thêm điểm mới vào list và xếp theo thứ tự giảm dần
            if (!scores.Contains(newScore))
            {
                scores.Add(newScore);
            }
            scores.Sort((x, y) => y.CompareTo(x));

            // Viết 5 điểm cao nhất vào file
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < Math.Min(5, scores.Count); i++)
                {
                    writer.WriteLine(scores[i]);
                }
            }
        }
        private void frmGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            this.Close();
            e.Cancel = true;
            FrmMenu menuForm = new FrmMenu();
            menuForm.Show();
        }
    }
}
