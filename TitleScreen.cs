using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;


public class TitleScreen : IScreen
{

    public void OnEnter()
    {

    }
    

    public   void OnExit()
    {

    }

    public void Update(float dt, InputManager input)
    {
        Game g = Game.Instance;

        if (input.IsKeyPressed(KeyCode.Space))
        {
            g.screens.SetScreen(g.gameScreen);
        }


        //Game.Instance.entities.Update(dt, input);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        //Game.Instance.tiles.Render(renderer, sdl);
        //Game.Instance.entities.Render(renderer, sdl);
    }
}