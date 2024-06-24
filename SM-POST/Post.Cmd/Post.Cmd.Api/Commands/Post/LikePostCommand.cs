namespace Post.Cmd.Api.Commands.Post;

using CQRS.Core.Commands;

/// <summary>
///     The like post command class
/// </summary>
/// <seealso cref="BaseCommand" />
public class LikePostCommand : BaseCommand
{
	public LikePostCommand(Guid id) => Id = id;
}
