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
using ESB.Utils.Reflection;

namespace EasyNetQ.Desktop
{
    public partial class ProducerForm : Form
    {
        public ProducerForm()
        {
            InitializeComponent();
        }
        public QueuePropertiesPublisher QueueProperties { get; set; }

        RabbitMQPublisher producer;
        internal void Start()
        {
            this.Text = QueueProperties.ExchangeProperties.ExchangeName + " " + QueueProperties.RoutingKey;
            producer = new RabbitMQPublisher();
            producer.SetQueueProperties(QueueProperties);
            producer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            producer.Send(textBox1.Text, QueueProperties.RoutingKey);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            producer.Send(textBox1.Text,textBox2.Text);
        }
    }
}
