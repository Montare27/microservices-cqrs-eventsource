namespace Post.Query.Api.DTOs;

using Common.DTOs;
using Domain.Entities;

public record PostLookupResponse(
	List<PostEntity> Posts,
	string Message
) : BaseResponse(Message);
