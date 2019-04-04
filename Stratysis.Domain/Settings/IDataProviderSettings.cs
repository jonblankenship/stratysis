namespace Stratysis.Domain.Settings
{
    public interface IDataProviderSettings
    {
        string QuandlApiKey { get; set; }

        string QuandlFolderPath { get; set; }
    }
}
