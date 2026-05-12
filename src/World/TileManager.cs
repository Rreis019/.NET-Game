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

    public void RemoveAtPosition(Vector2D<float> pos)
    {
        _tiles.RemoveAll(t =>
            pos.X >= t.x && pos.X < t.x + 16 &&
            pos.Y >= t.y && pos.Y < t.y + 16
        );
    }

    public void RenderTile(int index,float x,float y)
    {
        var src = _tileSet.GetSource(index);

        var dest = new Rectangle<int>(
            (int)x,
            (int)y,
            16,
            16
        );

        Game.Instance.textures.Render(
            _tileSet.textureId,
            src,
            dest
        );
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        foreach (var tile in _tiles)
        {
            RenderTile(tile.index,tile.x,tile.y);
        }
    }
}