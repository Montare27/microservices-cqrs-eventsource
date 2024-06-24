namespace Post.Cmd.Api.Controllers;

using Commands.Comment;
using Common.DTOs;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class EditCommentController(
	ILogger<AddCommentController> logger,
	ICommandDispatcher commandDispatcher
) : ControllerBase
{
	[HttpPut("{id::guid}")]  
	public async Task<ActionResult> EditCommentAsync(Guid id, EditCommentCommand command)
	{
		command.Id = id;
		try
		{
			await commandDispatcher.SendAsync(command);
			return Ok(
			new BaseResponse("Edit comment request completed successfully!")
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
			new NewPostResponse(command.CommentId, SAFE_ERROR_MESSAGE)
			);
		}
	}
}
