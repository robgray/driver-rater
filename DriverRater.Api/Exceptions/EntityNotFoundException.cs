namespace DriverRater.Api.Exceptions;

using System;

public class EntityNotFoundException(string message) : Exception(message)
{
}