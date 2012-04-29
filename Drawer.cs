using System;
using System.Drawing;

namespace NumbersSearcher
{
    //вспомогательный класс для отрисовки изображения
    public class Drawer
    {
        private Graphics g;
        public Graphics G
        {
            get { return g; }
        }
        private Bitmap btmp;
        public Bitmap Btmp
        {
            get { return btmp; }
        }

        public Drawer(int width, int height)
        {
            btmp = new Bitmap(width, height);
            g = Graphics.FromImage(btmp);
        }
        public void Clear()
        {
            g.Clear(Color.White);
        }
        public int PutEllipse(int x, int y, int h)
        {
            try
            {
                
                g.FillEllipse(new SolidBrush(Color.Black), x, y, h, h);
                return 0;
            }
            catch
            {
                return 1;
            }
        }
        public void PutPixel(int x, int y, Color c)
        {
            g.DrawRectangle(new Pen(c), x, y, 1, 1);
        }
    }

}