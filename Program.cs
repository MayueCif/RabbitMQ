using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
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
                //using (var channel = connection.CreateModel())
                //{
                //    channel.QueueDeclare("stacking", false, false, false, null);//创建消息队列
                //    var properties = channel.CreateBasicProperties();
                //    properties.DeliveryMode = 1;
                //    properties.Priority = 2;
                //    properties.ContentType = "text/plain";
                //    properties.Expiration = "60000";
                //    string message = "RabbitMQ Test"; //传递的消息内容
                //    channel.BasicPublish("", "stacking", properties, Encoding.UTF8.GetBytes(message)); //生产消息

                //    Console.WriteLine($"Send:{message}");

                //    //var consumer = new RabbitMQConsumer(channel);
                //    //consumer.Received += (ch, ea) =>
                //    //{
                //    //    var body = ea.Body.ToArray();
                //    //    Console.WriteLine($"Received:{Encoding.UTF8.GetString(body)}");
                //    //    channel.BasicAck(ea.DeliveryTag, false);
                //    //};
                //    //var consumerTag = channel.BasicConsume("stacking", false, consumer);
                //    //channel.BasicCancel(consumerTag);
                //    //Console.ReadKey();

                //    var result = channel.BasicGet("stacking",false);
                //    Console.WriteLine($"Received:{Encoding.UTF8.GetString(result.Body.ToArray())}");
                //    channel.BasicAck(result.DeliveryTag, false);
                //    //channel.BasicReject()
                //    //channel.BasicNack()

                //    channel.ModelShutdown += (s,e)=>{ 

                //    };

                //    //channel.Close();
                //}

                //using (var channel = connection.CreateModel())
                //{
                //    var exchangeName = "exchange_name";
                //    channel.ExchangeDeclare(exchangeName, "direct", true);//创建一个持久化的、非自动删除的、绑定类型为direct的交换器
                //    var queueName = channel.QueueDeclare().QueueName;   //创建一个非持久化的、排他的、自动删除的队列(队列名由RabbitMQ自动生成)
                //    channel.QueueBind(queueName, exchangeName, "routing_key");  //使用路由键(routing_key)将队列和交换器绑定

                //    channel.QueueDeclare("queue_name", true);   // QueueDeclare拥有多个重载
                //}

                //connection.Close();


                //using (var channel = connection.CreateModel())
                //{
                //    //设置备胎交换器参数
                //    var arguments = new Dictionary<string, object>();
                //    arguments.Add("alternate-exchange","myAe");
                //    channel.ExchangeDeclare("normalExchange", "direct",true,false, arguments);
                //    channel.ExchangeDeclare("myAe", "fanout", true, false);
                //    channel.QueueDeclare("nromalQueue",true,false,false);
                //    channel.QueueBind("nromalQueue", "normalExchange", "normalKey");
                //    channel.QueueDeclare("unroutedQueue", true, false, false);
                //    channel.QueueBind("unroutedQueue", "myAe","ae");

                //    var properties = channel.CreateBasicProperties();
                //    properties.DeliveryMode = 2;
                //    string message = "RabbitMQ Test"; //传递的消息内容
                //    channel.BasicPublish("normalExchange", "normalKey", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                //    channel.BasicPublish("normalExchange", "un-routkey", properties, Encoding.UTF8.GetBytes(message)); //生产消息

                //    Console.WriteLine($"Send:{message}");
                //}


                //using (var channel = connection.CreateModel())
                //{
                //    var arguments = new Dictionary<string, object>();
                //    arguments.Add("x-expires", 10000);  //单位毫秒
                //    arguments.Add("x-message-ttl", 6000);  //单位毫秒
                //    channel.QueueDeclare(arguments: arguments);

                //    channel.ExchangeDeclare("dlx_exchange", "direct");
                //    var argumentsDlx = new Dictionary<string, object>();
                //    argumentsDlx.Add("x-dead-letter-exchange", "dlx_exchange");
                //    argumentsDlx.Add("x-dead-letter-routing-key", "dlx-routing-key"); //为DLX指定路由键，如果没有特殊指定，则使用原队列的路由键
                //    channel.QueueDeclare("dlx_queue",false,false,false,argumentsDlx);
                //}

                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare("priority_exchange", "direct");
                    var arguments = new Dictionary<string, object>();
                    arguments.Add("x-max-priority", 10);  //
                    channel.QueueDeclare("priority_queue", false, false, false, arguments);
                    channel.QueueBind("priority_queue", "priority_exchange", "priority_key");

                    var properties = channel.CreateBasicProperties();
                    properties.Priority = 2;
                    string message = "Priority 2"; //传递的消息内容
                    channel.BasicPublish("priority_exchange", "priority_key", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                    properties.Priority = 5;
                    message = "Priority 5"; //传递的消息内容
                    channel.BasicPublish("priority_exchange", "priority_key", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                    var result = channel.BasicGet("priority_queue", false);
                    channel.BasicAck(result.DeliveryTag, true);
                    Console.WriteLine($"Received:{Encoding.UTF8.GetString(result.Body.ToArray())}");

                }
            }

        }

    }
}
