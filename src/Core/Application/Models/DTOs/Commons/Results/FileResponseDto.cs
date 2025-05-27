namespace Application.Models.DTOs.Commons.Results
{
    public class FileResponseDto : ResultDto
    {
        public FileResponseDto(string name, string extension, string? dbPath, string? staticPath, float size, int statusCode, bool success, string message) : base(statusCode, success, message)
        {
            Name = name;
            Extension = extension;
            DbPath = dbPath;
            StaticPath = staticPath;
            Size = size;
        }
        public FileResponseDto(string name, string extension, string? dbPath, string? staticPath, float size, int statusCode, bool success) : 
            base(statusCode, success)
        {
            Name = name;
            Extension = extension;
            DbPath = dbPath;
            StaticPath = staticPath;
            Size = size;
        }

        

        public string Name { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string? DbPath { get; set; }
        public string? StaticPath { get; set; }
        public float Size { get; set; }
    }
}