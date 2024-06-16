namespace Post.Common.Events.Post;

using CQRS.Core.Events;

/// <summary>
/// The post liked event class
/// </summary>
/// <seealso cref="BaseEvent"/>
public class PostLikedEvent() : BaseEvent(nameof(PostLikedEvent));
