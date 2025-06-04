namespace Application.Abstractions.Commons.Logger
{
    public interface ILoggerService<TClass>
        where TClass : class
    {
        void Info(string message);
        void Error(string message);
    }
}