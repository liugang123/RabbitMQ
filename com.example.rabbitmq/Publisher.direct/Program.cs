using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.direct
{
    /// <summary>
    /// 队列名称和路由键完全匹配时，消息才会发送到指定的队列上
    /// 绑定相同路由键的队列，消息会广播到所有队列
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //  1.初始化消息代理连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };
            // 2.创建连接
            using (var connetion = factory.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connetion.CreateModel())
                {
                    // 4.定义交换机
                    var exchange = "exchange.direct";
                    channel.ExchangeDeclare(exchange, "direct", false, false, null);
                    // 5.定义队列
                    var queue1 = "direct.queue1";
                    var queue2 = "direct.queue2";
                    var queue3 = "direct.queue3";
                    channel.QueueDeclare(queue1, false, false, false, null);
                    channel.QueueDeclare(queue2, false, false, false, null);
                    channel.QueueDeclare(queue3,false,false,false,null);
                    // 6.绑定
                    channel.QueueBind(queue1, exchange, "queue", null);
                    channel.QueueBind(queue2, exchange, "queue2", null);
                    channel.QueueBind(queue3,exchange,"queue",null);
                    // 7.发送消息
                    while (true)
                    {
                        Console.WriteLine("please input message");
                        var message = Console.ReadLine();
                        var msgContent = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange, "queue", null, msgContent);
                        channel.BasicPublish(exchange, "queue2", null, msgContent);
                        Console.WriteLine("message publish finished!");
                    }
                }
            }


        }
    }
}
