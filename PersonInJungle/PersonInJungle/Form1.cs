using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Media;

namespace PersonInJungle
{
    public partial class Form1 : Form
    {
        //private const var txtclr = scoreText.ForeColor;
        bool jumping = false; //if player jumping or not
        int jumpSpeed = 10; //to set jump speed
        int force = 20; //force of the jump
        int score = 0;
        int obstacleSpeed = 10;//default speed of obstacle
        Random rnd = new Random();// new random class created
        static bool death = false;
        

     
        public Form1()
        {
            InitializeComponent();

            resetGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void gameEvent(object sender, EventArgs e)
        {
            person.Top += jumpSpeed;
            scoreText.Text = "Score: " + score;
            if (jumping && force<0)
            {
                jumping = false;
            }
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left + x.Width < -120)
                    {
                        x.Left = this.ClientSize.Width + rnd.Next(200, 800);
                        try
                        {
                            SoundPlayer snd = new SoundPlayer(Properties.Resources.point);
                            snd.Play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error : " + ex.Message);
                        }

                        score++;
                    }
                    if (person.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        person.Image = Properties.Resources.per3;
                        scoreText.Text += " Press R to restart";
                        scoreText.ForeColor = Color.Red;
                        death = true;
                        try
                        {
                            SoundPlayer snd1 = new SoundPlayer(Properties.Resources.fearfemale);
                            SoundPlayer snd2 = new SoundPlayer(Properties.Resources.game_over_words);
                            snd1.Play();
                            //add time gap to clearly listening both voices
                            Thread.Sleep(1000);
                            snd2.Play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error : " + ex.Message);
                        }
                    }
                }
            }

            if (person.Top >= 290 && !jumping)
            {
                force = 20;
                person.Top = floor.Top - person.Height;
                jumpSpeed = 0;
            }

            if (score >= 10)
            {
                obstacleSpeed = 15;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !jumping && !death)
            {
                try
                {
                    SoundPlayer snd = new SoundPlayer(Properties.Resources.jump);
                    snd.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                resetGame();
            }
            if (jumping)
            {
                jumping = false;
            }
        }

        public void resetGame()
        {
            jumping = false; //if player jumping or not
            jumpSpeed = 10; //to set jump speed
            force = 20; //force of the jump
            score = 0;
            obstacleSpeed = 10;//default speed of obstacle
            person.Top = floor.Top - person.Height;
            scoreText.Text = "Score: " + score;
            scoreText.ForeColor = Color.ForestGreen;
            person.Image = Properties.Resources.person2;
            death = false;
            
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left = 640 + (x.Left + rnd.Next(600, 1000) + x.Width * 3);
                }
            }
            gameTimer.Start();
        }
    }
}
