﻿namespace DriverRater.Api.Extensions;

using System;
using System.Reflection;

public static class ObjectExtensions
{
    public static bool HasAttribute<T>(this object obj) where T : Attribute => obj.GetType().GetTypeInfo().GetCustomAttribute<T>() != null;

    public static T If<T>(this T obj, bool predicate, Action<T> configureAction)
    {
        if (predicate) configureAction(obj);
        return obj;
    }
}