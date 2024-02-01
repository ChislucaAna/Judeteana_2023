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
    public partial class Sarpe : Form
    {
        public Sarpe()
        {
            InitializeComponent();
        }

        StreamReader reader;

        private void Sarpe_Load(object sender, EventArgs e)
        {
           reader=new StreamReader("Cuvinte.txt");
        }
    }
}
