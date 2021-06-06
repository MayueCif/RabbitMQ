using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace rabbitmq
{
    class Program
    {
        static void Main(string[] args)
        {
 
            var factory = new ConnectionFactory {
                HostName = "localhost",   //主机名
                UserName = "mymq",        //默认用户名
                Password = "123456",      //默认密码
                RequestedHeartbeat = TimeSpan.FromSeconds(30)
            };

            using (var connection = factory.CreateConnection())//连接服务器
            {

                //connection.ConnectionShutdown
                //创建一个通道
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("stacking", false, false, false, null);//创建消息队列
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 1;
                    properties.Priority = 2;
                    properties.ContentType = "text/plain";
                    properties.Expiration = "60000";
                    string message = "RabbitMQ Test"; //传递的消息内容
                    channel.BasicPublish("", "stacking", properties, Encoding.UTF8.GetBytes(message)); //生产消息

                    Console.WriteLine($"Send:{message}");

                    //var consumer = new RabbitMQConsumer(channel);
                    //consumer.Received += (ch, ea) =>
                    //{
                    //    var body = ea.Body.ToArray();
                    //    Console.WriteLine($"Received:{Encoding.UTF8.GetString(body)}");
                    //    channel.BasicAck(ea.DeliveryTag, false);
                    //};
                    //var consumerTag = channel.BasicConsume("stacking", false, consumer);
                    //channel.BasicCancel(consumerTag);
                    //Console.ReadKey();

                    var result = channel.BasicGet("stacking",false);
                    Console.WriteLine($"Received:{Encoding.UTF8.GetString(result.Body.ToArray())}");
                    channel.BasicAck(result.DeliveryTag, false);
                    //channel.BasicReject()
                    //channel.BasicNack()

                    channel.ModelShutdown += (s,e)=>{ 
                    
                    };

                    //channel.Close();
                }

                //using (var channel = connection.CreateModel())
                //{
                //    var exchangeName = "exchange_name";
                //    channel.ExchangeDeclare(exchangeName, "direct", true);//创建一个持久化的、非自动删除的、绑定类型为direct的交换器
                //    var queueName = channel.QueueDeclare().QueueName;   //创建一个非持久化的、排他的、自动删除的队列(队列名由RabbitMQ自动生成)
                //    channel.QueueBind(queueName, exchangeName, "routing_key");  //使用路由键(routing_key)将队列和交换器绑定

                //    channel.QueueDeclare("queue_name", true);   // QueueDeclare拥有多个重载
                //}

                //connection.Close();
            }

        }

    }
}
