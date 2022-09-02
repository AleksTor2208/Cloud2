namespace CloudAppWebApi.Model
{
    public interface IFile
    {
        string Id { get; }
        bool IsChecked { get; }
        string Title { get; }
        string Extension { get; }
        string FileSize { get; }
        string ModifiedDate { get; }
        //string Content { get; }
    }
}
