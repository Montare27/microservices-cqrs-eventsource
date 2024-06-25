namespace Post.Cmd.Api.Controllers;

using Commands;
using Commands.Comment;
using Common.DTOs;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class RestoreReadDbController(
	ILogger<RestoreReadDbController> logger,
	ICommandDispatcher commandDispatcher
) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> RestoreReadDbAsync()
	{
		try
		{
			await commandDispatcher.SendAsync(new RestoreReadDbCommand());
			return Ok(
				new BaseResponse("Restore read db request completed successfully!")
			);
		}
		catch (InvalidOperationException e)// validation error
		{
			logger.LogWarning(e, "Client made a bad request!");
			return BadRequest(
			new BaseResponse(e.Message)
			);
		}
		catch (Exception e)
		{
			const string SAFE_ERROR_MESSAGE = "Error while processing request to restore read db!";
			logger.LogError(e, SAFE_ERROR_MESSAGE);
			return BadRequest(
			new BaseResponse(SAFE_ERROR_MESSAGE)
			);
		}
	}
}
