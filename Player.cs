using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Player : Entity
{
    public float Speed = 200f;

    // true = gravity goes down, false = gravity goes up
    public bool GravityDown = true;

    private float _velocityY = 0f; 
    private WorldBounds _bounds = new WorldBounds(0,0,0,0);


    public Player(float x, float y) : base(x, y, 32, 64)
    {

        //TextureLoader.Load()

    }

    public override void Update(float dt,InputManager input)
    {
        if (input.IsKeyDown(KeyCode.Space) && _velocityY == 0)
        {
            FlipGravity();
        }

        if (input.IsKeyDown(KeyCode.A))
        {
            X -= Speed * dt;  
        }

        if (input.IsKeyDown(KeyCode.D))
        {
            X += Speed * dt;  
        }

        // Auto forward movement (endless runner behavior)
        //X += Speed * dt;

        // Apply custom gravity
        float gravity = GravityDown ? 5000f : -5000f;
        _velocityY += gravity * dt;

        Y += _velocityY * dt;

        //Apply ground coolider
        if (Y + Height > _bounds.Bottom)
        {
            Y = _bounds.Bottom - Height;
            _velocityY = 0;
        }
        if (Y < _bounds.Top)
        {
            Y = _bounds.Top;
            _velocityY = 0;
        }

        if (X < _bounds.Left)
            X = _bounds.Left;

        if (X + Width > _bounds.Right)
            X = _bounds.Right - Width;
    }

    public override void Render(IntPtr renderer,Sdl sdl)
    {
        unsafe
        {
            var r = (Renderer*)renderer;

            // Set color to red
            sdl.SetRenderDrawColor(r, 255, 0, 0, 255);

            var rect = new Rectangle<int>((int)X, (int)Y, (int)Width,(int)Height);

            sdl.RenderFillRect(r, ref rect);
        }
    }

    public void SetWorldBounds(float left,float right, float top, float bottom)
    {
        _bounds = new WorldBounds(left,right,top, bottom);
    }

    public void FlipGravity()
    {
        // Invert gravity direction
        GravityDown = !GravityDown;

        // Reset vertical velocity to avoid unwanted bounce effect
        _velocityY = 0;
    }
}