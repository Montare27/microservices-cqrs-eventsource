namespace Post.Query.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Comment")]
public class CommentEntity
{
	[Key]
	public Guid CommentId { get; set; }
	public Guid PostId { get; set; }
	public string Username { get; set; } = default!;
	public string Comment { get; set; } = default!;
	public DateTime CommentDate { get; set; }
	public bool Edited { get; set; }
	[JsonIgnore]
	public virtual PostEntity Post { get; set; } = default!;
}
