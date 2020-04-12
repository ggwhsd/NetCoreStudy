using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LambdaAsync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button4.Click += async (sender, e) =>
            {
                await ExampleTaskAsync();
                MessageBox.Show("After 5000 output");
            };
        }

        private async void ExampleAsync()
        {
            await Task.Delay(1000);
            MessageBox.Show("After 1000 output");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExampleAsync();
        }

        private Task ExampleTaskAsync()
        {
            return Task.Delay(5000);
            
        }
        private async void CallTask()
        {
            await ExampleTaskAsync();
            MessageBox.Show("After 5000 output");

        }
        private void button2_Click(object sender, EventArgs e)
        {
            CallTask();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await ExampleTaskAsync();
            MessageBox.Show("After 5000 output");
        }
    }
}
