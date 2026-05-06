using System;
using System.IO;

namespace TheAdventure
{
    public enum EntityId
    {
        Player = 0,
        Apple = 1
    }

    public static class EntityFactory
    {
        public static Entity Create(EntityId id, float x, float y)
        {
            switch (id)
            {
                case EntityId.Player:
                {
                    Player p = new Player(x, y);

                    // Temporary bounds
                    p.SetWorldBounds(0, 800 / 2f, 50f, 700f);

                    return p;
                }

                case EntityId.Apple:
                {
                     TextureData fruitTextureData;
                    // Load texture
                    int appleTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Items/Fruits/", "Apple.png"),out fruitTextureData
                    );

                    Animation idleAnimation = new Animation(
                        spriteSheetId: appleTextureId,
                        frameWidth: 32,
                        frameHeight: 32,
                        frameCount: 11,
                        frameTime: 0.08f,
                        loop: true
                    );

                    Collectible apple = new Collectible(
                        x,
                        y,
                        32,
                        32,
                        appleTextureId,
                        idleAnimation,
                        1 // value / score
                    );

                    return apple;
                }

                default:
                    throw new ArgumentException("Unknown EntityId: " + id);
            }
        }
    }
}