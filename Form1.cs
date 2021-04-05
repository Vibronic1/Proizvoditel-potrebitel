using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//Брестер.А.Н БББО-07-19
namespace Proizvoditel_potrebitel
{
    

    public partial class Form1 : Form
    {
        public Point position(Point Tochka,int pos)
        {
            Point vivod = new Point();
            if (Tochka.X > 850)
            {
                vivod.X = 850 + (((Tochka.X  - (850 )) / 4) * pos);
            }else if(Tochka.X < 850)
            {
                vivod.X = 700 - ((((850) - (Tochka.X )) / 4) * pos);
            }
            if (Tochka.Y < 450)
            {
                vivod.Y = 450 - ((((450 ) - (Tochka.Y)) / 4) * pos);
            }
            else if (Tochka.Y > 450)
            {
                vivod.Y = 450+  (((Tochka.Y  - (450 )) / 4) * pos);
            }
            return vivod ;
        }
        static bool[,] dostavka = new bool[,] { {false, false, false, false, false },{ false, false, false, false, false },{ false, false, false, false, false } };
        public static Image[] model = new Image[] { Image.FromFile("../../resources/Sclad.png"), Image.FromFile("../../resources/Potrebitel.png"), Image.FromFile("../../resources/Proizvoditel.png"), Image.FromFile("../../resources/Posilka.png") };
        static Bitmap Scene = new Bitmap(1800,1000);
        static Graphics g = Graphics.FromImage(Scene);
        static string[] labele = new string[] {"sleep", "sleep", "sleep", "sleep", "sleep"};
        private static void dobavlenie(int n,int Pos)
        {
            if (Pos == 0) { labele[0] = n.ToString(); }
            if (Pos == 1) { labele[1] = n.ToString(); }
            if (Pos == 2) { labele[2] = n.ToString(); }
           
            
            dostavka[2, Pos] = true;
            Thread.Sleep(2000);
            dostavka[2, Pos] = false;
            dostavka[1, Pos] = true;
            Thread.Sleep(2000);
            dostavka[1, Pos] = false;
            dostavka[0, Pos] = true;
            Thread.Sleep(2000);
            dostavka[0, Pos] = false;
            ochered += n;
        }
        private static void Umenshenie(int n,int Pos)
        {
            if (Pos == 3) { labele[3] = n.ToString(); }
            if (Pos == 4) { labele[4] = n.ToString(); }
            ochered -= n;
            dostavka[0, Pos] = true;
            Thread.Sleep(2000);
            dostavka[0, Pos] = false;
            dostavka[1, Pos] = true;
            Thread.Sleep(2000);
            dostavka[1, Pos] = false;
            dostavka[2, Pos] = true;
            Thread.Sleep(2000);
            dostavka[2, Pos] = false;
           
        }
        private static bool end = false;
        public static int ochered = 0;
        public static void proizvoditel(int N)
        {
            
            Random rand=new Random();
            bool sleep = false;
            while (end == false)
            {
                Thread.Sleep(100);
                if (ochered < 80)
                {
                    sleep = false;
                }
                else
                {
                    if (ochered > 100)
                    {
                        sleep = true;
                        labele[N] = "sleep";
                    }
                }
                if (!sleep)
                {
                   dobavlenie(rand.Next(1, 100),N); 
                }
            }
        }
        public static void potrebitel(int Pos)
        {
                Random rand = new Random();
                while ((end == false) || (ochered != 0))
                {
                    if (ochered > 0)
                    {
                    if (ochered > 100) { Umenshenie(rand.Next(1, 100), Pos); } else { if (ochered > 1) { Umenshenie(rand.Next(1, ochered), Pos); }else { Umenshenie(1, Pos); } }; 
                    }
                }
        }
        public static void otrisovka_posilki()
        {
            
        }

        public Form1()
        {

            InitializeComponent();
            g.DrawImage(model[2], 10, 75, 128, 128);
            g.DrawImage(model[2], 10, 350, 128, 128);
            g.DrawImage(model[2], 10, 650, 128, 128);

            g.DrawImage(model[0], new Point(600, 200));
            g.DrawImage(model[1], new Point(1275, 550));
            g.DrawImage(model[1], new Point(1275, 100));
            
                Thread Proizv1, Proizv2, Proizv3;
            Proizv1 = new Thread(() => proizvoditel(0));
            Proizv2 = new Thread(() => proizvoditel(1));
            Proizv3 = new Thread(() => proizvoditel(2));
            Proizv1.Start();
            Proizv2.Start();
            Proizv3.Start();
            Thread Potreb1, Potreb2;
            Potreb1=new Thread(() => potrebitel(3));
            Potreb2=new Thread(() => potrebitel(4));
            Potreb1.Start();
            Potreb2.Start();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = labele[0].ToString();
            label7.Text = labele[1].ToString();
            label6.Text = labele[2].ToString();
            label8.Text = labele[3].ToString();
            label9.Text = labele[4].ToString();
            Bitmap Scene1 =new Bitmap(Scene);
            Graphics g1 = Graphics.FromImage(Scene1);
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (dostavka[j, i])
                    {
                        switch (i)
                        {
                            case 0:
                                g1.DrawImage(model[3], position(new Point(10, 75),j));
                                break; 
                            case 1:
                                g1.DrawImage(model[3], position(new Point(10, 350),j));
                                break;
                            case 2:
                                g1.DrawImage(model[3], position(new Point(10, 650),j));
                                break;
                            case 3:
                                g1.DrawImage(model[3], position(new Point(1475, 100),j));
                                break;
                            case 4:
                                g1.DrawImage(model[3], position(new Point(1475, 550),j));
                                break;
                        }
                    }
                }
            }
            if (ochered < 0) { ochered = 0; }
            if (ochered > 200) { ochered = 200; }
            label4.Text = ochered.ToString();
            scene.Image = Scene1;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            end = true;
            labele[0] = "sleep";
            labele[1] = "sleep";
            labele[2] = "sleep";
        }
    }
}
