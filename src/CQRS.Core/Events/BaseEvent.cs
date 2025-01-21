namespace CQRS.Core.Events;

using Messages;

public abstract class BaseEvent(string type) : Message
{
	public int Version { get; set; }
	public string Type { get; set; } = type;
}
