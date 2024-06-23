namespace Post.Query.Infrastructure.Converters;

using Common.Events.Comment;
using Common.Events.Post;
using CQRS.Core.Events;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonException=ThirdParty.Json.LitJson.JsonException;

/// <summary>
/// Polymorphic JsonConverter for BaseEvent and its derived classes.
/// </summary>
public class EventJsonConverter : JsonConverter<BaseEvent>
{
	public override bool CanConvert(Type typeToConvert) => 
		// checks if an argument is assignable to a variable of type BaseEvent
		typeToConvert.IsAssignableFrom(typeof(BaseEvent));  

	public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// attempts to parse JsonDocument from the reader
		if (!JsonDocument.TryParseValue(ref reader, out var doc))
			throw new JsonException($"Failed to parse {nameof(JsonDocument)}!");
		
		// tries to get the Type property from the root element
		if(!doc.RootElement.TryGetProperty("Type", out var type))
			// Discriminator property acts as a tag to identify the type of the event
			throw new JsonException($"Could not detect the Type discriminator property!");
		
		var typeDiscriminator = type.GetString();
		var jsonString = doc.RootElement.GetRawText();

		return typeDiscriminator switch{
			nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(jsonString, options),
			nameof(MessageUpdatedEvent) => JsonSerializer.Deserialize<MessageUpdatedEvent>(jsonString, options),
			nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostLikedEvent>(jsonString, options),
			nameof(CommentAddedEvent) => JsonSerializer.Deserialize<CommentAddedEvent>(jsonString, options),
			nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(jsonString, options),
			nameof(CommentRemovedEvent) => JsonSerializer.Deserialize<CommentRemovedEvent>(jsonString, options),
			nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostRemovedEvent>(jsonString, options),
			_ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
		};
	}

	public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}
}
