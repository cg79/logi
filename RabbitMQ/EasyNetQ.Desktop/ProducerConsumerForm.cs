using EasyNetQ.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyNetQ.Desktop
{
    public partial class ProducerConsumerForm : Form
    {
        public ProducerConsumerForm()
        {
            InitializeComponent();
        }
        private RabbitMQPublisherConsumer rabbitMQSenderReceiver = null;
        public Action<string> action = null;
        delegate void ShowResponseCallbackcs(string arg1);
        public void receiveMessage(string aaa)
        {
            if (richTextBox1.InvokeRequired)
            {
                ShowResponseCallbackcs dd = new ShowResponseCallbackcs(receiveMessage);
                this.Invoke(dd, new object[] { aaa });
            }
            else
            {

                richTextBox1.Text += aaa;
            }


        }
        public void SetRabbitMQSenderReceiver(RabbitMQPublisherConsumer pubSub)
        {
            this.rabbitMQSenderReceiver = pubSub;
            //action = new Action<string>(a => receiveMessage(a));
            //rabbitMQSenderReceiver.SetMethodForReceivedMessage(action);
            rabbitMQSenderReceiver.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            rabbitMQSenderReceiver.Stop();
            base.OnClosed(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rabbitMQSenderReceiver.Send(textBox1.Text);
        }

    }
}
