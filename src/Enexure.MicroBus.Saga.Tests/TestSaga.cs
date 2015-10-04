using System;
using System.Threading.Tasks;
using Enexure.MicroBus.Sagas;
using Enexure.MicroBus.Sagas.Repositories;

namespace Enexure.MicroBus.Saga.Tests
{
	public class TestSaga : ISaga, ISagaStartedBy<SagaStartingAEvent>, ISagaStartedBy<SagaStartingBEvent>, IEventHandler<SagaEndingEvent>
	{
		public Guid Id { get; protected set; }
		public bool IsCompleted { get; protected set; }

		public Task Handle(SagaStartingAEvent @event)
		{
			Id = @event.Identifier;

			return Task.FromResult(0);
		}

		public Task Handle(SagaStartingBEvent @event)
		{
			Id = @event.Identifier;

			return Task.FromResult(0);
		}

		public Task Handle(SagaEndingEvent @event)
		{
			IsCompleted = true; 

			return Task.FromResult(0);
		}
	}

	public class FinderA : ISagaFinder<TestSaga, SagaStartingAEvent>
	{
		private readonly ISagaStore store;

		public FinderA(ISagaStore store)
		{
			this.store = store;
		}

		public Task<TestSaga> FindByAsync(SagaStartingAEvent message)
		{
			return Task.FromResult((TestSaga)store.Get(message.Identifier));
		}
	}

	public class FinderB : ISagaFinder<TestSaga, SagaStartingBEvent>
	{
		private readonly ISagaStore store;

		public FinderB(ISagaStore store)
		{
			this.store = store;
		}

		public Task<TestSaga> FindByAsync(SagaStartingBEvent message)
		{
			return Task.FromResult((TestSaga)store.Find(x => x.Id == message.Identifier));
		}
	}
	public class FinderC : ISagaFinder<TestSaga, SagaEndingEvent>
	{
		private readonly ISagaStore store;

		public FinderC(ISagaStore store)
		{
			this.store = store;
		}

		public Task<TestSaga> FindByAsync(SagaEndingEvent message)
		{
			return Task.FromResult((TestSaga)store.Find(x => x.Id == message.Identifier));
		}
	}

	public class SagaStartingAEvent : IEvent
	{
		public Guid Identifier { get; set; }
	}

	public class SagaStartingBEvent : IEvent
	{
		public Guid Identifier { get; set; }
	}

	public class SagaEndingEvent : IEvent
	{
		public Guid Identifier { get; set; }
	}

}