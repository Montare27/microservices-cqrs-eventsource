namespace Post.Cmd.Api.DTOs;

using Common.DTOs;

public record NewPostResponse(Guid Id, string Message) : BaseResponse(Message);
