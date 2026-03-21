namespace Hooome.Application.Common.Exceptions;

public class AlreadyExistException(string name):
    Exception($"Entity \"{name} already exist");