namespace Post.Cmd.Api.Controllers;

using Commands.Post;
using Common.DTOs;
using CQRS.Core.Infrastructure;
using DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class NewPostController(
	ILogger<NewPostController> logger,
	ICommandDispatcher commandDispatcher
) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> NewPostAsync(NewPostCommand command)
	{
		var id = Guid.NewGuid();
		try
		{
			command.Id = id;
			await commandDispatcher.SendAsync(command);
			return StatusCode(
				StatusCodes.Status201Created, 
				new NewPostResponse(id, "The post creating request completed successfully!")
			);
		}
		catch (InvalidOperationException e) // validation error
		{
			logger.LogWarning(e, "Client made a bad request!");
			return BadRequest(new BaseResponse(e.Message));
		}
		catch (Exception e) 
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new post!";
			logger.LogError(e, SAFE_ERROR_MESSAGE);
			return StatusCode(
				StatusCodes.Status500InternalServerError, 
				new NewPostResponse(id, SAFE_ERROR_MESSAGE)
			);
		}
	}
}
