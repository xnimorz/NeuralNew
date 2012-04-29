using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace NumbersSearcher
{
    public partial class Form1 : Form
    {
        string Path;
  
        
        //int res = 0;
        List<byte> points;
        
        
        
        Holder objects;// = Holder.GetInstance;
        void ReadFile()
        {
            //чтение файла chars.txt, который содержит список существующих символов
            if (File.Exists("chars.txt"))
            {

                StreamReader sr = null;
                try
                {//пытаемся считать
                    sr = new StreamReader("chars.txt");
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        char smb = line[0];
                       //listBox1.Items.Add(smb);
                        objects.AddNeuron(smb);
                    }
                    
                    
                }
                catch
                {
        
        
                }
                finally
                {//закрываем в любом случае
                    sr.Close(); 
                }
            }
            else
            {
//если файла нет, создадим
                StreamWriter write = new StreamWriter("chars.txt");
                write.Close();
            }

        }
        void WriteFile(char a)
        {//добавление нового символа
           
            StreamWriter sw;
            sw = new StreamWriter("chars.txt",true);

            sw.WriteLine(a.ToString());
            sw.Flush();
            sw.Close();
            
        }
        public Form1()
        {
            InitializeComponent();//инициализация
            objects = Holder.GetInstance(Preview.Width, Preview.Height);
            
            //this.Clear();

            
            ReadFile();//чтение
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] StrList = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string CurrentF in StrList)
            {//добавляем поддержку драга, следим за многими файлами, берем последний
                if (CurrentF.Substring(CurrentF.Length - 4) == ".bmp")
                {
                    Path = CurrentF;
                    Preview.Load(Path);
                    points = new List<byte>(Preview.Width * Preview.Height + 1);
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {//поддержка дропа
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {//открыие изображения
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                Path = openFileDialog1.FileName;
                Preview.Load(Path);
                points = new List<byte>(Preview.Width * Preview.Height + 1);
            }
        }

        

        private void распознатьToolStripMenuItem_Click(object sender, EventArgs e)
        {//распознание
            points.Clear();
            points.Add(1);
            
            
            Bitmap temp = new Bitmap(Preview.Image,new Size(320,240));
            
            for (int i = 0; i < temp.Width; i++)
                for (int j = 0; j < temp.Height; j++)
                {
                    Color current = temp.GetPixel(i, j);
                    if (current.R > 127 && current.G > 127 && current.B > 127)
                        temp.SetPixel(i, j, Color.White);
                    else 
                        temp.SetPixel(i, j, Color.Black);
                }
                    for (int i = 1; i < points.Capacity; i++)
                        points.Add(temp.GetPixel((i - 1) % temp.Width, (i - 1) / temp.Width).ToArgb() != -1 ? (byte)1 : (byte)0);
            Result.Text = objects.Recognize(points);
        }

        private void исправитьToolStripMenuItem_Click(object sender, EventArgs e)
        {//исправление - изменяем цифру в текстовом поле, нажимаем исправить - изменение весов
            char smb;
            if (Result.Text == "")
                smb = '\n';
            else
                smb = Result.Text[0];
            Result.Text = objects.Correct(smb, points);
            Result.Text = "";
            
            this.Refresh();
            objects.Save();
            Result.Text = "Сохранен";
        }
    }
}
