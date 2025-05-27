namespace Application.Models.Constants.CachePrefixes
{
    public static class CachePrefix
    {
        public static class Categories
        {
            public const string Prefix = "categories";
            public const string All = $"{Prefix}.all";
            public static string CreatePrefix(string operationName) => string.IsNullOrEmpty(operationName) ? $"{Prefix}.noOperationName" : $"{Prefix}.{operationName}";
            public static string CreatePaginationPrefix(string operationName, int pageIndex, int pageSize) => $"{Prefix}.{operationName}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetAllWithPagination(int pageIndex, int pageSize) => $"{All}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetByParameter(string parameterName, string parameterValue) => $"{Prefix}.getbyid.{parameterName}={parameterValue}";
        }

        public static class Articles
        {
            public const string Prefix = "articles";
            public const string All = $"{Prefix}.all";
            public static string CreatePrefix(string operationName) => string.IsNullOrEmpty(operationName) ? $"{Prefix}.noOperationName" : $"{Prefix}.{operationName}";
            public static string CreatePaginationPrefix(string operationName, int pageIndex, int pageSize) => $"{Prefix}.{operationName}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetAllWithPagination(int pageIndex, int pageSize) => $"{All}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetByParameter(string parameterName, string parameterValue) => $"{Prefix}.getbyid.{parameterName}={parameterValue}";
        }

        public static class Writer
        {
            public const string Prefix = "writers";
            public const string All = $"{Prefix}.all";
            public static string CreatePrefix(string operationName) => string.IsNullOrEmpty(operationName) ? $"{Prefix}.noOperationName" : $"{Prefix}.{operationName}";
            public static string CreatePaginationPrefix(string operationName, int pageIndex, int pageSize) => $"{Prefix}.{operationName}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetAllWithPagination(int pageIndex, int pageSize) => $"{All}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetByParameter(string parameterName, string parameterValue) => $"{Prefix}.getbyid.{parameterName}={parameterValue}";
        }

        public static class User
        {
            public const string Prefix = "users";
            public const string All = $"{Prefix}.all";
            public static string CreatePrefix(string operationName) => string.IsNullOrEmpty(operationName) ? $"{Prefix}.noOperationName" : $"{Prefix}.{operationName}";
            public static string CreatePaginationPrefix(string operationName, int pageIndex, int pageSize) => $"{Prefix}.{operationName}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetAllWithPagination(int pageIndex, int pageSize) => $"{All}.pageIndex={pageIndex}.pageSize={pageSize}";
            public static string GetByParameter(string parameterName, string parameterValue) => $"{Prefix}.getbyid.{parameterName}={parameterValue}";
        }
    }
}
