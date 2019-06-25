using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;
using MATLABplane;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace WindowsFormsApp7
{
    [Serializable]
    public partial class Form1 : Form
    {
        dataclass data = new dataclass();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region -- Если .XML найден 
            if (File.Exists(@"Data.xml") == true)
            {
                // Загрузка данных из .XML
                using (FileStream stream = new FileStream(@"Data.xml", FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(dataclass));
                    data = (dataclass)serializer.Deserialize(stream);

                    textBox1.Text = data.Kob_str1;
                    textBox2.Text = data.Tob_str1;
                    textBox3.Text = data.t_zap_str1;
                    textBox4.Text = data.y_max_str1;
                    textBox5.Text = data.dYst_zad_str1;
                    textBox6.Text = data.tp_zad_str1;
                    textBox7.Text = data.p_str1;
                    textBox8.Text = data.dXvx_str1;

                    if (!(File.Exists(@".\bode1.png"))) { }
                    else
                    {
                        pictureBox2.Image = Image.FromFile(@".\bode1.png");
                        pictureBox3.Image = Image.FromFile(@".\step1.png");
                        pictureBox4.Image = Image.FromFile(@".\nyquist1.png");
                        pictureBox5.Image = Image.FromFile(@".\impulse1.png");
                    }
                }
            }
            #endregion

            #region -- Если .XML не найден
            else
            {
                textBox1.Text = "13";
                textBox2.Text = "30";
                textBox3.Text = "20";
                textBox4.Text = "50";
                textBox5.Text = "15";
                textBox6.Text = "200";
                textBox7.Text = "5";
                textBox8.Text = "2";
            }
            #endregion
        }

        public double Kob, Tob, t_zap, y_max, dYst_zad, tp_zad, dXvx, p; //объявление переменых
        public string Kob_str, Tob_str, t_zap_str, y_max_str, dYst_zad_str, tp_zad_str, dXvx_str, p_str;

        MWArray[] y = null; //выходной массив метода plane
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile(@".\mo.png");
            pictureBox3.Image = Image.FromFile(@".\mo.png");
            pictureBox4.Image = Image.FromFile(@".\mo.png");
            pictureBox5.Image = Image.FromFile(@".\mo.png");

            Kob_str = textBox1.Text; //считывание с TextBox
            Tob_str = textBox2.Text;
            t_zap_str = textBox3.Text;
            y_max_str = textBox4.Text;
            dYst_zad_str = textBox5.Text;
            tp_zad_str = textBox6.Text;
            p_str = textBox7.Text;
            dXvx_str = textBox8.Text;

            data.Kob_str1 = Kob_str;
            data.Tob_str1 = Tob_str;
            data.t_zap_str1 = t_zap_str;
            data.y_max_str1 = y_max_str;
            data.dYst_zad_str1 = dYst_zad_str;
            data.tp_zad_str1 = tp_zad_str;
            data.p_str1 = p_str;
            data.dXvx_str1 = dXvx_str;

            Kob = Convert.ToDouble(Kob_str); //преобразоване string в double
            Tob = Convert.ToDouble(Tob_str);
            t_zap = Convert.ToDouble(t_zap_str);
            y_max = Convert.ToDouble(y_max_str);
            dYst_zad = Convert.ToDouble(dYst_zad_str);
            tp_zad = Convert.ToDouble(tp_zad_str);
            p = Convert.ToDouble(p_str);
            dXvx = Convert.ToDouble(dXvx_str);

            Class1 obj_plane = new Class1(); //экземпляр класса компонента               
            y = obj_plane.Teylorr(0, Kob, Tob, t_zap, y_max, dYst_zad, tp_zad, dXvx);//обращение к методу Teylor, первый параметр - это кол-во возвращаемых аргументов

            button1.Visible = false;
            button3.Visible = true;

            pictureBox2.Image = Image.FromFile(@".\bode.png");
            pictureBox3.Image = Image.FromFile(@".\step.png");
            pictureBox4.Image = Image.FromFile(@".\nyquist.png");
            pictureBox5.Image = Image.FromFile(@".\impulse.png");

            using (FileStream stream = new FileStream(@"Data.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(dataclass));
                serializer.Serialize(stream, data);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button3.Visible = false;

            File.Copy(@".\bode.png", @".\bode1.png", true);
            File.Copy(@".\step.png", @".\step1.png", true);
            File.Copy(@".\nyquist.png", @".\nyquist1.png", true);
            File.Copy(@".\impulse.png", @".\impulse1.png", true);

            Process.Start(@".\test.rtf");
            Application.Restart();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                if (Char.IsNumber(e.KeyChar) ||
                (!string.IsNullOrEmpty(textBox1.Text) && e.KeyChar == ','))
                {
                    return;
                }

                e.Handled = true;
            }
        }
    }
}
