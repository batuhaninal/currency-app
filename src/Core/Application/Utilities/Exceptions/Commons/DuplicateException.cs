namespace Application.Utilities.Exceptions.Commons
{
    public class DuplicateException : BusinessException
    {
        public DuplicateException(string fieldName, string value) : base($"{fieldName} = {value} is already taken!")
        {
        }
    }
}
