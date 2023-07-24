namespace Api.Services;

public interface IPaymentService
{
    bool Charge(double total, ICard card);
}