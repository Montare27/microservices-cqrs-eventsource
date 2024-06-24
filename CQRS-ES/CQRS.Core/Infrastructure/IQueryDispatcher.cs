namespace CQRS.Core.Infrastructure;

using Queries;

public interface IQueryDispatcher
{
	void RegisterHandler<T>(Func<T, Task> handler) where T : BaseQuery;
	
	Task SendAsync(BaseQuery query);
}
