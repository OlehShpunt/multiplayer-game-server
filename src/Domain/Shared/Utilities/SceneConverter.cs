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
            _ => throw new ArgumentException($"Unknown scene: {scene}"),
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
            _ => throw new ArgumentException($"Unknown scene: {sceneString}"),
        };
    }
}
