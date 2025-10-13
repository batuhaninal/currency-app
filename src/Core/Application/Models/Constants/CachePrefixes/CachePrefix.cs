namespace Application.Models.Constants.CachePrefixes
{
    public static class CachePrefix
    {
        public const string CategoryPrefix = "categories";
        public const string CurrencyPrefix = "currencies";
        public const string UserPrefix = "users";
        public static string CreatePrefix(string prefix, string operationName) => string.IsNullOrEmpty(operationName) ? $"{prefix}.noOperationName" : $"{prefix}.{operationName}";
        public static string CreatePaginationPrefix(string prefix, string operationName, int pageIndex, int pageSize) => $"{prefix}.{operationName}.pageIndex={pageIndex}.pageSize={pageSize}";
        public static string GetAllWithPagination(string prefix, int pageIndex, int pageSize) => $"{prefix}.all.pageIndex={pageIndex}.pageSize={pageSize}";
        public static string CreateByParameter(string prefix, string operationName, string parameterName, string parameterValue) => $"{prefix}.{operationName}.{parameterName}={parameterValue}";
    }
}
