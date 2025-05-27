namespace Client.Models.Commons
{
    public sealed record PopupModel
    {
        public PopupModel()
        {
            
        }

        public PopupModel(string title, string id = "base-modal", PopupType popupType = PopupType.Create)
        {
            Title = title;
            Id = id;
            PopupType = popupType;
        }

        public string Title { get; init; } = null!;
        public string Id { get; init; }
        public PopupType PopupType { get; init; }
    }

    public enum PopupType
    {
        Create = 0,
        Update = 1
    }
}