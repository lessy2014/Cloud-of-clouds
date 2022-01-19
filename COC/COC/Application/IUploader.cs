namespace COC.Application
{
    public interface IUploader
    {
        public void UploadFile(string pathToUpload, string fileToUploadPath, Account account);
    }
}