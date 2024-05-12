namespace Post.Cmd.Domain.Aggregates;

using Common.Events.Comment;
using Common.Events.Post;
using CQRS.Core.Domain;

public class PostAggregate : AggregateRoot
{
	private bool _active;

	private string _author;

	private readonly Dictionary<Guid, Tuple<string, string>> _comments = [];
	
	public bool Active { get; set; }

	public PostAggregate() {}

	/// <summary>
	/// Raises event with new PostCreatedEvent. When the uncommitted change was occured, runs <see cref="Apply"/> that assigns members of this class
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
	/// Applies an event, assigns new values to this class
	/// </summary>
	/// <param name="event"></param>
	public void Apply(PostCreatedEvent @event)
	{
		_id = @event.Id;
		_active = true;
		_author = @event.Author;
	}

	/// <summary>
	/// Edits message
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
	/// Applies updating
	/// </summary>
	/// <param name="event"></param>
	public void Apply(MessageUpdatedEvent @event)
	{
		_id = @event.Id;
	}

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
	/// Applies liking the post
	/// </summary>
	/// <param name="event"></param>
	public void Apply(PostLikedEvent @event)
	{
		_id = @event.Id;
	}

	/// <summary>
	/// Adds a comment
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
		
		RaiseEvent(new CommendAddedEvent{
			Id = _id,
			Comment = comment,
			CommendId = Guid.NewGuid(),
			Username = username,
			CommentDate = DateTime.Now
		});
	}

	
	public void Apply(CommendAddedEvent @event)
	{
		_id = @event.Id;
		_comments.Add(@event.CommendId, new Tuple<string, string>(@event.Comment, @event.Username));
	}
}
