namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
{
    public Guid Id { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public IntegrationBaseEvent(Guid id, DateTime createdOn)
    {
        Id = id;
        CreatedOn = createdOn;
    }

    public IntegrationBaseEvent()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }
}
