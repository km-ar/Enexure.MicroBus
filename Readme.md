Enexure.MicroBus
=================
[![Build status](https://ci.appveyor.com/api/projects/status/nwb1ebtfxiedyput/branch/master?svg=true)](https://ci.appveyor.com/project/Daniel45729/enexure-microbus/branch/master)

MicroBus is a simple in process mediator for .NET

> PM> Install-Package [Enexure.MicroBus](https://www.nuget.org/packages/Enexure.MicroBus/)

I wanted a super simple mediator with as few dependencies as possible. You can register handlers and pipelines without dependency injection, use a dependency injection framework or write your own activator. 

MicroBus supports the three fundamental bus message types, commands, events and queries(request/response). 

	bus.Send(new TestCommand());
	
	bus.Query(new TestQuery());
	
	bus.Publish(new TestEvent());
	
Here's what a command and it's handler look like
	
	class TestCommand : ICommand
	{
	}
	
	class TestCommandHandler : ICommandHandler<TestCommand>
	{
		public async Task Handle(TestCommand command)
		{
			Console.WriteLine("Test command handler");
		}
	}

If you need to handle cross cutting concerns you can use a pipeline handler

	public class CrossCuttingHandler : IPipelineHandler
	{
		private readonly IPipelineHandler innerHandler;

		public CrossCuttingHandler(IPipelineHandler innerHandler)
		{
			this.innerHandler = innerHandler;
		}

		public async Task Handle(IMessage message)
		{
			Console.WriteLine("Cross cutting handler");

			await innerHandler.Handle(message);
		}
	}
	
[Registration with Autofac](https://www.nuget.org/packages/Enexure.MicroBus.Autofac/) would look like this
	
	var containerBuilder = new ContainerBuilder();

	containerBuilder.RegisterMicroBus(builder => {

		var pipeline = builder.CreatePipeline()
			.AddHandler<CrossCuttingHandler>();

		builder.RegisterHandler<TestCommandHandler>(pipeline);
	});

	var container = containerBuilder.Build();

	