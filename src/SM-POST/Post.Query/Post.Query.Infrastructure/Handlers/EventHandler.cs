﻿namespace Post.Query.Infrastructure.Handlers;

using Common.Events.Comment;
using Common.Events.Post;
using Domain.Entities;
using Domain.Repositories;

public class EventHandler(
	IPostRepository postRepository,
	ICommentRepository commentRepository
) : IEventHandler
{
	public Task On(PostCreatedEvent @event)
	{
		return postRepository.CreateAsync(new PostEntity{
			Id = @event.Id,
			Author = @event.Author,
			DatePosted = @event.DatePosted,
			Message = @event.Message
		});
	}


	public async Task On(MessageUpdatedEvent @event)
	{
		var post = await postRepository.GetByIdAsync(@event.Id);
		if (post == null)
		{
			return;
		}

		post.Message = @event.Message;
		await postRepository.UpdateAsync(post);
	}


	public async Task On(PostLikedEvent @event)
	{
		var post = await postRepository.GetByIdAsync(@event.Id);
		if (post == null)
		{
			return;
		}

		post.Likes++;
		await postRepository.UpdateAsync(post);
	}

	public Task On(CommentAddedEvent @event)
	{
		return commentRepository.CreateAsync(new CommentEntity{
			PostId = @event.Id,
			CommentId = @event.CommendId,
			CommentDate = @event.CommentDate,
			Username = @event.Username,
			Comment = @event.Comment,
			Edited = false
		});
	}

	public async Task On(CommentUpdatedEvent @event)
	{
		var comment = await commentRepository.GetByIdAsync(@event.CommendId);
		if (comment == null)
		{
			return;
		}

		comment.Comment = @event.Comment;
		comment.CommentDate = @event.EditDate;
		comment.Edited = true;

		await commentRepository.UpdateAsync(comment);
	}

	public Task On(CommentRemovedEvent @event)
	{
		Console.WriteLine($"Remove comment: {@event.CommendId}");
		return commentRepository.DeleteAsync(@event.CommendId);
	}

	public Task On(PostRemovedEvent @event)
	{
		return postRepository.DeleteAsync(@event.Id);
	}
}
