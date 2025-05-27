namespace Application.Utilities.Helpers
{
    public static class CacheHelpers
    {
        public static bool WillCache(int pageIndex, int pageSize) =>
            pageIndex <= 5 &&
                (pageSize == 10 ||
                pageSize == 20 ||
                pageSize == 25 ||
                pageSize == 50);
    }
}
