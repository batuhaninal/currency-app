namespace Application.Models.Constants.Settings
{
    public class ServiceApiSetting
    {
        public ServiceApi TradingView { get; set; }
        public ServiceApi DovizCom { get; set; }
        public ServiceApi PHI3Mini { get; set; }
    }

    public class ServiceApi
    {
        public string Path { get; set; } = null!;
    }
}