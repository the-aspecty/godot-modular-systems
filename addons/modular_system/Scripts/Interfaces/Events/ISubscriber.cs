public interface ISubscriber
{
    void OnNotify();
}

/// <summary>
/// Represents a subscriber that can receive notifications with specific event data.
/// </summary>
/// <typeparam name="T">The type of event data to receive</typeparam>
public interface ISubscriber<T>
{
    /// <summary>Called when the publisher sends a notification.</summary>
    /// <param name="eventData">The event data from the publisher</param>
    void OnNotify(T eventData);
}
