namespace Api.Services;

public interface IShipmentService
{
    void Ship(IAddressInfo addressInfo, IEnumerable<CartItem> items);
}

