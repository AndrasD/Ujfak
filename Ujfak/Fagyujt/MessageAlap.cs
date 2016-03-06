using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TableInfo
{
    public partial class MessageAlap : Form
    {
        private string[] dr = new string[] { "OK", "Mégsem", "Igen", "Nem", "None", "Cancel" };
        private Fak.DialogResult valasz = Fak.DialogResult.Cancel;
        private int tav = 10;
        private Message Hivo;
        private Button button1=null;
        private Button button2=null;
        private Button button3=null;
        public Fak.DialogResult Valasz
        {
            get { return valasz; }
            set { valasz = value; }
        }
        public MessageAlap(Message hivo,string text, string nametext, Button[] buttonok)
        {
            InitializeComponent();
            Hivo = hivo;
            this.Text = nametext;
            if(nametext.Length!=0)
                  this.Width = nametext.Length * 7 + 100;
            label1.Text = text;
            if (buttonok != null)
            {
                int x;
                int y;
                this.Controls.AddRange(buttonok);
                switch (buttonok.Length)
                {
                    case 1:
                        button1 = buttonok[0];
                        button1.Click += new EventHandler(button_Click);
                        button1.Location = new Point((this.Size.Width - button1.Size.Width) / 2, (this.Size.Height - label1.Size.Height - label1.Location.Y) / 2 + label1.Location.Y + label1.Size.Height);
                        break;
                    case 2:
                        button1 = buttonok[0];
                        button1.Click += new EventHandler(button_Click);
                        button2 = buttonok[1];
                        button2.Click += new EventHandler(button_Click);
                        x = (this.Size.Width - button2.Size.Width - button1.Size.Width - tav) / 2;
                        y = (this.Size.Height - label1.Size.Height - label1.Location.Y) / 2 + label1.Location.Y + label1.Size.Height;
                        button1.Location = new Point(x, y);
                        button2.Location = new Point(button1.Location.X + button1.Size.Width + tav, y);
                        break;
                    case 3:
                        button1 = buttonok[0];
                        button1.Click += new EventHandler(button_Click);
                        button2 = buttonok[1];
                        button2.Click += new EventHandler(button_Click);
                        button3 = buttonok[2];
                        button3.Click += new EventHandler(button_Click);
                        y = (this.Size.Height - label1.Size.Height - label1.Location.Y) / 2 + label1.Location.Y + label1.Size.Height;
                        x = (this.Size.Width - button2.Size.Width - button3.Size.Width - button1.Size.Width - 2 * tav) / 2;
                        button1.Location = new Point(x, y);
                        button2.Location = new Point(button1.Location.X + button1.Size.Width + tav, y);
                        button3.Location = new Point(button2.Location.X + button2.Size.Width + tav, y);
                        break;

                }
            }

        }
        public void button_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            for (int i = 0; i < dr.Length; i++)
            {
                if (dr[i] == but.Text)
                {
                    Hivo.Valasz = (Fak.DialogResult)i;
                    this.Close();
                }
            }
            this.Close();
        }
        private void MessageAlap_Closing(object sender, FormClosingEventArgs e)
        {
            if (Hivo.Valasz == Fak.DialogResult.None)
                Hivo.Valasz = Fak.DialogResult.Cancel;

        }

    }
    public class Message
    {
        //        public enum MessageBoxButtons { OK, OKMégsem, IgenNem, IgenNemMégsem };
        private Fak.DialogResult valasz = Fak.DialogResult.None;
        private Button ButtonOk = new Button();
        private Button ButtonMegsem = new Button();
        private Button ButtonIgen = new Button();
        private Button ButtonNem = new Button();
        private MessageAlap Showbox = null;
        public Fak.DialogResult Valasz
        {
            get { return valasz; }
            set { valasz = value; }
        }
        public Message()
        {
            ButtonOk.Text = "OK";
            ButtonOk.Size = new System.Drawing.Size(56, 23);
            ButtonIgen.Text = "Igen";
            ButtonIgen.Size = new System.Drawing.Size(56, 23);
            ButtonNem.Text = "Nem";
            ButtonNem.Size = new System.Drawing.Size(56, 23);
            ButtonMegsem.Text = "Mégsem";
            ButtonMegsem.Size = new System.Drawing.Size(56, 23);
        }
        public Fak.DialogResult Show(string text)
        {
            return Show(text, "", Fak.MessageBoxButtons.None);
        }
        public Fak.DialogResult Show(string text, string nametext)
        {
            return Show(text, nametext, Fak.MessageBoxButtons.None);
        }
        public Fak.DialogResult Show(string text, string nametext, Fak.MessageBoxButtons buttonsenum)
        {
            valasz = Fak.DialogResult.None;
            switch ((int)(MessageBoxButtons)buttonsenum)
            {
                case 0:
                    Showbox = new MessageAlap(this, text, nametext, new Button[] { ButtonOk });
                    break;
                case 1:
                    Showbox = new MessageAlap(this, text, nametext, new Button[] { ButtonOk, ButtonMegsem });
                    break;
                case 2:
                    Showbox = new MessageAlap(this, text, nametext, new Button[] { ButtonIgen, ButtonNem });
                    break;
                case 3:
                    Showbox = new MessageAlap(this, text, nametext, new Button[] { ButtonIgen, ButtonNem, ButtonMegsem });
                    break;
                case 4:
                    Showbox = new MessageAlap(this, text, nametext, null);
                    break;
            }
            Showbox.ShowDialog();
            return valasz;
        }
    }

}