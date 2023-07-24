using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IPaymentService _paymentService;
    private readonly IShipmentService _shipmentService;

    public CartController(
        ICartService cartService,
        IPaymentService paymentService,
        IShipmentService shipmentService)
    {
        _cartService = cartService;
        _paymentService = paymentService;
        _shipmentService = shipmentService;
    }

    #region [Post]
    [HttpPost]
    public string CheckOut(ICard card, IAddressInfo addressInfo)
    {
        bool result = _paymentService.Charge(_cartService.Total(), card);

        if (result == true)
        {
            _shipmentService.Ship(addressInfo, _cartService.Items());
            return "charged";
        }

        return "not charged";
    }
    #endregion
}