namespace Post.Query.Api.Controllers;

using Common.DTOs;
using CQRS.Core.Infrastructure;
using Domain.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Queries.Post;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController(
	ILogger<PostLookupController> logger,
	IQueryDispatcher<PostEntity> queryDispatcher
) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAllPostsAsync()
	{
		try
		{
			List<PostEntity> posts = await queryDispatcher.SendAsync(new FindAllPostsQuery());
			return NormalResponse(posts);
		}
		catch (Exception e)
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all posts!";
			return ErrorResponse(e, SAFE_ERROR_MESSAGE);
		}
	}

	[HttpGet("byId/{id:guid}")]
	public async Task<ActionResult> GetPostByIdAsync(Guid id)
	{
		try
		{
			List<PostEntity> posts = await queryDispatcher.SendAsync(new FindPostByIdQuery{Id = id});
			return NormalResponse(posts);
		}
		catch (Exception e)
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve post by id!";
			return ErrorResponse(e, SAFE_ERROR_MESSAGE);
		}
	}

	[HttpGet("byAuthor/{author}")]
	public async Task<ActionResult> GetPostByAuthorAsync(string author)
	{
		try
		{
			List<PostEntity> posts = await queryDispatcher.SendAsync(new FindPostsByAuthorQuery{Author = author});
			return NormalResponse(posts);
		}
		catch (Exception e)
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts by author!";
			return ErrorResponse(e, SAFE_ERROR_MESSAGE);
		}
	}

	[HttpGet("withComments")]
	public async Task<ActionResult> GetPostsWithCommentsAsync()
	{
		try
		{
			List<PostEntity> posts = await queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());
			return NormalResponse(posts);
		}
		catch (Exception e)
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts with comments!";
			return ErrorResponse(e, SAFE_ERROR_MESSAGE);
		}
	}

	[HttpGet("withLikes")]
	public async Task<ActionResult> GetPostsWithLikesAsync()
	{
		try
		{
			List<PostEntity> posts = await queryDispatcher.SendAsync(new FindPostsWithLikesQuery());
			return NormalResponse(posts);
		}
		catch (Exception e)
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts with likes!";
			return ErrorResponse(e, SAFE_ERROR_MESSAGE);
		}
	}

	private ActionResult NormalResponse(List<PostEntity> posts)
	{
		if (posts.Count == 0)
		{
			return NoContent();// 204
		}

		int count = posts.Count;
		return Ok(new PostLookupResponse(
		posts,
		$"Successfully returned {count} post{(count > 1 ? "s" : string.Empty)}")
		);
	}

	private ActionResult ErrorResponse(Exception e, string message)
	{
		logger.LogError(e, message);
		return BadRequest(
		new BaseResponse(message)
		);
	}
}
