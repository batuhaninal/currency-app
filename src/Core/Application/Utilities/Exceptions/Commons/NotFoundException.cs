namespace Application.Utilities.Exceptions.Commons
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string entityName) : base($"{entityName} not found!")
        {
        }
    }
}
