namespace Application.Utilities.Exceptions.Commons
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
