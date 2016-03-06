using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TableInfo
{
    public partial class Formfa : Form
    {
        public Formfa()
        {
            InitializeComponent();
        }

        private void label1text_Changed(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            if (label1.Text == "this.Close()")
                this.Close();
            this.Refresh();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Formfa_Load(object sender, EventArgs e)
        {

        }
    }
}