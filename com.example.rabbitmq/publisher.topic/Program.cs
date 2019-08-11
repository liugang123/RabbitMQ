using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publisher.topic
{
    class Program
    {
        /// <summary>
        /// 主题交换机消息生成者
        /// 绑定相同路由键的队列，消息会广播到所有队列
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 1.初始化连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
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
                    var queue1 = "topic.queue1";
                    var queue2 = "topic.queue2";
                    var queue3 = "topic.queue3";
                    channel.QueueDeclare(queue1, false, false, false, null);
                    channel.QueueDeclare(queue2, false, false, false, null);
                    channel.QueueDeclare(queue3, false, false, false, null);
                    // 5.定义交换机
                    var exchange = "exchange.topic";
                    channel.ExchangeDeclare(exchange, "topic");
                    // 6.绑定
                    channel.QueueBind(queue1, exchange, "queue");
                    channel.QueueBind(queue2, exchange, "queue2");
                    channel.QueueBind(queue3, exchange, "queue");
                    // 7.生成消息
                    while (true)
                    {
                        Console.WriteLine("please enter message");
                        var message = Console.ReadLine();
                        var msgContent = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange, "queue", null, msgContent);
                        channel.BasicPublish(exchange, "queue2", null, msgContent);
                        Console.WriteLine("message send success!");
                    }
                }
            }
        }
    }
}
