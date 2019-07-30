using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publisher.fanout
{
    class Program
    {
        /// <summary>
        ///  fanout类型的交换机会将消息发送到所有绑定的队列上
        ///  不考虑绑定的路由键
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

            // 2.创建连接对象
            using (var connection = factory.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connection.CreateModel())
                {
                    // 4.创建队列
                    var queue1 = "fanout.queue1";
                    var queue2 = "fanout.queue2";
                    channel.QueueDeclare(queue1, false, false, false, null);
                    channel.QueueDeclare(queue2, false, false, false, null);
                    // 5.创建交换机
                    channel.ExchangeDeclare("fanout.exchange", "fanout");
                    // 6.绑定
                    channel.QueueBind(queue1, "fanout.exchange", "");
                    channel.QueueBind(queue2, "fanout.exchange", "");

                    // 7. 生成消息
                    while (true)
                    {
                        Console.WriteLine("please enter message:");
                        var message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);

                        // 发送消息
                        channel.BasicPublish("fanout.exchange", "", null, body);
                        Console.WriteLine("message send finish :" + message);
                    }
                    //channel.BasicPublish("fanout.exchange",,)
                }
            }
        }

    }
}
