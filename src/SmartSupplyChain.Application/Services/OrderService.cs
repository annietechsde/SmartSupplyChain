using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Orders;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync(int? userId = null)
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        if (userId.HasValue)
        {
            orders = orders.Where(order =>
                order.Customer != null &&
                order.Customer.UserId == userId.Value).ToList();
        }

        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            ExpectedDeliveryDate = order.ExpectedDeliveryDate,
            Items = order.OrderItems.Select(item => new CreateOrderItemDto
            {
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        });
    }

    public async Task<OrderDto?> GetByIdAsync(int id, int? userId = null)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order is null)
            return null;

        if (userId.HasValue &&
            (order.Customer == null || order.Customer.UserId != userId.Value))
            return null;

        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            ExpectedDeliveryDate = order.ExpectedDeliveryDate,
            Items = order.OrderItems.Select(item => new CreateOrderItemDto
            {
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };
    }

    public async Task<bool> CreateAsync(
        CreateOrderDto request,
        int? userId = null)
    {
        if (request.Items == null ||
            request.Items.Count == 0 ||
            request.Items.Any(item =>
                item.Quantity <= 0 || item.UnitPrice < 0))
        {
            return false;
        }

        var customer = await _unitOfWork.Customers.GetByIdAsync(
            request.CustomerId);

        if (customer is null)
            return false;

        if (userId.HasValue && customer.UserId != userId.Value)
            return false;

        var order = new Order
        {
            OrderNumber = $"ORD-{Guid.NewGuid():N}",
            CustomerId = request.CustomerId,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate
        };

        foreach (var item in request.Items)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            });
        }

        order.TotalAmount = order.OrderItems.Sum(item => item.TotalPrice);

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CreateExternalOrderAsync(
        ExternalOrderDto request)
    {
        if (request.Items == null ||
            string.IsNullOrWhiteSpace(request.ExternalOrderId) ||
            string.IsNullOrWhiteSpace(request.CustomerName) ||
            string.IsNullOrWhiteSpace(request.CustomerEmail) ||
            string.IsNullOrWhiteSpace(request.CustomerPhone) ||
            string.IsNullOrWhiteSpace(request.AddressLine1) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.State) ||
            string.IsNullOrWhiteSpace(request.PostalCode) ||
            string.IsNullOrWhiteSpace(request.Country) ||
            request.Items.Count == 0 ||
            request.Items.Any(item =>
                string.IsNullOrWhiteSpace(item.ProductName) ||
                item.Quantity <= 0 ||
                item.UnitPrice < 0))
        {
            return false;
        }

        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var externalOrderId = request.ExternalOrderId.Trim();

            var existingOrder =
                await _unitOfWork.Orders.GetByExternalOrderIdAsync(
                    externalOrderId);

            if (existingOrder is not null)
                return false;

            var email = request.CustomerEmail.Trim();

            var customer =
                await _unitOfWork.Customers.GetByEmailAsync(email);

            var address = new Address
            {
                AddressLine1 = request.AddressLine1.Trim(),
                AddressLine2 = string.IsNullOrWhiteSpace(request.AddressLine2)
                    ? null
                    : request.AddressLine2.Trim(),
                City = request.City.Trim(),
                State = request.State.Trim(),
                PostalCode = request.PostalCode.Trim(),
                Country = request.Country.Trim()
            };

            await _unitOfWork.Addresses.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();

            if (customer is null)
            {
                customer = new Customer
                {
                    Name = request.CustomerName.Trim(),
                    Email = email,
                    PhoneNumber = request.CustomerPhone.Trim(),
                    AddressId = address.Id
                };

                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();
            }

            var order = new Order
            {
                OrderNumber = $"ORD-{Guid.NewGuid():N}",
                ExternalOrderId = externalOrderId,
                CustomerId = customer.Id,
                DeliveryAddressId = address.Id,
                ExpectedDeliveryDate = request.ExpectedDeliveryDate
            };

            foreach (var item in request.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductName = item.ProductName.Trim(),
                    ProductCode = string.IsNullOrWhiteSpace(item.ProductCode)
                        ? null
                        : item.ProductCode.Trim(),
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice
                });
            }

            order.TotalAmount =
                order.OrderItems.Sum(item => item.TotalPrice);

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        });
    }

    public async Task<bool> DeleteAsync(int id, int? userId = null)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order is null)
            return false;

        if (userId.HasValue &&
            (order.Customer == null || order.Customer.UserId != userId.Value))
        {
            return false;
        }

        _unitOfWork.Orders.Delete(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}