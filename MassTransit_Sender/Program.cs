using System;
using MassTransit;
using MassTransit_Messages;

namespace MassTransit_Sender
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Sender> Hello World!");

			var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
			{
				var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
				{
					h.Username("guest");
					h.Password("guest");
				});
			});

			bus.Start();
			var message = new HelloMessage { Text = $"{DateTime.Now.Ticks}> Hi" };
			bus.Publish<IHelloMessage>(message);

			Console.WriteLine($"Published '{message.Text}'");
			Console.WriteLine("Press any key to exit:");
			Console.ReadKey();

			bus.Stop();
		}
	}

	public class HelloMessage : IHelloMessage
	{
		public string Text { get; set; } = string.Empty;
		string IHelloMessage.Text => Text;
	}
}
