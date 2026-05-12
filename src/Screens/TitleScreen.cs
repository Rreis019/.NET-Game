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


        if (input.IsKeyPressed(KeyCode.E))
        {
            g.screens.SetScreen(g.levelEditor);
        }


        //Game.Instance.entities.Update(dt, input);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        Game g = Game.Instance;

        g.defaultBlackFont.DrawText("PRESS SPACE TO START", 10,10);
        g.defaultBlackFont.DrawText("PRESS E TO GO LEVEL EDITOR", 10,30);
        //Game.Instance.tiles.Render(renderer, sdl);
        //Game.Instance.entities.Render(renderer, sdl);
    }
}