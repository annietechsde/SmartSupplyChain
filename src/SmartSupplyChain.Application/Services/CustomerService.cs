using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.DTOs.Customers;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CustomerValidator _customerValidator;

    public CustomerService(
        IUnitOfWork unitOfWork,
        CustomerValidator customerValidator)
    {
        _unitOfWork = unitOfWork;
        _customerValidator = customerValidator;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();

        return customers.Select(customer => new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            UserId = customer.UserId,
            AddressId = customer.AddressId
        });
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);

        if (customer is null)
        {
            return null;
        }

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            UserId = customer.UserId,
            AddressId = customer.AddressId
        };
    }

    public async Task<bool> CreateAsync(CreateCustomerDto request)
    {
        if (!_customerValidator.IsValid(request))
        {
            return false;
        }

        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.UserId,
            AddressId = request.AddressId
        };

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateCustomerDto request)
    {
        if (!_customerValidator.IsValid(request))
        {
            return false;
        }

        var customer = await _unitOfWork.Customers.GetByIdAsync(id);

        if (customer is null)
        {
            return false;
        }

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.PhoneNumber = request.PhoneNumber;
        customer.UserId = request.UserId;
        customer.AddressId = request.AddressId;
        customer.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Customers.Update(customer);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);

        if (customer is null)
        {
            return false;
        }

        _unitOfWork.Customers.Delete(customer);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}