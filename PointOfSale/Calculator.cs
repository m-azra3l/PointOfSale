using System;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class Calculator : Form
    {
        decimal num1;
        decimal num2;
        string operation;
        public Calculator()
        {
            InitializeComponent();
        }
        private void input(string a)
        {
            if (textBox1.Text == "0")
                textBox1.Text = a;
            else
                textBox1.Text += a;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            input("7");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            input("8");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            input("9");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            input("4");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            input("5");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            input("6");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            input("1");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            input("2");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            input("3");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input("0");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text += ".";
        }

        private void button24_Click(object sender, EventArgs e)
        {
            num2 = decimal.Parse(textBox1.Text);
            ////////////////////////////////
            switch (operation)
            {
                case "+":
                    textBox1.Text = (num1 + num2).ToString();
                    break;
                case "-":
                    textBox1.Text = (num1 - num2).ToString();
                    break;
                case "*":
                    textBox1.Text = (num1 * num2).ToString();
                    break;
                case "/":
                    textBox1.Text = (num1 / num2).ToString();
                    break;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            num1 = decimal.Parse(textBox1.Text);
            operation = "+";
            textBox1.Text = "0";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            num1 = decimal.Parse(textBox1.Text);
            operation = "-";
            textBox1.Text = "0";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            num1 = decimal.Parse(textBox1.Text);
            operation = "/";
            textBox1.Text = "0";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            num1 = decimal.Parse(textBox1.Text);
            operation = "*";
            textBox1.Text = "0";
        }

        private void button25_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
        }

        private void picMinimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}