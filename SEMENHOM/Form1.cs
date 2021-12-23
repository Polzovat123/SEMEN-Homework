using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEMENHOM
{
    public partial class Form1 : Form
    {
        private Graphics k;
        private City[] cities;
        private Way[] ways;
        int[] haveStay = new int[1000];
        int[] Price = new int[1000];
        private int LW = 0;
        int[,] matrix;
        private int S = 0, D = 40;
        private Button[] table = new Button[10000];
        private int s = 0;
        int code = -1;
        bool haveOne = false;
        int _x, _y;
        int c = -1;

        public Form1(){
            cities = new City[101];
            ways = new Way[10000];
            matrix = new int[102, 102];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    matrix[i, j] = -1;
                }
            }
            InitializeComponent();
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e){
            MouseEventArgs a = (MouseEventArgs)e;
            //a.X;a.Y;
            for (int i = 0; i < S; i++)
            {
                if (cities[i].inTown(a.X, a.Y)){
                    return;
                }
            }
            cities[S] = new City(a.X - D / 2, a.Y - D / 2, S + 1);
            S++;
            drawMonitor();
            drawMatrix();
        }
        void drawMonitor(){
            Graphics p = pictureBox1.CreateGraphics();
            p.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            for (int i=0; i<LW; i++){
                ways[i].draw(k);
            }
            for (int i = 0; i < S; i++){
                cities[i].draw(pictureBox1, Controls);
            }           
        }

        void drawMatrix()
        {
            for (int i = 0; i < s; i++) Controls.Remove(table[i]);
            for (int i = 0; i < s; i++) table[i] = null;
            s = (S + 2) * (S + 2);
            for (int i = 0; i < s; i++)
            {
                table[i] = new Button();
            }
            int index = 0;
            int sw = pictureBox1.Width + 20, sh = 20;
            int nw = sw, nh = 20;
            for (int i = 0; i < S + 1; i++)
            {
                if (i == 0)
                {
                    DoBut(table[0], "N", nw, nh);
                    index++; nw += 45;
                    for (int j = 1; j < S + 1; j++)
                    {
                        DoBut(table[index], j.ToString(), nw, nh);
                        index++; nw += 45;
                    }
                }
                if (i != 0)
                    for (int j = 0; j < S + 1; j++)
                    {
                        if (j == 0)
                        {
                            DoBut(table[index], i.ToString(), nw, nh);
                            index++; nw += 45;
                        }
                        else
                        {
                            DoBut(table[index], (matrix[i - 1, j - 1]).ToString(), nw, nh);
                            index++; nw += 45;
                        }
                    }
                nh += 30;
                nw = sw;
            }
        }
        private void DoBut(Button btn, string text, int x, int y){
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Width = 45;
            btn.Height = 30;
            btn.Enabled = true;
            Controls.Add(btn);
        }

        private void pictureBox1_Click(object sender, EventArgs e){
            if (code == 2){
                MouseEventArgs a = (MouseEventArgs)e;
                for (int i=0; i<S; i++){
                    if (cities[i].inTown(a.X, a.Y)){
                        cities[i].markedTown();
                    }
                }
            }
            if (code == 1){
                doWay(e);
            }
            drawMatrix();
            drawMonitor();
        }

        City buff=null, first=null;
        private void doWay(EventArgs e)
        {
            MouseEventArgs a = (MouseEventArgs)e;
            if (!checkIsHave(a.X, a.Y))
            {
                if (haveOne && buff!=first){
                    matrix[buff.code - 1, first.code - 1] = 1;
                    matrix[first.code - 1, buff.code - 1] = 1;
                    haveOne = false;
                    ways[LW] = new Way(first.x+D/2, buff.x + D / 2, first.y + D / 2, buff.y + D / 2);
                    LW++;
                }
                else{
                    haveOne = true;
                    first = buff;
                }
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            code = 2;//choose cities
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int start = 0, finish = 1;
            try
            {
                start = Int32.Parse(textBox1.Text);
                finish = Int32.Parse(textBox2.Text);
                //Parse dontGo vertex
            }catch (Exception er){
                tbHelper.Text = "We can`t read you number";
                return;
            }
            tbHelper.Text = search_way(start - 1, finish - 1);
        }

        int next_vertex()
        {
            int m = 9999999, v = 0;
            for (int i = 0; i < S; i++)
            {
                if (m > Price[i] && haveStay[i] == 0)
                {
                    m = Price[i];
                    v = i;
                }
            }
            return v;
        }

        string search_way(int start, int finish)
        {
            for (int i = 0; i < S; i++) haveStay[i] = 0;
            for (int i = 0; i < S; i++) Price[i] = 99999999;
            Price[start] = 0;
            int now = start;
            int last = -1;
            while (haveStay[finish] != 1)
            {
                for (int i = 0; i < S; i++)
                {
                    Console.WriteLine(matrix[now, i] != c && cities[i].isfriendlyTown());
                    if (matrix[now, i] != c && cities[i].isfriendlyTown()){
                        Price[i] = Math.Min(Price[i], Price[now] + matrix[now, i]);
                    }
                }
                haveStay[now] = 1;
                last = now;
                now = next_vertex();
                if (last == now) break;
            }
            if (haveStay[finish]==0){
                return "We havent way to this city.";
            }
            string reverse_V = "";
            now = finish;
            int nn = 0;
            while (haveStay[start] != 0 && nn!=-1)
            {
                int Next = 999999;
                nn = -1;
                for (int i = 0; i < S; i++)
                {
                    if (matrix[now, i] != code && Next > Price[i])
                    {
                        Next = Price[i];
                        nn = i;
                    }
                }
                if (nn == -1) break;
                reverse_V = reverse_V + (now+1).ToString() + "<-";
                haveStay[now] = 0;
                Console.WriteLine(now);
                now = nn;
            }
            reverse_V = reverse_V.Substring(0, reverse_V.Length-2);// + (start + 1).ToString();
            return reverse_V;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            k = pictureBox1.CreateGraphics();
        }

        private void radioButton2_Click(object sender, EventArgs e){
            code = 1;//create way
        }

        private bool checkIsHave(int x, int y){
            float sq;
            for (int i = 0; i < S; i++){
                if (cities[i].inTown(x, y)){
                    buff = cities[i];
                    return false;
                }
            }
            return true;
        }
    }
}
