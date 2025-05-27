using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Models.Commons
{
    public sealed record DropdownModel
    {
        public DropdownModel()
        {

        }

        public DropdownModel(string title, string id, string name, string className, SelectList data, string selected = "0")
        {
            Title = title;
            Id = id;
            Name = name;
            Class = className;
            Data = data;
            SelectedData = selected;
        }

        public string Title { get; init; } = null!;
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Class { get; init; } = null!;
        public SelectList Data { get; init; } = null!;
        public string SelectedData { get; init; }
    }
}