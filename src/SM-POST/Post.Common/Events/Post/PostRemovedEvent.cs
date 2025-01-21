namespace Post.Common.Events.Post;

using CQRS.Core.Events;

/// <summary>
///     The post removed event class
/// </summary>
/// <seealso cref="BaseEvent" />
public class PostRemovedEvent() : BaseEvent(nameof(PostRemovedEvent))
{
}
