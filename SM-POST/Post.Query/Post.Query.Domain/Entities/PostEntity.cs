namespace Post.Query.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Post")]  // This is a table in the database
public class PostEntity
{
	[Key]
	public Guid Id { get; set; }
	public string Author { get; set; } = default!;
	public string Message { get; set; } = default!;
	public DateTime DatePosted { get; set; }
	public int Likes { get; set; }
	public DateTime? DeletedAt { get; set; }
	// it is virtual because it is a navigation property (provides a way to navigate to related data)
	// Also the point is that we will use lazy loading or track changes, so we need to make it virtual
	public virtual ICollection<CommentEntity>? Comments { get; set; }   
	public Guid CreatedBy { get; set; }
	public Guid UpdatedBy { get; set; }	
}