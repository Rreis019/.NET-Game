using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public interface IScreen
{
    void OnEnter();
    void OnExit();

    void Update(float dt, InputManager input);
    void Render(IntPtr renderer, Sdl sdl);
}