namespace Post.Query.Infrastructure.Dispatchers;

using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;

public class QueryDispatcher : IQueryDispatcher
{
	private readonly Dictionary<Type, Func<BaseQuery, Task>> _handlers = new();
	
	public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseQuery
	{
		if(_handlers.ContainsKey(typeof(T)))
			throw new IndexOutOfRangeException("You cannot register the same command handler twice!");
		
		_handlers.Add(typeof(T), x => handler((T)x));
	}

	public async Task SendAsync(BaseQuery query)
	{
		if (!_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task> handler))
			throw new ArgumentNullException(nameof(handler), "No query handler was registered!");

		await handler(query);
	}
}
