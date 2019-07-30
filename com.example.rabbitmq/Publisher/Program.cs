using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    /// <summary>
    /// 默认交换机，消息生产者
    /// 每个新建队列都会自动绑定到默认交换机上
    /// 绑定的路由键（routeKey）和队列名称一样
    /// 默认交换机属于direct类型
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 1.初始化连接工厂
            var factory = new ConnectionFactory() { HostName = "localhost" };
            // 2.建立连接
            using (var connection = factory.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connection.CreateModel())
                {
                    // 4.申明队列
                    string queue = "default.exchange";
                    channel.QueueDeclare(queue, false, false, false, null);

                    // 5 生产消息
                    while (true)
                    {
                        Console.WriteLine("please input mesage：");
                        var message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);

                        // 6.发送数据包
                        channel.BasicPublish("", queue, null, body);

                        Console.WriteLine("send finish:" + message);
                    }
                }
            }
        }
    }
}
