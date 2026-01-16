namespace Domain;

public class Item
{
    public string Id { get; init; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int PurchasePrice { get; set; }

    public Item(string id, int price, int purchasePrice, string name)
    {
        Price = price;
        Name = name;
        Id = id;
        PurchasePrice = purchasePrice;
    }
}
