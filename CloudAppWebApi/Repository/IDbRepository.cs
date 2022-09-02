using CloudAppWebApi.Model;

namespace CloudAppWebApi.Repository
{
    public interface IDbRepository
    {
        Task<IEnumerable<IFile>> GetFiles();
        IFile? GetDetails(string Id);
        Task<IFile> DownloadFile(string Id);
        Task<IFile> Delete(string Id);

        Task UploadFileAsync(IFile file);
    }
}