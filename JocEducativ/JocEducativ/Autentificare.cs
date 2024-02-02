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
    public partial class Autentificare : Form
    {
        public Autentificare()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JocEducativ.mdf;Integrated Security=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;
        string line;
        string email, parola;
        string nume;
        int ok = 0;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            email = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            parola = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Close();
                con.Open();
                cmd = new SqlCommand(String.Format("SELECT * FROM Utilizatori WHERE EmailUtilizator='{0}' AND Parola='{1}';",email,parola),con);
                r = cmd.ExecuteReader();
                while(r.Read())
                {
                    ok = 1;
                    nume = r[1].ToString();
                }
                con.Close();
                r.Close();
                if (ok == 1)
                {
                    AlegeJoc callable = new AlegeJoc(email,nume);
                    callable.ShowDialog();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Date de autentificare invalide!");
                    textBox1.Text = null;
                    textBox2.Text = null;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Autentificare_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                reader = new StreamReader("Utilizatori.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] bucati = line.Split(';');
                    cmd = new SqlCommand(String.Format("INSERT INTO Utilizatori VALUES('{0}','{1}','{2}');", bucati[0], bucati[1], bucati[2]), con);
                    cmd.ExecuteNonQuery();
                }
                reader.Close();
                reader = new StreamReader("Rezultate.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] bucati = line.Split(';');
                    cmd = new SqlCommand(String.Format("INSERT INTO Rezultate VALUES({0},'{1}',{2});", bucati[1], bucati[2], bucati[3]), con);
                    cmd.ExecuteNonQuery();
                }
                reader.Close();
                reader = new StreamReader("Itemi.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] bucati = line.Split(';');
                    cmd = new SqlCommand(String.Format("INSERT INTO Itemi VALUES('{0}','{1}','{2}','{3}','{4}',{5});", bucati[1], bucati[2], bucati[3], bucati[4], bucati[5], bucati[6]), con);
                    cmd.ExecuteNonQuery();
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            AlegeJoc callable = new AlegeJoc("ana", "ana");
            callable.ShowDialog();
        }
    }
}
