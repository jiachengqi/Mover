namespace frontend.Models;

public class RouteOptimizationModel
{
    public List<AddressItem> Addresses { get; set; } = new List<AddressItem>
    {
        new AddressItem(),
        new AddressItem()
    };
}