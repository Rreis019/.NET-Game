using System.IO;
using System.Collections.Generic;
using Silk.NET.Maths;

namespace TheAdventure;

public class TileSet
{
    private Rectangle<int>[] _rects;

    public int textureId { get; private set; }

    public int tileCount => _rects.Length;

    public TileSet(string spriteSheetPath, string tilePositionsPath)
    {
        var g = Game.Instance;

        textureId = g.textures.LoadTexture(spriteSheetPath, out _);

        _rects = LoadRectsFromFile(tilePositionsPath);
    }

    public Rectangle<int> GetSource(int index)
    {
        return _rects[index];
    }

    private Rectangle<int>[] LoadRectsFromFile(string path)
    {
        var list = new List<Rectangle<int>>();

        foreach (var line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var p = line.Split(',');

            int x = int.Parse(p[0]);
            int y = int.Parse(p[1]);
            int x2 = int.Parse(p[2]);
            int y2 = int.Parse(p[3]);

            list.Add(new Rectangle<int>(
                x,
                y,
                x2 - x,
                y2 - y
            ));
        }

        return list.ToArray();
    }
}