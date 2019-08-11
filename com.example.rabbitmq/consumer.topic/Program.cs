using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consumer.topic
{
    /// <summary>
    /// 主题交换机消息消费者
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 1.创建连接工厂
            var facotry = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };
            // 2.创建连接
            using (var connection = facotry.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connection.CreateModel())
                {
                    // 4.定义队列
                    var queue = "topic.queue1";
                    channel.QueueDeclare(queue, false, false, false, null);
                    // 5.定义交换机
                    var exchange = "exchange.topic";
                    channel.ExchangeDeclare(exchange, "topic");
                    // 6.绑定
                    channel.QueueBind(queue, exchange, "queue");
                    // 7.创建消费者
                    var consumer = new EventingBasicConsumer(channel);
                    // 8.触发消费事件
                    consumer.Received += Consumer_Received;
                    // 9.消费消息
                    channel.BasicConsume(queue, true, consumer);
                    Console.ReadKey();
                }
            }
        }
        /// <summary>
        /// 消息处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("start consumer message");
            var message = e.Body;
            var msgContent = Encoding.UTF8.GetString(message);
            Console.WriteLine(msgContent);
            Console.WriteLine("message handler finished!");
        }

    }
}
