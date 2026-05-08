using System;
using System.Collections.Generic;

public static class StaticEventBus
{
    private static Dictionary<Type, List<Delegate>> _subscribers = new();

    public static void Publish<T>(T eventData)
    {
        if (_subscribers.TryGetValue(typeof(T), out var list))
        {
            foreach (var handler in list)
                ((Action<T>)handler)?.Invoke(eventData);
        }
    }

    public static void Subscribe<T>(Action<T> handler)
    {
        if (!_subscribers.ContainsKey(typeof(T)))
            _subscribers.Add(typeof(T), new List<Delegate>());

        _subscribers[typeof(T)].Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        if (!_subscribers.ContainsKey(typeof(T))) return;

        _subscribers[typeof(T)].Remove(handler);
    }
}