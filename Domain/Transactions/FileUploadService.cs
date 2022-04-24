using Domain.Transactions;

namespace Domain
{
    public class FileUploadService
    {
        public static List<string> ProcessFile(StreamReader streamReader, string fileFormat) 
        { 
            IFileProcessor fileProcessor;

            if(fileFormat.ToLower() == "csv")
            {
                fileProcessor = CsvFileProcessor.Create(streamReader);
            }
            else if (fileFormat.ToLower() == "xml")
            {
                fileProcessor = XmlFileProcessor.Create(streamReader);
            }
            else
            {
                throw new Exception("File format not found");
            }

            fileProcessor.ReadAndValidateFile();

            if(fileProcessor.ErrorMessages.Count == 0)
            {
                fileProcessor.StoreTransactions();
            }

            return fileProcessor.ErrorMessages;
        }


    }
}