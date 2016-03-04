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
using ESB.Utils.Serializers;
using System.Threading;
using ESB.Utils.Reflection;

namespace EasyNetQ.Desktop
{
    public partial class ConsumerForm : Form
    {
        public ConsumerForm()
        {
            InitializeComponent();
        }

        public Action<string> action = null;

        public QueuePropertiesReceiver QueueProperties { get; set; }
        RabbitMQRequestConsumer consumer = null;
        delegate void ShowResponseCallbackcs(string arg1);

        public  void receiveMessage(string aaa)
        {
            var lkkjhkh = Thread.CurrentThread.ManagedThreadId;
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
        internal void Start()
        {
            this.Text = QueueProperties.ExchangeProperties.ExchangeName + " " +QueueProperties.QueueName + " " + QueueProperties.RoutingKey;
            action = new Action<string>(a => receiveMessage(a));

            consumer = new RabbitMQRequestConsumer();
            consumer.SetQueueProperties(QueueProperties);
            consumer.SetMethodForReceivedMessage(action);
            //consumer.MessageReceivedEvent += consumer_MessageReceivedEvent;
            consumer.Start();
        }

        void consumer_MessageReceivedEvent(string obj)
        {
            receiveMessage(obj);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (consumer != null)
            {
                consumer.Stop();
                consumer = null;
            }
            base.OnClosing(e);
        }
    }
}
