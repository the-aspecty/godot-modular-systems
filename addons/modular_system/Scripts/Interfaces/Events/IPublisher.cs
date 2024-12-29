/// <summary>
/// Represents a publisher that can notify subscribers with specific event data.
/// </summary>
/// <typeparam name="T">The type of event data to publish</typeparam>
public interface IPublisher<T>
{
    /// <summary>Adds a subscriber to the publisher's notification list.</summary>
    /// <returns>True if subscriber was successfully added.</returns>
    bool AddSubscriber(ISubscriber<T> subscriber);

    /// <summary>Removes a subscriber from the publisher's notification list.</summary>
    /// <returns>True if subscriber was successfully removed.</returns>
    bool RemoveSubscriber(ISubscriber<T> subscriber);

    /// <summary>Notifies all subscribers with the provided event data.</summary>
    void NotifySubscribers(T eventData);
}
