using System.Collections.Generic;

namespace NumbersSearcher
{
    //Хранение объектов
    public class Holder
    {
        double speed = 0.5;//статически зададим скорость обучения
        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        Net net;

        public string Recognize(List<byte> x)
        {
            recognition = net.Recognize(x);
            if (recognition == '\n')
                return "не удалось";
            return recognition.ToString();
        }

        char recognition;
        public char Recognition
        {
            get { return recognition; }
        }

       
        private int state = 0;
        public int State
        {
            get { return state; }
            set { state = value; }
        }
        private static Holder instance;
        private Holder(int width, int height)
        {//инициализация распознания образа с заданными размерами
            
            net = new Net(width * height);
            
        }
        public static Holder GetInstance(int width, int height)
        {
            if (instance == null)
            {
                instance = new Holder(width, height);
            }
            return instance;
        }
        public void AddNeuron(char symbol)
        {//новый нейрон
            net.AddNeuron(symbol);
        }

        public string Correct(char symbol, List<byte> x)
        {//исправление
            recognition = net.Correct(symbol,x,speed);
            if (recognition == '\n')
                return "не удалось";

            return recognition.ToString();
        }
        public void Save()
        {//сохранение
            net.Save();
        }
        public List<double> GetNeuron(char s)
        {//получение отдельного нейрона
            return net.GetNeuron(s);
        }
        
    }
}
