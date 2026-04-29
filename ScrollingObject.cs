using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public abstract class ScrollingObject : Entity
{
    public float Speed = 200f;
        private byte r, g, b, a;


    protected ScrollingObject(float x, float y, float width, float height)
        : base(x, y, width, height)
    {
    }

    public override void Update(float dt, InputManager input)
    {
        X -= Speed * dt;

        //Auto destroy if is out of screen
        if (X + Width < 0)
            IsActive = false;
    }

    public override void Render(IntPtr renderer, Sdl sdl)
    {
    }
}