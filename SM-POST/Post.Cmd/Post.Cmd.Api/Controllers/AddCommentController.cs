namespace Post.Cmd.Api.Controllers;

using Commands.Comment;
using Common.DTOs;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class AddCommentController(
	ILogger<AddCommentController> logger,
	ICommandDispatcher commandDispatcher
) : ControllerBase
{
	[HttpPut("{id::guid}")]
	public async Task<ActionResult> AddComment(Guid id, AddCommentCommand command)
	{
		command.Id = id;
		try
		{
			await commandDispatcher.SendAsync(command);
			return Ok(
				new BaseResponse("Add comment request completed successfully!")
			);
		}
		catch (InvalidOperationException e)// validation error
		{
			logger.LogWarning(e, "Client made a bad request!");
			return BadRequest(
			new BaseResponse(e.Message)
			);
		}
		catch (AggregateNotFoundException e)
		{
			logger.LogWarning(e, "Could not retrieve aggregate, client passed an incorrect post ID targeting the aggregate!");
			return BadRequest(
			new BaseResponse(e.Message)
			);
		}
		catch (Exception e) 
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to add comment!";
			logger.LogError(e, SAFE_ERROR_MESSAGE);
			return BadRequest( 
			new NewPostResponse(command.Id, SAFE_ERROR_MESSAGE)
			);
		}
	}
}
