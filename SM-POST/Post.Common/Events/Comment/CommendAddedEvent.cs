namespace Post.Common.Events.Comment;

using CQRS.Core.Events;

/// <summary>
/// The commend added event class
/// </summary>
/// <seealso cref="BaseEvent"/>
public class CommendAddedEvent() : BaseEvent(nameof(CommendAddedEvent))
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
	/// Gets or sets the value of the comment date
	/// </summary>
	public DateTime CommentDate { get; set; }
}
