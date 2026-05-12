using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;


public class GameScreen : IScreen
{

    public   void OnEnter()
    {

        Game g = Game.Instance;

        //TODO : Just testing the entitites remove after and use levels

        Entity p = EntityFactory.Create(EntityId.Player,150,100);
        Entity apple = EntityFactory.Create(EntityId.Apple,50,100);

        Entity wall = (Entity)new InvisibleCollider(0,200,250,10);
        Entity wall2 = (Entity)new InvisibleCollider(0,0,250,10);


        Entity saw = EntityFactory.Create(EntityId.Saw,50,100);

        Entity smashHead = EntityFactory.Create(EntityId.RockHead,90,100);

        Entity spike = EntityFactory.Create(EntityId.Spike,90,100);



        g.entities.Add(p);
        g.entities.Add(apple);
        g.entities.Add(wall);
        g.entities.Add(wall2);

        g.entities.Add(saw);
        g.entities.Add(smashHead);

        g.entities.Add(spike);
        
        g.tiles.Add(new Tile(0,0,0));
        g.tiles.Add(new Tile(1,32,0));
    }
    

    public   void OnExit()
    {

    }


    public void Update(float dt, InputManager input)
    {
        Game.Instance.entities.Update(dt, input);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        Game.Instance.tiles.Render(renderer, sdl);
        Game.Instance.entities.Render(renderer, sdl);
    }
}