namespace Domain;

public class Item
{
    public int Id { set; get; }
    public string Name { set; get; }
    public int Price { set; get; }
    public int PurchasePrice { set; get; }

    public Item(int id = -1, int price = -1, int purchasePrice = -1, string name = "not specified")
    {
        Price = price;
        Name = name;
        Id = id;
        PurchasePrice = purchasePrice;
    }

    public int GetPrice()
    {
        return Price;
    }

    public string GetName()
    {
        return Name;
    }

    public void SetPrice(int price)
    {
        Price = price;
    }

    public void SetName(string name)
    {
        Name = name;
    }
}
