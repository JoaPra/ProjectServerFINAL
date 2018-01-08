using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectServer
{
    public partial class Form1 : Form
    {
        private TcpListener Server;
        private TcpClient Client;

        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            void TcpClientWait()
            {
                while (true)
                {
                    Byte[] bytes = new Byte[256];
                    String data = null;
                    Invoke(new Action(() => // 
                    {
                        listBox1.Items.Add("Czekam na połączenie z klientem"); //to nie moze sie wykonać z innego wątku niż główny wątek
                    }));
                    TcpClient client = Server.AcceptTcpClient();
                    Invoke(new Action(() => // 
                    {
                        listBox1.Items.Add("Połączono z klientem"); 
                    }));
                    NetworkStream stream = client.GetStream();
                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // tłumaczenie stringa na Ascii
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        

                        // zmieniamy sobie na wielkie litery
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        stream.Write(msg, 0, msg.Length);
                        Invoke(new Action(() => // 
                        {
                            listBox2.Items.Add(data);
                        }));
                    }
                }

            }
            IPAddress addressIp;
            try
            {
                addressIp = IPAddress.Parse(textBox1.Text);
            }
            catch
            {
                textBox1.Text = string.Empty;
                return;
            }

            int port = Convert.ToInt16(numericUpDown1.Value);

            try
            {
                Server = new TcpListener(addressIp, port);
                Server.Start();
                listBox1.Items.Add("Serwer wystartował poprawnie");

                button1.Enabled = false;
                button2.Enabled = true;

                Thread thread = new Thread(TcpClientWait);
                thread.Start();


            }
            catch
            {
                listBox1.Items.Add("Nieudane połączenie");
                listBox1.Update();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
