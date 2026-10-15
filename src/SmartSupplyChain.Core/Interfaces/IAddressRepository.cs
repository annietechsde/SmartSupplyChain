
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interfaces;

public interface IAddressRepository
{
    Task AddAsync(Address address);
}
