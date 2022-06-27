namespace StoredProcedureApi.Repository
{
    public class UploadModel
    {
      public string? FileName { get; set; }
      public FileTypes FileType { get; set; }
    }

    public enum FileTypes
    {
        Image = 1,
        Document,
    }
}
