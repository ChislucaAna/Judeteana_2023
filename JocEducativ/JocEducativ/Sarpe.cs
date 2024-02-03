using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.SqlClient;

namespace JocEducativ
{
    public partial class Sarpe : Form
    {
        public Sarpe(string adress)
        {
            InitializeComponent();
            email = adress;
            this.KeyPreview = true;
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JocEducativ.mdf;Integrated Security=True");
        }

        string email;
        SqlConnection con;
        SqlCommand cmd;
        Random rnd;
        int ok1 = 0, ok2 = 0; //loading head and food
        int length; //lungimea sarpelui
        int punctaj = 0;

        public class componenta //bucatiile din care e facut snake-ul
        {
            public int x;
            public int y;
            public string dir;

            public componenta(int pozx, int pozy, string directie)
            {
                x = pozx;
                y = pozy;
                dir = directie;
            }
        }

        componenta[] snake = new componenta[100];

        public class ball
        {
            public int x;
            public int y;

            public ball(int pozx, int pozy)
            {
                x = pozx;
                y = pozy;
            }
        }

        ball food;

        public void generate_head()
        {
            rnd = new Random();
            int x = rnd.Next(100, this.Width - 100);
            int y = rnd.Next(100, this.Height / 2);
            componenta aux = new componenta(x, y, "in jos");
            snake[0] = aux;
            length = 1;
            ok1 = 1;
        }

        public void generate_food()
        {
            rnd = new Random();
            int x = rnd.Next(50, this.Width - 100);
            int y = rnd.Next(this.Height / 2, this.Height - 200);
            ball aux = new ball(x, y);
            food = aux;
            ok2 = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            generate_head();
            generate_food();
            Thread.Sleep(200);
            timer1.Start();
        }

        public void modifica_coordonatele()
        {
            int i = 0;
            for (i = 0; i < length; i++)
            {
                if (snake[i].dir == "in sus")
                {
                    snake[i].y -= 20;
                }
                if (snake[i].dir == "in jos")
                {
                    snake[i].y += 20;
                }
                if (snake[i].dir == "spre dreapta")
                {
                    snake[i].x += 20;
                }
                if (snake[i].dir == "spre stanga")
                {
                    snake[i].x -= 20;
                }
            }
        }

        public void adauga_componenta()
        {
            int x = snake[length - 1].x;
            int y = snake[length - 1].y;
            if (snake[length - 1].dir == "in sus")
            {
                componenta aux = new componenta(x, y+20, "in sus");
                snake[length] = aux;
            }
            if (snake[length - 1].dir == "in jos")
            {
                componenta aux = new componenta(x, y-20, "in jos");
                snake[length] = aux;
            }
            if (snake[length - 1].dir == "spre dreapta")
            {
                componenta aux = new componenta(x-20, y, "spre dreapta");
                snake[length] = aux;
            }
            if (snake[length - 1].dir == "spre stanga")
            {
                componenta aux = new componenta(x+20, y, "spre stanga");
                snake[length]=aux;
            }
            length++;
        }


        public void verifica_intersectia()
        {
            Rectangle rect1 = new Rectangle(snake[0].x, snake[0].y, 20, 20);
            Rectangle rect2 = new Rectangle(food.x, food.y, 20, 20);
            if (rect1.IntersectsWith(rect2))
            {
                generate_food();
                adauga_componenta();

                timer1.Stop();
                Intrebare callable = new Intrebare();
                callable.ShowDialog();
                while(callable.Visible)
                {
                    Thread.Sleep(200);
                }
                timer1.Start();

                punctaj = punctaj + Intrebare.puncte  + 10;
                label1.Text = "Punctaj:";
                label1.Text += punctaj.ToString();
            }
        }

        public void verifica_pozitia()
        {
            if(snake[0].x<10 || snake[0].y<10 || snake[0].x>this.Width-10 || snake[0].y>this.Height-30)
            {
                Application.Exit();
            }
        }

        public void verifica_coada()
        {
            Rectangle rect = new Rectangle(snake[0].x, snake[0].y, 20, 20);
            for (int i=1; i<length;i++)
            {
                Rectangle rect1 = new Rectangle(snake[i].x, snake[i].y, 20, 20);
                if(rect.IntersectsWith(rect1))
                {
                    Application.Exit();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            verifica_intersectia();
            verifica_pozitia(); //verifica daca a iesit din form
            verifica_coada(); //verifica daca capul sarpelui se intersecteaza cu coada sa
            modifica_coordonatele();
            pictureBox1.Refresh();
            preia_directia();
        }

        public void preia_directia()
        {
            int i;
            for (i = length-1; i >= 1; i--)
            {
                snake[i].dir = snake[i - 1].dir;
            }
        }

        private void Sarpe_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyValue == 'W')
            {
                if (snake[0].dir == "spre stanga" || snake[0].dir == "spre dreapta")
                {
                    snake[0].dir = "in sus";
                }
            }
            if (e.KeyValue == 'A')
            {
                if (snake[0].dir == "in sus" || snake[0].dir == "in jos")
                {
                    snake[0].dir = "spre stanga";
                }
            }
            if (e.KeyValue == 'S')
            {
                if (snake[0].dir == "spre stanga" || snake[0].dir == "spre dreapta")
                {
                    snake[0].dir = "in jos";
                }
            }
            if (e.KeyValue == 'D')
            {
                if (snake[0].dir == "in sus" || snake[0].dir == "in jos")
                {
                    snake[0].dir = "spre dreapta";
                }
            }
        }

        public void adauga_inregistrare()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("INSERT INTO Rezultate VALUES({0},'{1}',{2});", "1", email, punctaj.ToString()), con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            adauga_inregistrare();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (ok1 == 1 && ok2 == 1)
            {
                int i = 0;
                for (i = 0; i < length; i++)
                {
                    SolidBrush white = new SolidBrush(Color.White);
                    SolidBrush green = new SolidBrush(Color.Green);
                    Rectangle rect = new Rectangle(snake[i].x, snake[i].y, 20, 20);
                    if (i == 0)
                        e.Graphics.FillEllipse(white, rect);
                    else
                        e.Graphics.FillEllipse(green, rect);
                }

                SolidBrush red = new SolidBrush(Color.Red);
                Rectangle rectangle = new Rectangle(food.x, food.y, 20, 20);
                e.Graphics.FillEllipse(red, rectangle);
            }
        }
    }
}
