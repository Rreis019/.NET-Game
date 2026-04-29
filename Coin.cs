using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Coin : ScrollingObject
{
    public Coin(float x, float y)
        : base(x, y, 16, 16)
    {
    }

    public override void Render(IntPtr renderer, Sdl sdl)
    {
        unsafe
        {
            var r = (Renderer*)renderer;

            sdl.SetRenderDrawColor(r, 255, 215, 0, 255);

            var rect = new Rectangle<int>((int)X, (int)Y, (int)Width, (int)Height);
            sdl.RenderFillRect(r, ref rect);
        }
    }
}