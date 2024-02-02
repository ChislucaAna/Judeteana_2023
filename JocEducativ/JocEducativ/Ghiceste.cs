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
    public partial class Ghiceste : Form
    {
        public Ghiceste(string adress)
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JocEducativ.mdf;Integrated Security=True");
            email = adress;
        }

        Random rnd;
        StreamReader reader;
        string cuvant;
        int stadiu = 6;
        int g = 0;
        int punctaj = 100;
        Rectangle rect;
        SqlConnection con;
        SqlCommand cmd;
        string email;
        int loaded = 0;

        public class butoane
        {
            public string val;
            public int x;
            public int y;
            public int shown;

            public butoane(string letter,int pozx, int pozy)
            {
                val = letter;
                x = pozx;
                y = pozy;
                shown = 1;
            }
        }

        butoane[] panel = new butoane[100];

        public class liniute
        {
            public char val;
            public int com;
            public Label lbl;
            public int x;
            public int y;

            public liniute(char letter, int completed,int pozx,int pozy)
            {
                val = letter;
                com = completed;
                x = pozx;
                y = pozy;

                lbl = new Label();
                lbl.Text = "_";
                lbl.Location = new Point(pozx, pozy);
                lbl.Width = 20;
                lbl.Height = 20;
            }
        }

        liniute[] spaces = new liniute[100];

        private void Ghiceste_Load(object sender, EventArgs e)
        {
            Thread.Sleep(200);
            generate_word();
            generate_liniute();
        }

        private void generate_word()
        {
            rnd = new Random();
            int index = rnd.Next(1,9);
            string line;
            int cnt = 0;
            reader = new StreamReader("Cuvinte.txt");
            while((line=reader.ReadLine())!=null)
            {
                cnt++;
                if(cnt==index)
                    cuvant = line;
            }
        }

        public void generate_liniute()
        {
            int i;
            int x = 20; int y = 20;
            for(i=0; i<cuvant.Length; i++)
            {
                liniute val = new liniute(cuvant[i], 0,x,y);
                spaces[i] = val;
                this.Controls.Add(spaces[i].lbl);
                x += 20;
            }
        }

        public void creste_floarea()
        {
            if (stadiu < 6)
            {
                stadiu++;
                string file = stadiu.ToString();
                file += ".png";
                pictureBox1.Image = Image.FromFile(file);
            }
            if(cuvant_complet()==1)
            {
                calculare_punctaj();
                adauga();
                Thread.Sleep(200);
                Application.Exit();
            }
        }

        public void adauga()
        {
            label1.Text += punctaj.ToString();
            con.Open();
            cmd = new SqlCommand(String.Format("INSERT INTO Rezultate VALUES({0},'{1}',{2});", "0",email,punctaj.ToString()),con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void calculare_punctaj()
        {
            punctaj = punctaj - 4 * g;
        }

        public int cuvant_complet()
        {
            int ok = 1;
            for(int i=0; i<cuvant.Length; i++)
            {
                if(String.Compare(spaces[i].lbl.Text,cuvant[i].ToString())!=0)
                {
                    ok = 0;
                }
            }
            return ok;
        }

        public void descreste_floarea()
        {
            g++;
            if(stadiu>1)
            {
                stadiu--;
                string file = stadiu.ToString();
                file += ".png";
                pictureBox1.Image = Image.FromFile(file);
            }
            if(stadiu==1)
            {
                punctaj = 0;
                label1.Text = "PUNCTAJ:0";
                adauga();
                MessageBox.Show("AI PIERDUT");
                Thread.Sleep(200);
                Application.Exit();
            }
        }

        public void verificare() //verifica daca s-a dat click si daca e potirvita litera
        {
            int ok = 0;
            for (int i = 0; i <= 25; i++)
            {
                Rectangle litera = new Rectangle(panel[i].x, panel[i].y, 20, 20);
                if (rect.IntersectsWith(litera))
                {
                    //dispare lit
                    panel[i].shown = 0;
                    this.Refresh();
                    for (int j = 0; j < cuvant.Length; j++)
                    {
                        if (String.Compare(panel[i].val, spaces[j].val.ToString()) == 0)
                        {
                            spaces[j].lbl.Text = spaces[j].val.ToString();
                            creste_floarea();
                            ok = 1;
                        }
                    }
                    if (ok == 0)
                    {
                        descreste_floarea();
                    }
                }
            }
        }

        private void Ghiceste_MouseClick(object sender, MouseEventArgs e)
        {
            rect = new Rectangle(e.X, e.Y, 20, 20);
            verificare();
        }

        private void Ghiceste_Paint(object sender, PaintEventArgs e) //pictam tastele
        {
            if (loaded == 0)
            {
                int pozx = 10, pozy = this.Height - 100;
                int i = 0;
                for (int cnt = 97; cnt <= 122; cnt++)
                {
                    butoane aux = new butoane(Convert.ToChar(cnt).ToString(), pozx, pozy); ;
                    panel[i] = aux;
                    SolidBrush black = new SolidBrush(Color.Black);
                    var fontFamily = new FontFamily("Times New Roman");
                    var font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Pixel);
                    e.Graphics.DrawString(Convert.ToChar(cnt).ToString(), font, black, pozx, pozy);

                    pozx += 30;
                    i++;
                }
                loaded = 1;
            }
            else
            {
                int pozx = 10, pozy = this.Height - 100;
                int i = 0;
                for (int cnt = 97; cnt <= 122; cnt++)
                {

                    if (panel[i].shown == 1)
                    {
                        SolidBrush black = new SolidBrush(Color.Black);
                        var fontFamily = new FontFamily("Times New Roman");
                        var font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Pixel);
                        e.Graphics.DrawString(Convert.ToChar(cnt).ToString(), font, black, pozx, pozy);
                    }

                    pozx += 30;
                    i++;
                }
            }
        }
    }
}
