using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consumer.direct
{
    /// <summary>
    /// 直接交互机消息消费者
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 1.创建消息代理连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                Password = "admin",
                UserName = "admin"
            };
            // 2.创建连接
            using (var connection = factory.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connection.CreateModel())
                {
                    // 4.定义队列
                    var queue = "direct.queue1";
                    channel.QueueDeclare(queue, false, false, false, null);
                    // 5.定义交换机
                    var exchange = "exchange.direct";
                    channel.ExchangeDeclare(exchange, "direct");
                    // 6.绑定
                    channel.QueueBind(queue, exchange, "queue");
                    // 7.创建消费者
                    var consumer = new EventingBasicConsumer(channel);
                    // 8.消息触发事件
                    consumer.Received += Consume_Received;
                    // 9.消息消费
                    channel.BasicConsume(queue, true, consumer);

                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        ///  消息处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consume_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("start receive message");
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(message);
            Console.WriteLine("consume message finished!");
        }
    }
}
