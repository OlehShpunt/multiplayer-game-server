namespace Domain;

public class Item
{
    public int Id { get; init; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int PurchasePrice { get; set; }

    public Item(int id = -1, int price = -1, int purchasePrice = -1, string name = "not specified")
    {
        Price = price;
        Name = name;
        Id = id;
        PurchasePrice = purchasePrice;
    }
}
