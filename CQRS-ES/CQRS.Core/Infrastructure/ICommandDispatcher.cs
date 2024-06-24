namespace CQRS.Core.Infrastructure;

using Commands;

/**
 * <summary>
 *     Command Dispatcher interface
 *     In OOP it is basically an interface for Mediator
 *     Command Dispatcher role in CQRS architecture
 * </summary>
 */
public interface ICommandDispatcher
{
	/**
	 * <summary>
	 *     Registers the handler using the specified handler
	 * </summary>
	 * <typeparam name="T">The </typeparam>
	 * <param name="handler">The handler</param>
	 */
	void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;

	/// <summary>
	///     Sends the command
	/// </summary>
	/// <param name="command">The command</param>
	Task SendAsync(BaseCommand command);
}
