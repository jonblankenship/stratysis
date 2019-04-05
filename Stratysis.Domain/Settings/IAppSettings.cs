namespace Stratysis.Domain.Settings
{
    public interface IAppSettings: IDataProviderSettings
    {
        decimal DefaultCommission { get; set; }
    }
}
