using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    /// <summary>
    /// 默认交换机，消息消费者
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 1.初始化工厂实列
            var factory = new ConnectionFactory() { HostName = "localhost" };
            // 2.创建连接
            using (var connection = factory.CreateConnection())
            {
                // 3.创建信道
                using (var channel = connection.CreateModel())
                {
                    // 4.申明队列
                    var queue = "default.exchange";
                    channel.QueueDeclare(queue, false, false, false, null);
                    // 5.定义消息消费者
                    var consumer = new EventingBasicConsumer(channel);
                    // 6.绑定消息处理事件
                    consumer.Received += Consumer_Received;
                    // 7.启动消费者
                    channel.BasicConsume(queue, true, consumer);

                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 消息消费方法
        /// </summary>
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("receive message : " + message);
        }

    }
}
