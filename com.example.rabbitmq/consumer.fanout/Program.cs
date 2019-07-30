using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consumer.fanout
{
    class Program
    {
        /// <summary>
        /// fanout消息消费者
        /// </summary>
        static void Main(string[] args)
        {
            // 1.初始化连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

            // 2.创建连接
            using (var connection = factory.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connection.CreateModel())
                {
                    // 4.定义队列
                    var queue = "fanout.queue1";
                    channel.QueueDeclare(queue, false, false, false, null);
                    // 5.定义交换机
                    channel.ExchangeDeclare("fanout.exchange", "fanout");
                    // 6.绑定
                    channel.QueueBind(queue, "fanout.exchange", "");
                    // 7.定义消费者
                    var consumer = new EventingBasicConsumer(channel);
                    // 8.触发消费事件
                    consumer.Received += Consumer_Received;
                    // 9.消费消息
                    channel.BasicConsume(queue, true, consumer);

                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 消息消费事件
        /// </summary>
        /// <param name="e"></param>
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("received message:" + message);
        }

    }
}
