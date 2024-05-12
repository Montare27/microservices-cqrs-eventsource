namespace Post.Common.Events.Comment;

using CQRS.Core.Events;

/// <summary>
/// The comment removed event class
/// </summary>
/// <seealso cref="BaseEvent"/>
public class CommentRemovedEvent() : BaseEvent(nameof(CommentRemovedEvent))
{
	/// <summary>
	/// Gets or sets the value of the commend id
	/// </summary>
	public Guid CommendId { get; set; }
}
