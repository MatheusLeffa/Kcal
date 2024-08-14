namespace Kcal.App.Exceptions;

public class NotFoundException(String message) : HttpRequestException(message)
{
}