namespace Post.Query.Infrastructure.Dispatchers;

using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Domain.Entities;

public class QueryDispatcher : IQueryDispatcher<PostEntity>
{
	private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();

	public void RegisterHandler<T>(Func<T, Task<List<PostEntity>>> handler) where T : BaseQuery
	{
		if (_handlers.ContainsKey(typeof(T)))
		{
			throw new IndexOutOfRangeException("You cannot register the same command handler twice!");
		}

		_handlers.Add(typeof(T), value: x => handler((T)x));
	}

	public async Task<List<PostEntity>> SendAsync(BaseQuery query)
	{
		if (!_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<PostEntity>>> handler))
		{
			throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
		}

		return await handler(query);
	}
}
