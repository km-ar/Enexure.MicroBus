﻿using System.Threading.Tasks;
using Enexure.MicroBus.MessageContracts;
using Enexure.MicroBus.Tests.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Enexure.MicroBus.Tests.Autofac
{
	[TestFixture]
	public class AutofacQueryTests
	{
		class Query : IQuery<Query, Result> { }

		class Result : IResult { }

		class QueryHandler : IQueryHandler<Query, Result>
		{
			public Task<Result> Handle(Query query)
			{
				return Task.FromResult(new Result());
			}
		}

		[Test]
		public async Task TestQuery()
		{
			var pipline = new Pipeline()
				.AddHandler<PipelineHandler>();

			var bus = new BusBuilder()
				.RegisterQuery<Query, Result>().To<QueryHandler>(pipline)
				.BuildBus();

			var result = await bus.Query(new Query());

			result.Should().NotBeNull();

		}
	}
}
