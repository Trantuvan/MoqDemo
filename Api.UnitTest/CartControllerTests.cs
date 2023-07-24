namespace Api.UnitTest;

public class CartControllerTests
{
    private CartController controller;
    private Mock<IPaymentService> paymentServiceMock;
    private Mock<ICartService> cartServiceMock;

    private Mock<IShipmentService> shippingServiceMock;
    private Mock<ICard> cardMock;
    private Mock<IAddressInfo> addressInfoMock;
    private List<CartItem> items;

    [SetUp]
    public void Setup()
    {
        paymentServiceMock = new Mock<IPaymentService>();
        cartServiceMock = new Mock<ICartService>();
        shippingServiceMock = new Mock<IShipmentService>();

        // Arrange       
        cardMock = new Mock<ICard>();
        addressInfoMock = new Mock<IAddressInfo>();

        var cartItemMock = new Mock<CartItem>();

        items = new List<CartItem> { cartItemMock.Object };

        cartServiceMock.Setup(c => c.Items()).Returns(items);

        controller = new CartController(cartServiceMock.Object,
                                        paymentServiceMock.Object,
                                        shippingServiceMock.Object);
    }

    [Test]
    public void ShouldReturnCharged()
    {
        //Arrange
        const string statusExpected = "charged";
        paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(true);

        //Act
        string statusActual = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

        //Assert
        shippingServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items), Times.Once());
        Assert.That(statusActual, Is.EqualTo(statusExpected));
    }

    [Test]
    public void ShouldReturnNotCharged()
    {
        const string statusExpected = "not charged";
        paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object))
            .Returns(false);

        string statusActual = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

        shippingServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items), Times.Never());
        Assert.That(statusActual, Is.EqualTo(statusExpected));
    }
}