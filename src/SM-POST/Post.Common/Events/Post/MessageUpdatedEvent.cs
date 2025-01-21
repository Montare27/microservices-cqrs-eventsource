namespace Post.Common.Events.Post;

using CQRS.Core.Events;

/// <summary>
///     The message updated event class
/// </summary>
/// <seealso cref="BaseEvent" />
public class MessageUpdatedEvent() : BaseEvent(nameof(MessageUpdatedEvent))
{
	/// <summary>
	///     Gets or sets the value of the message
	/// </summary>
	public string Message { get; set; } = default!;
}
