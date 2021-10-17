using System;
using System.IO.Ports;
namespace SerialConsole
{
    class Program
    {
        private SerialPort sport;

        private  void Start()
        {
            sport = new SerialPort();
            sport.PortName = "COM3";
            sport.BaudRate = 9600;
            sport.Encoding = System.Text.Encoding.ASCII;
            sport.Open();
            
            Console.WriteLine("sport.ReadTimeout:" + sport.ReadTimeout);
            int readCode=sport.ReadChar();
            Console.WriteLine("readCode:" + readCode);
            int lineLength = 100;
            int count = 0;
            while (true)
            {
                readCode = sport.ReadChar();
               
                Console.Write(((char)readCode));
                count++;
                if (count == lineLength)
                {
                    count = 0;
                 
                }

            }
            
            sport.Close();
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Start();
            Console.WriteLine("Hello World!");
        }
    }
}
