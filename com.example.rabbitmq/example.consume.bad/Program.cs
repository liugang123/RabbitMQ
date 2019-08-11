using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example.consume.bad
{
    /// <summary>
    /// 消息消费者（异常消费端）
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var count = 0;
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
                    var queue = "example.queue";
                    channel.QueueDeclare(queue, true, false, false, null);
                    // 4.1 公平分发机制 prefetchCount = 1
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    // 5.定义交换机
                    var exchange = "example.exchange";
                    channel.ExchangeDeclare(exchange, "topic");
                    // 6.绑定
                    channel.QueueBind(queue, exchange, "example");
                    // 7.定义消费者
                    var consume = new EventingBasicConsumer(channel);
                    // 8.触发消费事件
                    consume.Received += (sender, e) =>
                    {

                        Console.WriteLine("start consume message !");
                        var message = Encoding.UTF8.GetString(e.Body);
                        Console.WriteLine(message);
                        count++;
                        if (count < 3)
                        {
                            // 手动确认机制
                            channel.BasicAck(e.DeliveryTag, false);
                        }
                        else
                        {
                            Console.WriteLine("消费端异常，消息未确认");
                        }
                    };
                    // 9.消费消息
                    // noAck=true:自动消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
                    // autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
                    channel.BasicConsume(queue, false, consume);
                    Console.ReadLine();
                }
            }
        }
    }
}
