using System;
using Silk.NET.SDL;

namespace TheAdventure;

public abstract class Entity
{
    public float X;
    public float Y;

    public float Width;
    public float Height;

    public bool IsActive = true;

    protected Entity(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    // (Movement, Physic, etc.)
    public abstract void Update(float deltaTime,InputManager input);

    // Renderization
    public abstract void Render(IntPtr renderer,Sdl sdl);

    // Simple colision AABB
    public bool Intersects(Entity other)
    {
        return !(X + Width < other.X ||
                 X > other.X + other.Width ||
                 Y + Height < other.Y ||
                 Y > other.Y + other.Height);
    }
}