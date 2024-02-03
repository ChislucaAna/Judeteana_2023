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
    public partial class Intrebare : Form
    {
        public Intrebare()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JocEducativ.mdf;Integrated Security=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        Random rnd;
        SqlDataReader r;
        string enunt, corect;
        static public int puncte;
        string[] raspuns = new string[4];
        int answer;

        private void button1_Click(object sender, EventArgs e)
        {
            check_answer();
        }

        public void check_answer()
        {
            if(answer==Convert.ToInt32(corect))
            {
                MessageBox.Show(" Felicitări, ai răspuns corect!");
            }
            else
            {
                MessageBox.Show(String.Format("Răspunsul tău este greșit! Răspunsul corect este {0}", raspuns[Convert.ToInt32(corect)]) );
                puncte = 0;
            }
            this.Hide();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            answer = 3;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            answer = 2;
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            answer = 1;
        }

        public void generate_question()
        {
            con.Open();
            rnd = new Random();
            int index = rnd.Next(2, 19);;
            cmd = new SqlCommand("SELECT * FROM Itemi;", con);
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (index==0)
                {
                    enunt = r[1].ToString();
                    raspuns[1] = r[2].ToString();
                    raspuns[2] = r[3].ToString();
                    raspuns[3] = r[4].ToString();
                    corect = r[5].ToString();
                    puncte = Convert.ToInt32(r[6]);
                }
                index--;
            }
            r.Close();
            label1.Text = enunt;
            radioButton1.Text = raspuns[1];
            radioButton2.Text = raspuns[2];
            radioButton3.Text = raspuns[3];
            con.Close();
        }

        private void Intrebare_Load(object sender, EventArgs e)
        {
            generate_question();
        }
    }
}
