namespace Post.Cmd.Api.Events.Comment;

using CQRS.Core.Events;

/// <summary>
/// The comment updated event class
/// </summary>
/// <seealso cref="BaseEvent"/>
public class CommentUpdatedEvent() : BaseEvent(nameof(CommentUpdatedEvent))
{
	/// <summary>
	/// Gets or sets the value of the commend id
	/// </summary>
	public Guid CommendId { get; set; }
	/// <summary>
	/// Gets or sets the value of the comment
	/// </summary>
	public string Comment { get; set; } = string.Empty;
	/// <summary>
	/// Gets or sets the value of the username
	/// </summary>
	public string Username { get; set; } = string.Empty;
	/// <summary>
	/// Gets or sets the value of the edit date
	/// </summary>
	public DateTime EditDate { get; set; }
}
