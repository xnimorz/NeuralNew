using System.Collections.Generic;
using System.IO;

namespace NumbersSearcher
{
    //сети
    class Net
    {
        //создание сети с заданным разрешением изображения (количество точек)
        public Net(int pointCount)
        {
            resolution = pointCount;
        }

      
        public void Save()
        {  //сохранение всех нейронов в файл
            foreach (Neuron n in net)
                n.Save();
        }

        int resolution;
        List<Neuron> net = new List<Neuron>();

        
        public void AddNeuron(char symbol)
        {//добавление нейрона - create
            net.Add(new Neuron(symbol, resolution));
        }
        int lastNeuron;

      
        public char Recognize(List<byte> x)
        {//распознание, поиск, сравнени
            char result = '\n';
            lastNeuron = -1;
            foreach(Neuron n in net)
                if (n.Y(x) == 1)
                {
                    result = n.Symbol;
                    lastNeuron = net.IndexOf(n);
                }
            
            return result;
        }


        
        public char Correct(char symbol, List<byte> x, double speed)
        {//корректировка
            bool check = false;
                for (int i = 0; i < net.Count; i++)
                {
                    if (symbol == '\n')
                    {
                        if (net[i].LastY > 0)
                            net[i].Correct(x, -1, speed);
                    }
                    else
                    {
                        if (net[i].Symbol == symbol)
                        {
                            net[i].Correct(x, 1, speed);
                            check = true;
                        }
                        else if (net[i].LastY > 0)
                            net[i].Correct(x, -1, speed);
                    }
                }
                if (!check)
                {
                    net.Add(new Neuron(symbol, x.Count-1));
                    net[net.Count - 1].Correct(x, 1, speed);
                    StreamWriter write = new StreamWriter("chars.txt", true);
                    write.WriteLine(symbol);
                    write.Close();
                }
            return Recognize(x);
        }

     
        public List<double> GetNeuron(char s)
        {   //данные о нейроне
            foreach (Neuron n in net)
                if (n.Symbol == s)
                    return n.w;
            return null;
        }

    }
}
