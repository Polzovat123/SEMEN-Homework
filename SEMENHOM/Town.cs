using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SEMENHOM
{
    interface Town{
    }
    class City {
        public int x;
        public int y;
        private int D = 40;
        private Color pen = Color.Black;
        private bool canGo = true;
        public TextBox num;
        public int code;
        public City(int _x, int _y, int _code){
            x = _x;
            y = _y;
            num = new TextBox();
            num.Text = _code.ToString();
            num.Width = 20;
            code = _code;
            num.Location = new Point(_x, _y);
        }
        public void draw(PictureBox pb, Control.ControlCollection C){
            Graphics dr = pb.CreateGraphics();
            dr.DrawEllipse(new Pen(pen, 8), x, y, D, D);
            dr.FillEllipse(new SolidBrush(Color.White), x, y, D, D);
            dr.DrawString(num.Text, num.Font, Brushes.Black, new PointF(x+15, y+15));
            dr.Dispose();
            C.Add(num);
        }
        public void markedTown(){
            canGo = !canGo;
            Console.WriteLine("Marked");
            if (!canGo) pen = Color.Red;
            else pen = Color.Black;
        }
        public bool isfriendlyTown(){
            return canGo;
        }
        public bool inTown(int _x, int _y){
            int dist = (x - _x+D/2) * (x - _x+D/2) + (y - _y+D/2) * (y - _y+D/2);
            Console.WriteLine(dist);
            Console.WriteLine(D * D / 4);
            if (dist <= D * D / 4) return true;
            return false;
        }
    }
    class Way{
        int x1, x2;
        int y1, y2;
        Pen pen = new Pen(Color.Black, 8);
        public Way(int _x1, int _x2, int _y1, int _y2){
            x1 = _x1;
            x2 = _x2;
            y1 = _y1;
            y2 = _y2;
        }
        public void draw(Graphics k){
            DrawArrow(k,
                        new Pen(Color.Black, 5),
                        new Point(x1, y1),
                        new Point(x2, y2),
                        25);
        }

        private void DrawArrowhead(Graphics gr, Pen pen, PointF p, float nx, double ny, double length)
        {
            double ax = length * (-ny - nx);
            double ay = length * (nx - ny);
            PointF[] points =
            {
                new PointF((float)(p.X + ax), (float)(p.Y + ay)),
                p,
                new PointF((float)(p.X - ay), (float)(p.Y + ax))
            };
            gr.DrawLines(pen, points);
        }
        private void DrawArrow(Graphics gr, Pen pen, PointF p1, PointF p2, double length)
        {
            gr.DrawLine(pen, p1, p2);
            double vx = p2.X - p1.X;
            double vy = p2.Y - p1.Y;
            double dist = (float)Math.Sqrt(vx * vx + vy * vy);
            vx /= dist;
            vy /= dist;
            DrawArrowhead(gr, new Pen(Color.Black, 5), p2, (float)vx, vy, length);

        }
    }

    
}
