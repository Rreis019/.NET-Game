using System.Collections.Generic;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class TileManager
{
    private readonly List<Tile> _tiles = new();

    private TileSet _tileSet;

    public TileManager(TileSet tileSet)
    {
        _tileSet = tileSet;
    }

    public void Add(Tile tile)
    {
        _tiles.Add(tile);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        foreach (var tile in _tiles)
        {
            var src = _tileSet.GetSource(tile.index);

            var dest = new Rectangle<int>(
                tile.x,
                tile.y,
                32,
                32
            );

            Game.Instance.textures.Render(
                _tileSet.textureId,
                src,
                dest
            );
        }
    }
}