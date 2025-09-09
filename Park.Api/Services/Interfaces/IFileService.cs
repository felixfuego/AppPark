using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileUploadResponseDto> UploadFileAsync(UploadFileDto uploadDto, int? userId = null);
        Task<FileDto?> GetFileAsync(int fileId);
        Task<FileDto?> GetFileByPathAsync(string filePath);
        Task<byte[]?> DownloadFileAsync(int fileId);
        Task<byte[]?> DownloadFileByPathAsync(string filePath);
        Task<bool> DeleteFileAsync(int fileId);
        Task<bool> DeleteFileByPathAsync(string filePath);
        Task<IEnumerable<FileDto>> GetFilesAsync(FileSearchDto searchDto);
        Task<PagedResultDto<FileDto>> SearchFilesAsync(FileSearchDto searchDto);
        Task<FileStatsDto> GetFileStatsAsync();
        Task<FileInfoDto> GetFileInfoAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);
        Task<string> GenerateUniqueFileNameAsync(string originalFileName);
        Task<string> GetFileExtensionAsync(string fileName);
        Task<string> GetMimeTypeAsync(string fileName);
        Task<bool> IsValidFileTypeAsync(string fileName, TipoArchivo tipo);
        Task<long> GetFileSizeAsync(string filePath);
        Task<bool> CleanTempFilesAsync();
        Task<bool> CleanOldFilesAsync(int diasRetencion = 30);
    }
}
