using Application.Models.DTOs.Commons.Files;
using Application.Models.DTOs.Commons.Results;
using Application.Models.Enums;

namespace Application.Abstractions.Commons.Files
{
    public interface IFileService
    {
        Task<List<FileResponseDto>> UploadAsync(CreateFileDto createFileDto, FileEnums fileEnums = FileEnums.IMAGE);
        void Remove(string path);
    }
}