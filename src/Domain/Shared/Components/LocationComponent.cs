namespace Domain;

public class LocationComponent
{
    public float X { get; set; }
    public float Y { get; set; }
    public Scene Scene { get; set; }

    public LocationComponent(float x = 0, float y = 0, Scene scene = Scene.Town)
    {
        X = x;
        X = y;
        Scene = scene;
    }
}
