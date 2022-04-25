using Domain.Interfaces;
using Domain.Processors;

namespace Domain
{
    public class FileUploadService
    {
        public static List<string> ProcessFile(StreamReader streamReader, string fileFormat, ITransactionRepository transactionRepository) 
        { 
            IFileProcessor fileProcessor;

            if(fileFormat.ToLower() == ".csv")
            {
                fileProcessor = new CsvFileProcessor(streamReader, transactionRepository);
            }
            else if (fileFormat.ToLower() == ".xml")
            {
                fileProcessor = new XmlFileProcessor(streamReader, transactionRepository);
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