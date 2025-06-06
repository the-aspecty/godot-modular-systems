using System;
using System.Collections.Generic;
using Godot;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void RegisterService<T>(T service)
        where T : class
    {
        _services[typeof(T)] = service;
    }

    public static void RegisterServiceInferType<T>(T service, string name = null)
        where T : class
    {
        var type = typeof(T);
        _services[type] = service;
    }

    public static T GetService<T>()
        where T : class
    {
        return _services.TryGetValue(typeof(T), out var service) ? (T)service : null;
    }

    public static void Clear()
    {
        _services.Clear();
    }

    public static void RemoveService<T>()
        where T : class
    {
        _services.Remove(typeof(T));
    }

    public static void ListServices()
    {
        foreach (var service in _services)
        {
            GD.Print(service.Key);
        }
    }
}
