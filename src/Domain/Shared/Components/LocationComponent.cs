using System.Numerics;

namespace Domain;

public class LocationComponent
{
    public Vector2 Position { get; set; }
    public Scene Scene { get; set; }

    public LocationComponent(Vector2 position, Scene scene)
    {
        Position = position;
        Scene = scene;
    }
}
