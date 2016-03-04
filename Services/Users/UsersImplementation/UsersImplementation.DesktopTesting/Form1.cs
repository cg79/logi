using EasyNetQ.DTO;
using MongoRepository.Pagination;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsersImplementation.Repositories;

namespace UsersImplementation.DesktopTesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        RabbitMQPublisherConsumer rabbitMQPublisherConsumer;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //PageRequest pr = new PageRequest();
            //pr.FilterCriteria = new List<FilterCriteria>();
            //pr.FilterCriteria.Add(new FilterCriteria()
            //{
            //    FieldName = "NrInm",
            //    Operator = Operator.Contains,
            //    Value = ""
            //});
            //pr.PageIndex = 0;
            //pr.RowsPerPage = 5;

            //UserCarRepository userCarRepository = new UserCarRepository();
            //var rez = userCarRepository.GetPage(pr);
            //return;

            rabbitMQPublisherConsumer = new RabbitMQPublisherConsumer();
            rabbitMQPublisherConsumer.SetQueuePropertiesForReceiver(new QueuePropertiesReceiver()
            {
                QueueName = "QLPusher",
                AutoDelete = false,
                Durable = true,
                ExchangeProperties = new ExchangeProperties() { ExchangeName = "LPusher", ExchangeType = "fanout" },
                RoutingKey = string.Empty
            });
            rabbitMQPublisherConsumer.SetQueuePropertiesForSender(new QueuePropertiesPublisher()
            {
                AutoDelete = false,
                Durable = true,
                ExchangeProperties = new ExchangeProperties() { ExchangeName = "LResponse", ExchangeType = "fanout" },
                RoutingKey = string.Empty
            });
            rabbitMQPublisherConsumer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransportOfferRepository transportOfferRepository = new TransportOfferRepository();
            transportOfferRepository.aaa();
            return;
            CarRepository carRepository = new Repositories.CarRepository();
            CarModelRepository carModelRepository = new CarModelRepository();

            string jsonString = File.ReadAllText(@"d:\claudiu\Logistica\Regex\n1.txt");

            JObject albums = JObject.Parse(jsonString) as JObject;

            dynamic json = JValue.Parse(jsonString);

            int index = 0;
            int k = 0;
            foreach (dynamic test in json)
            {
                index++;
                Console.Write(test.Name);
                //carRepository.Save(new UsersImplementation.DTO.Car() { ID = index,Name = test.Name });

                
                foreach (dynamic test1 in test)
                {
                    k = 0;
                    foreach (dynamic model in test1)
                    {
                        k++;
                        carModelRepository.Save(new DTO.CarModel() {ID = k,Name = model.Value,CarID=index });
                        Console.Write(model.Value);

                    }

                }

            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            rabbitMQPublisherConsumer.Stop();
            rabbitMQPublisherConsumer = null;
        }
    }
}
