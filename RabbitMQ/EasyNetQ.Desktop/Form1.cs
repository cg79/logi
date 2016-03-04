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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.TextChanged += comboBox1_TextChanged;
            comboBox1.Text = "fanout";

            comboBox2.TextChanged += comboBox2_TextChanged;
            comboBox2.Text = "fanout";

            
        }

        void comboBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox2.Text;
        }

        void comboBox1_TextChanged(object sender, EventArgs e)
        {
            txtExchangeType.Text = comboBox1.Text;
        }

        private QueuePropertiesReceiver CreateReceiverQProp()
        {
            QueuePropertiesReceiver qp = new QueuePropertiesReceiver()
            {
                QueueName = txtQueueName.Text,
                RoutingKey = txtRoutingKey.Text,
                ExchangeProperties = new ExchangeProperties() 
                {
                    ExchangeName = txtExchangeName.Text,
                    ExchangeType = txtExchangeType.Text
                },
                Durable = chkDurable1.Checked,
                AutoDelete = chkAutoDelete1.Checked
            };
            return qp;
        }
        private QueuePropertiesReceiver CreateReceiverQProp1()
        {
            QueuePropertiesReceiver qp = new QueuePropertiesReceiver()
            {
                QueueName = textBox4.Text,
                RoutingKey = textBox2.Text,
                ExchangeProperties = new ExchangeProperties()
                {
                    ExchangeName = textBox3.Text,
                    ExchangeType = textBox1.Text
                },
                Durable = chkDurable2.Checked,
                AutoDelete = chkAutoDelete2.Checked
            };
            return qp;
        }
        private QueuePropertiesPublisher CreateSenderQProp()
        {
            QueuePropertiesPublisher qp = new QueuePropertiesPublisher()
            {
                RoutingKey = txtRoutingKey.Text,
                ExchangeProperties = new ExchangeProperties()
                {
                    ExchangeName = txtExchangeName.Text,
                    ExchangeType = txtExchangeType.Text
                },
                Durable = chkDurable1.Checked,
                AutoDelete = chkAutoDelete1.Checked
            };
            return qp;
        }
        private QueuePropertiesPublisher CreateSenderQProp1()
        {
            QueuePropertiesPublisher qp = new QueuePropertiesPublisher()
            {
                RoutingKey = textBox2.Text,
                ExchangeProperties = new ExchangeProperties()
                {
                    ExchangeName = textBox3.Text,
                    ExchangeType = textBox1.Text
                },
                Durable = chkDurable2.Checked,
                AutoDelete = chkAutoDelete2.Checked
            };
            return qp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProducerForm f = new ProducerForm();
            f.QueueProperties = CreateSenderQProp();
            f.Show();
            f.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConsumerForm cf = new ConsumerForm();
            cf.QueueProperties = CreateReceiverQProp();
            if (checkBox1.Checked)
            {
                cf.QueueProperties.QueueName = Guid.NewGuid().ToString();
            }
            cf.Show();

            cf.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProducerConsumerForm prodCons = new ProducerConsumerForm();
            RabbitMQPublisherConsumer pubSub = new RabbitMQPublisherConsumer();

            pubSub.SetQueuePropertiesForReceiver(CreateReceiverQProp());
            pubSub.SetQueuePropertiesForSender(CreateSenderQProp1());
            
            

            prodCons.SetRabbitMQSenderReceiver(pubSub);
            prodCons.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ConsumerFormResponse cf = new ConsumerFormResponse();
            cf.QueueProperties = CreateReceiverQProp1();
            if (checkBox2.Checked)
            {
                cf.QueueProperties.QueueName = Guid.NewGuid().ToString();
            }
            cf.Show();

            cf.Start();
        }
    }
}
