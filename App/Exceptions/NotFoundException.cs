namespace Kcal.App.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}