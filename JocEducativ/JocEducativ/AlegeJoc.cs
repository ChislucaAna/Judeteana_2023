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
using System.Data.SqlClient;

namespace JocEducativ
{
    public partial class AlegeJoc : Form
    {
        public AlegeJoc(string email)
        {
            InitializeComponent();
            label1.Text += ":";
            label1.Text += email;
            label1.Text += "!";
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JocEducativ.mdf;Integrated Security=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;
        int mx1 = 0; int mx2 = 0; int mx3 = 0;
        string email1, email2, email3;

        public void findmax(int tipjoc)
        {

        }

        public void find_name_by_email(string email)
        {

        }

        private void AlegeJoc_Load(object sender, EventArgs e)
        {
            //cele mai mari 3 rezultate din Ghiceste
            con.Open();
            cmd = new SqlCommand("SELECT * FROM Rezultate WHERE TipJoc=0;",con);
            r = cmd.ExecuteReader();
           while(r.Read())
           {
                if(Convert.ToInt32(r[3])> mx1)
                {
                    mx3 = mx2;
                    email3 = email2;

                    mx2 = mx1;
                    email2 = email1;

                    mx1 = Convert.ToInt32(r[3]);
                    email1 = r[2].ToString();

                }
                else
                {
                    if(Convert.ToInt32(r[3])>mx2)
                    {
                        mx3 = mx2;
                        email3 = email2;

                        mx2 = Convert.ToInt32(r[3]);
                        email2 = r[2].ToString();
                    }
                    else
                    {
                        if(Convert.ToInt32(r[3])>mx3)
                        {
                            mx3 = Convert.ToInt32(r[3]);
                            email3 = r[2].ToString();
                        }
                    }
                }
           }
            r.Close();
            con.Close();
            //adauga rezultatele despre jocul Ghiceste
            //cauta in baza de date nuumele utilizatorului cu emailurile alea
            cmd = new SqlCommand(String.Format("SELECT FROM Utilizatori WHERE NumeUtilizator='{0}';", email1), con);

            //reseteaza variabilele
            mx1 = 0;
            mx2 = 0;
            mx3 = 0;
            //Sarpe
            con.Open();
            cmd = new SqlCommand("SELECT * FROM Rezultate WHERE TipJoc=1;",con);
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (Convert.ToInt32(r[3]) > mx1)
                {
                    mx3 = mx2;
                    email3 = email2;

                    mx2 = mx1;
                    email2 = email1;

                    mx1 = Convert.ToInt32(r[3]);
                    email1 = r[2].ToString();

                }
                else
                {
                    if (Convert.ToInt32(r[3]) > mx2)
                    {
                        mx3 = mx2;
                        email3 = email2;

                        mx2 = Convert.ToInt32(r[3]);
                        email2 = r[2].ToString();
                    }
                    else
                    {
                        if (Convert.ToInt32(r[3]) > mx3)
                        {
                            mx3 = Convert.ToInt32(r[3]);
                            email3 = r[2].ToString();
                        }
                    }
                }
            }
            con.Close();
            r.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
