using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form : System.Windows.Forms.Form
    {
        private int dirX, dirY;         //The direction of movement of the snake
        private int fruitX, fruitY;         //Coordinates of food
        private const int sizeX = 40;           //The size of the playing field
        private const int sizeY = 24;
        private const int sizeOfSides = 20;         //Cube size
        private const int startScore = 2;           //Initial score (affects the initial length of the snake)
        private int score = startScore;         //General score
        private const int scoreMax = 100;           //Maximum score (game over)
        private const int intervalTime = 100;           //Snake movement interval (millisevonds)
        private bool snakeIsMoving = false;
        private Random rand = new Random();         //For food generation
        private PictureBox[] Snake = new PictureBox[scoreMax + startScore + 1];         //Snake head and tail
        private PictureBox fruit = new PictureBox();            //Food
        private PictureBox scoreStrip = new PictureBox();           //Top bar
        private SoundPlayer eatSound = new SoundPlayer();         //The sound of eating food
        private SoundPlayer gameOverSound = new SoundPlayer();           //The sound of game over
        public Form()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(MoveSnake);
            timer.Interval = intervalTime;
            timer.Start();
            this.KeyDown += new KeyEventHandler(Controller);
            Start();
        }
        private void Start()
        {
            eatSound.Stream = Properties.Resources.eat;
            gameOverSound.Stream = Properties.Resources.game_over;
            dirX = 1; dirY = 0;
            panel.BackColor = Color.FromArgb(255, 230, 100);
            panelTop.BackColor = Color.WhiteSmoke;

            scoreStrip.Location = new Point(0, 0);
            scoreStrip.Size = new Size(panelTop.Width / scoreMax * (score - startScore), panelTop.Height);
            scoreStrip.BackColor = Color.Red;
            panelTop.Controls.Add(scoreStrip);

            for (int i = 0; i <= startScore; i++)
            {
                Snake[i] = new PictureBox();
                if (i == 0)
                {
                    Snake[i].Location = new Point(sizeX / 2 * sizeOfSides, sizeY / 2 * sizeOfSides);
                    Snake[i].BackColor = Color.FromArgb(0, 140, 0);
                } else
                {
                    Snake[i].Location = new Point(Snake[i - 1].Location.X - sizeOfSides, Snake[i - 1].Location.Y);
                    Snake[i].BackColor = Color.FromArgb(i, 200 + (i / 4), 0);
                }
                Snake[i].Size = new Size(sizeOfSides, sizeOfSides);
                panel.Controls.Add(Snake[i]);
            }

            GenerateFruit();
            fruit.Size = new Size(sizeOfSides, sizeOfSides);
            fruit.BackColor = Color.Red;
            panel.Controls.Add(fruit);
        }
        private void MoveSnake(Object myObject, EventArgs eventsArgs)
        {
            if (Snake[0].Location.X == fruit.Location.X && Snake[0].Location.Y == fruit.Location.Y)
            {
                EatFruit();
            }
            if (Snake[0].Location.X / sizeOfSides < 0 || Snake[0].Location.X / sizeOfSides >= sizeX ||
                Snake[0].Location.Y / sizeOfSides < 0 || Snake[0].Location.Y / sizeOfSides >= sizeY)
            {
                GameOver();
            }
            for (int i = 1; i < score + 1; i++)
            {
                if (Snake[0].Location.X == Snake[i].Location.X && Snake[0].Location.Y == Snake[i].Location.Y)
                {
                    GameOver();
                }
            }
            if (snakeIsMoving)
            {
                for (int i = score; i > 0; i--)
                {
                    Snake[i].Location = Snake[i - 1].Location;
                }
                Snake[0].Location = new Point(Snake[0].Location.X + dirX * sizeOfSides, Snake[0].Location.Y + dirY * sizeOfSides);
            }
        }
        private void GameOver()
        {
            gameOverSound.Play();
            snakeIsMoving = false;
            Thread.Sleep(1000);
            for (int i = score; i >= 0; i--)
            {
                panel.Controls.Remove(Snake[i]);
                Thread.Sleep(100);
            }
            panelTop.Controls.Remove(scoreStrip);
            score = startScore;
            Start();
        }
        private void GenerateFruit()
        {
            fruitX = rand.Next(sizeX);
            fruitY = rand.Next(sizeY);
            fruit.Location = new Point(fruitX * sizeOfSides, fruitY * sizeOfSides);
            for (int i = 0; i < score + 1; i++)
            {
                if (fruit.Location.X == Snake[i].Location.X && fruit.Location.Y == Snake[i].Location.Y)
                {
                    GenerateFruit();
                }
            }
        }
        private void EatFruit()
        {
            eatSound.Play();
            GenerateFruit();
            score++;
            scoreStrip.Size = new Size(panelTop.Width / scoreMax * (score - startScore), panelTop.Height);
            Snake[score] = new PictureBox();
            Snake[score].Location = Snake[score - 1].Location;
            Snake[score].Size = Snake[score - 1].Size;
            Snake[score].BackColor = Color.FromArgb(score, 200 + (score / 4), 0);
            panel.Controls.Add(Snake[score]);
            if(score - startScore == scoreMax)
            {
                GameOver();
            }
        }
        private void Controller(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    if (dirX != -1)
                    {
                        snakeIsMoving = true;
                        dirX = 1;
                        dirY = 0;
                    }
                    break;
                case "D":
                    if (dirX != -1)
                    {
                        snakeIsMoving = true;
                        dirX = 1;
                        dirY = 0;
                    }
                    break;
                case "Left":
                    if (dirX != 1)
                    {
                        snakeIsMoving = true;
                        dirX = -1;
                        dirY = 0;
                    }
                    break;
                case "A":
                    if (dirX != 1)
                    {
                        snakeIsMoving = true;
                        dirX = -1;
                        dirY = 0;
                    }
                    break;
                case "Up":
                    if (dirY != 1)
                    {
                        snakeIsMoving = true;
                        dirY = -1;
                        dirX = 0;
                    }
                    break;
                case "W":
                    if (dirY != 1)
                    {
                        snakeIsMoving = true;
                        dirY = -1;
                        dirX = 0;
                    }
                    break;
                case "Down":
                    if (dirY != -1)
                    {
                        snakeIsMoving = true;
                        dirY = 1;
                        dirX = 0;
                    }
                    break;
                case "S":
                    if (dirY != -1)
                    {
                        snakeIsMoving = true;
                        dirY = 1;
                        dirX = 0;
                    }
                    break;
            }
        }
    }
}