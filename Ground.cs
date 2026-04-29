using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Ground : Entity
{
    private byte r, g, b, a;

    public Ground(float x, float y, float width, float height,
                  byte r = 100, byte g = 255, byte b = 100, byte a = 255)
        : base(x, y, width, height)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public override void Update(float deltaTime, InputManager input)
    {
    }

    public override void Render(IntPtr renderer, Sdl sdl)
    {
        unsafe
        {
            var ren = (Renderer*)renderer;
            var rect = new Rectangle<int>((int)X, (int)Y, (int)Width,(int)Height);

               
            sdl.SetRenderDrawColor(ren, r, g, b, a);

            
            sdl.RenderFillRect(ren, ref rect);
        }
    }
}