using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TypingPractice
{
    public partial class InfoBox : Form
    {
        public InfoBox(string message, string title)
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            this.label1.Text = message;
            this.Text = title;
            this.Width = this.label1.Width + 165;
            this.button1.Location = new Point((this.Width / 2) - (this.button1.Width / 2), this.button1.Location.Y);
            this.label1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
