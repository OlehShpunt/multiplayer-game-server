namespace Domain;

public static class SceneConverter
{
    public static string SceneToString(Scene scene)
    {
        return scene switch
        {
            Scene.Town => "town",
            Scene.Supermarket => "supermarket",
            Scene.MiniMarket => "minimarket",
            Scene.BulkStore => "bulkstore",
            Scene.Kiosk => "kiosk",
            Scene.Bakery1 => "bakery1",
            Scene.Bakery2 => "bakery2",
            Scene.Bakery3 => "bakery3",
            Scene.Bakery4 => "bakery4",
            Scene.Bakery5 => "bakery5",
            Scene.Bakery6 => "bakery6",
            Scene.Bakery7 => "bakery7",
            Scene.Bakery8 => "bakery8",
            Scene.Bakery9 => "bakery9",
            Scene.Bakery10 => "bakery10",
            _ => "unknown",
        };
    }

    public static Scene StringToScene(string sceneString)
    {
        return sceneString.ToLowerInvariant() switch
        {
            "town" => Scene.Town,
            "supermarket" => Scene.Supermarket,
            "minimarket" => Scene.MiniMarket,
            "bulkstore" => Scene.BulkStore,
            "kiosk" => Scene.Kiosk,
            "bakery1" => Scene.Bakery1,
            "bakery2" => Scene.Bakery2,
            "bakery3" => Scene.Bakery3,
            "bakery4" => Scene.Bakery4,
            "bakery5" => Scene.Bakery5,
            "bakery6" => Scene.Bakery6,
            "bakery7" => Scene.Bakery7,
            "bakery8" => Scene.Bakery8,
            "bakery9" => Scene.Bakery9,
            "bakery10" => Scene.Bakery10,
            _ => Scene.Unknown,
        };
    }
}
