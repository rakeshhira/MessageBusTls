using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit_Messages;

namespace MassTransit_Receive
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Receiver> Hello World!");

			//var dict = new Dictionary<string, string>
			//{
			//	{"MemoryCollectionKey1", "value1"},
			//	{"MemoryCollectionKey2", "value2"}
			//};

			//var config = new ConfigurationBuilder()
			//	.AddInMemoryCollection(dict)
			//	.Build();M

			//IServiceCollection serviceCollection = new ServiceCollection();
			//serviceCollection.AddLogging(
			//		configure =>
			//		{
			//			configure.AddConsole();
			//			configure.AddDebug();
			//		})
			//	.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

			//ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

			var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
			{
				//var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
				//var host = sbc.Host("localhost", 5672, "/", (h) =>
				var host = sbc.Host("rabbitmq", 5671, "/", (h) =>
				{
					//h.Username("guest");
					//h.Password("guest");
					h.Username("52w4dIglmvEBGhHYASKj");
					h.Password("OvP0CXIq2azAF8QMnfTr");
				});

				sbc.ReceiveEndpoint(host, "your_message_queue", e => { e.Consumer(() => new HelloMessageConsumer()); });

				//sbc.UseExtensionsLogging(new LoggerFactory());
				//sbc.UseExtensionsLogging(serviceProvider.GetRequiredService<ILoggerFactory>());
			});
			bus.Start();

			var message = new HelloMessage { Text = $"{DateTime.Now.Ticks}> Hi" };
			bus.Publish<IHelloMessage>(message);

			Console.WriteLine("Press any key to exit");
			Console.ReadKey();

			bus.Stop();
		}
	}

	public class HelloMessage : IHelloMessage
	{
		public string Text { get; set; } = string.Empty;
		string IHelloMessage.Text => Text;
	}


	public class HelloMessageConsumer : IConsumer<IHelloMessage>
	{
		public async Task Consume(ConsumeContext<IHelloMessage> context)
		{
			await Console.Out.WriteLineAsync($"{DateTime.Now.Ticks}: Updating consumer: {context.Message.Text}");

			// update the customer address
		}
	}
}
