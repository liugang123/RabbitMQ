using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pulisher.example
{
    /// <summary>
    /// 消息生产者实例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 1.初始化连接工厂
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
                    var queue = "example.queue";
                    // drubale=true:队列对消息初始化
                    channel.QueueDeclare(queue, true, false, false, null);
                    // 5.定义交换机
                    var exchange = "example.exchange";
                    channel.ExchangeDeclare(exchange, "topic");
                    // 6.绑定
                    channel.QueueBind(queue, exchange, "example");
                    // 7.生产消息
                    while (true)
                    {
                        Console.WriteLine("please enter message");
                        var message = Console.ReadLine();
                        var messageBody = Encoding.UTF8.GetBytes(message);
                        // 设置消息属性：持久化
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        // 8.发送消息 
                        channel.BasicPublish(exchange, "example", properties, messageBody);

                    }
                }
            }
        }
    }
}
