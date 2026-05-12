using System;
using Silk.NET.SDL;

namespace TheAdventure;

public class InvisibleCollider : Entity
{
    private int _width,_height;

    public InvisibleCollider(float x, float y,int width,int height)
        : base(x, y)
    {
        id = 1337;
        _width = width;
        _height = height;
        isStatic   = true;
        hasPhysics = true;
        collider = new Collider(0,0,_width,_height,ColliderType.Solid);
    }

    public override void Update(float dt, InputManager input)
    {
    }

    public override void OnCollide(Entity other)
    {
    }


    public override void Render(IntPtr renderer, Sdl sdl)
    {
    }
}