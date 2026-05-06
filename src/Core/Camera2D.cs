using Silk.NET.Maths;

namespace TheAdventure;

public class Camera2D
{
    public Vector2D<float> position;
    public float zoom = 1.0f;

    public Camera2D()
    {
        position = new Vector2D<float>(0, 0);
        zoom = 1.0f;
    }

    // WORLD -> SCREEN
    public Vector2D<int> WorldToScreen(Vector2D<float> worldPos)
    {
        float x = (worldPos.X - position.X) * zoom;
        float y = (worldPos.Y - position.Y) * zoom;

        return new Vector2D<int>((int)x, (int)y);
    }

    public Rectangle<int> WorldToScreenRect(Rectangle<int> rect)
    {
        return new Rectangle<int>(
            (int)((rect.Origin.X - position.X) * zoom),
            (int)((rect.Origin.Y - position.Y) * zoom),
            (int)(rect.Size.X * zoom),
            (int)(rect.Size.Y * zoom)
        );
    }
}