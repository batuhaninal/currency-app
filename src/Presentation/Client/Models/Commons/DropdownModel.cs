using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Models.Commons
{
    public sealed record DropdownModel
    {
        public DropdownModel()
        {

        }

        public DropdownModel(string title, string id, string name, string className, SelectList data, bool isMultiple = false, string[]? selected = null)
        {
            Title = title;
            Id = id;
            Name = name;
            Class = className;
            Data = data;
            SelectedDatas = selected;
            IsMultiple = isMultiple;
        }

        public string Title { get; init; } = null!;
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Class { get; init; } = null!;
        public SelectList Data { get; init; } = null!;
        public string[]? SelectedDatas { get; init; }
        public bool IsMultiple { get; init; }
    }
}