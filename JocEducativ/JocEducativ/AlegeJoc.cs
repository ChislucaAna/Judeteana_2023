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
        public AlegeJoc(string email,string nume)
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JocEducativ.mdf;Integrated Security=True");
            label1.Text += "Bine ai venit,";
            label1.Text += nume;
            label1.Text += "!(";
            label1.Text += email;
            label1.Text += ")";
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;
        int mxprecedent = 1999999;
        string nume;
        string email;
        string punctaj;

        private void findmax(int tipjoc)
        {
            int mx = 0;
            cmd = new SqlCommand(String.Format("SELECT * FROM Rezultate WHERE TipJoc={0};", tipjoc.ToString()), con);
            con.Open();
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                punctaj = r[3].ToString();
                if (Convert.ToInt32(punctaj) >mx && Convert.ToInt32(punctaj)<mxprecedent)
                {
                    mx = Convert.ToInt32(punctaj);
                    email = r[2].ToString();
                }
            }
            con.Close();
            r.Close();
            find_name(email);
            mxprecedent = mx;
            if (tipjoc == 0)
                addtodatagrid1(email, nume, mx);
            else
                addtodatagrid2(email, nume, mx);
        }

        private void find_name(string email)
        {
            con.Open();
            cmd = new SqlCommand(String.Format("SELECT * FROM Utilizatori WHERE EmailUtilizator='{0}';", email), con);
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                nume = r[1].ToString();
            }
            con.Close();
            r.Close();
        }

        private void addtodatagrid1(string email,string nume,int mx)
        {
            dataGridView1.Rows.Add(email, nume, mx.ToString());
        }

        private void addtodatagrid2(string email, string nume, int mx)
        {
            dataGridView2.Rows.Add(email, nume, mx.ToString());
        }

        private void AlegeJoc_Load(object sender, EventArgs e)
        {
            int i; //tipjoc
            for (i = 0; i <= 1; i++)
            {
                mxprecedent = 1999999;
                for (int cnt = 1; cnt <= 3; cnt++)
                {
                    findmax(i);
                }
            }
            Ghiceste callable = new Ghiceste(email);
            callable.ShowDialog();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ghiceste callable = new Ghiceste(email);
            callable.ShowDialog();
            this.Hide();
        }
    }
}
