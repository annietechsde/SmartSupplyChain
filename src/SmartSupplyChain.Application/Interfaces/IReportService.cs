namespace SmartSupplyChain.Application.Interfaces;

public interface IReportService
{
    Task<object> GetOrderReportAsync();

    Task<object> GetShipmentReportAsync();

    Task<object> GetPaymentReportAsync();

    Task<object> GetDeliveryReportAsync();
}