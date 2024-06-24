namespace Post.Cmd.Domain.Aggregates;

using Common.Events.Comment;
using Common.Events.Post;
using CQRS.Core.Domain;

/// <summary>
///     AggregateRoot implementation. An instance of PostAggregate represents single social media post
/// </summary>
public class PostAggregate : AggregateRoot
{

	/// <summary>
	///     The comments
	/// </summary>
	private readonly Dictionary<Guid, Tuple<string, string>> _comments =[];

	/// <summary>
	///     The active
	/// </summary>
	private bool _active;

	/// <summary>
	///     The author
	/// </summary>
	private string _author;

	/// <summary>
	///     Initializes a new instance of the <see cref="PostAggregate" /> class
	/// </summary>
	public PostAggregate() {}

	/// <summary>
	///     Raises event with new PostCreatedEvent. When the uncommitted change was occured, runs <see cref="Apply" /> that
	///     assigns members of this class
	/// </summary>
	/// <param name="id"></param>
	/// <param name="author"></param>
	/// <param name="message"></param>
	public PostAggregate(Guid id, string author, string message)
	{
		RaiseEvent(new PostCreatedEvent{
			Id = id,
			Author = author,
			Message = message,
			DatePosted = DateTime.Now
		});
	}

	/// <summary>
	///     Gets or sets the value of the active
	/// </summary>
	public bool Active { get; set; }

	/// <summary>
	///     Applies an event, assigns new values to this class
	/// </summary>
	/// <param name="event"></param>
	public void Apply(PostCreatedEvent @event)
	{
		_id = @event.Id;
		_active = true;
		_author = @event.Author;
	}

	/// <summary>
	///     Edits message
	/// </summary>
	/// <param name="message"></param>
	public void EditMessage(string message)
	{
		if (!_active)
		{
			throw new InvalidOperationException("You cannot edit the message of an inactive post!");
		}

		if (string.IsNullOrWhiteSpace(message))
		{
			throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}!");
		}

		RaiseEvent(new MessageUpdatedEvent{
			Id = _id,
			Message = message
		});
	}

	/// <summary>
	///     Applies updating
	/// </summary>
	/// <param name="event"></param>
	public void Apply(MessageUpdatedEvent @event)
	{
		_id = @event.Id;
	}

	/// <summary>
	///     Likes the post
	/// </summary>
	/// <exception cref="InvalidOperationException">You cannot like an inactive post!</exception>
	public void LikePost()
	{
		if (!_active)
		{
			throw new InvalidOperationException("You cannot like an inactive post!");
		}

		RaiseEvent(new PostLikedEvent{
			Id = _id
		});
	}

	/// <summary>
	///     Applies liking the post
	/// </summary>
	/// <param name="event"></param>
	public void Apply(PostLikedEvent @event)
	{
		_id = @event.Id;
	}

	/// <summary>
	///     Adds a comment
	/// </summary>
	/// <param name="comment"></param>
	/// <param name="username"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public void AddComment(string comment, string username)
	{
		if (!_active)
		{
			throw new InvalidOperationException("You cannot add a comment to an inactive post!");
		}

		if (string.IsNullOrWhiteSpace(comment))
		{
			throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");
		}

		RaiseEvent(new CommentAddedEvent{
			Id = _id,
			Comment = comment,
			CommendId = Guid.NewGuid(),
			Username = username,
			CommentDate = DateTime.Now
		});
	}

	/// <summary>
	///     Applies CommentAddedEvent
	/// </summary>
	/// <param name="event"></param>
	public void Apply(CommentAddedEvent @event)
	{
		_id = @event.Id;
		_comments.Add(@event.CommendId, new Tuple<string, string>(@event.Comment, @event.Username));
	}

	/// <summary>
	///     Edits the comment using the specified comment id
	/// </summary>
	/// <param name="commentId">The comment id</param>
	/// <param name="comment">The comment</param>
	/// <param name="username">The username</param>
	/// <exception cref="InvalidOperationException">
	///     The value of {nameof(comment)} cannot be null or empty. Please provide a
	///     valid {nameof(comment)}!
	/// </exception>
	/// <exception cref="InvalidOperationException">You cannot edit a comment to an inactive post!</exception>
	public void EditComment(Guid commentId, string comment, string username)
	{
		if (!_active)
		{
			throw new InvalidOperationException("You cannot edit a comment to an inactive post!");
		}

		if (!_comments[commentId].Item2.Equals(username))
		{
			throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
		}

		if (string.IsNullOrWhiteSpace(comment))
		{
			throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");
		}

		RaiseEvent(new CommentUpdatedEvent{
			Id = _id,
			CommendId = commentId,
			Comment = comment,
			Username = username,
			EditDate = DateTime.Now
		});
	}

	/// <summary>
	///     Applies the event
	/// </summary>
	/// <param name="@event">The event</param>
	/// <param name="event"></param>
	public void Apply(CommentUpdatedEvent @event)
	{
		_id = @event.Id;
		_comments[@event.CommendId] = new Tuple<string, string>(@event.Comment, @event.Username);
	}

	/// <summary>
	///     Edits the comment using the specified comment id
	/// </summary>
	/// <param name="commentId">The comment id</param>
	/// <param name="comment">The comment</param>
	/// <param name="username">The username</param>
	/// <exception cref="InvalidOperationException">
	///     The value of {nameof(comment)} cannot be null or empty. Please provide a
	///     valid {nameof(comment)}!
	/// </exception>
	/// <exception cref="InvalidOperationException">You cannot edit a comment to an inactive post!</exception>
	public void RemoveComment(Guid commentId, string username)
	{
		if (!_active)
		{
			throw new InvalidOperationException("You cannot edit a comment to an inactive post!");
		}

		if (!_comments[commentId].Item2.Equals(username))
		{
			throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
		}

		RaiseEvent(new CommentRemovedEvent{
			Id = _id,
			CommendId = commentId
		});
	}

	/// <summary>
	///     Applies the event
	/// </summary>
	/// <param name="@event">The event</param>
	/// <param name="event"></param>
	public void Apply(CommentRemovedEvent @event)
	{
		_id = @event.Id;
		_comments.Remove(@event.CommendId);
	}

	/// <summary>
	///     Deletes the post using the specified username
	/// </summary>
	/// <param name="username">The username</param>
	/// <exception cref="InvalidOperationException">You are not allowed to delete a post that was made by somebody else!</exception>
	/// <exception cref="InvalidOperationException">You cannot edit a comment to an inactive post!</exception>
	public void DeletePost(string username)
	{
		if (!_active)
		{
			throw new InvalidOperationException("You cannot edit a comment to an inactive post!");
		}

		if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
		{
			throw new InvalidOperationException("You are not allowed to delete a post that was made by somebody else!");
		}

		RaiseEvent(new PostRemovedEvent{
			Id = _id
		});
	}


	/// <summary>
	///     Applies the event
	/// </summary>
	/// <param name="@event">The event</param>
	/// <param name="event"></param>
	public void Apply(PostRemovedEvent @event)
	{
		_id = @event.Id;
		_active = false;
	}
}
