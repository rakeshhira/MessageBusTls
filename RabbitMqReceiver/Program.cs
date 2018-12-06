using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqCommon;

namespace RabbitMqReceiver
{
	class Program
	{
		static void Main(string[] args)
		{
			bool usingRabbitMqCommonHelper = false;
			if (!usingRabbitMqCommonHelper)
			{
				Console.WriteLine($@"env rakesh_name={Environment.GetEnvironmentVariable("rakesh_name")}");
				var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
				using (var connection = factory.CreateConnection())
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(queue: "hello",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);

					var consumer = new EventingBasicConsumer(channel);
					consumer.Received += (model, ea) =>
					{
						var body = ea.Body;
						var message = Encoding.UTF8.GetString(body);
						Console.WriteLine(" [x] Received {0}", message);
					};
					channel.BasicConsume(queue: "hello",
										 autoAck: true,
										 consumer: consumer);

					Console.WriteLine(" Press [enter] to exit.");
					Console.ReadLine();
				}
			}
			else
			{
				RabbitMQClientMgr rabbitMQClientMgr = RabbitMqCommonHelper.Initialize(args.ToList());

				rabbitMQClientMgr.EventingBasicConsumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine($" [x] Received {message}");
				};

				rabbitMQClientMgr.BasicConsume();

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();

				rabbitMQClientMgr.Uninitialize();
			}
		}
	}
}
