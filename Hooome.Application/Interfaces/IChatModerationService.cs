namespace Hooome.Application.Interfaces;

public interface IChatModerationService
{
    string Filter(string message);
}
