namespace Post.Common.Events.Post;

using CQRS.Core.Events;

/// <summary>
/// The post created event class
/// </summary>
/// <seealso cref="BaseEvent"/>
public class PostCreatedEvent() : BaseEvent(nameof(PostCreatedEvent))
{
	/// <summary>
	/// Gets or sets the value of the author
	/// </summary>
	public string Author { get; set; } = default!;
	/// <summary>
	/// Gets or sets the value of the message
	/// </summary>
	public string Message { get; set; } = default!;
	/// <summary>
	/// Gets or sets the value of the date posted
	/// </summary>
	public DateTime DatePosted { get; set; }
}
