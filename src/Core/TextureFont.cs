using System;
using System.Collections.Generic;
using Silk.NET.Maths;
using Silk.NET.SDL;

namespace TheAdventure;

public class TextureFont
{
    private readonly string _textureName;
    private readonly int _tileWidth;
    private readonly int _tileHeight;

    private readonly Dictionary<char, int> _charMap = new();

    private int _textureId;
    private TextureData _textureData;

    public TextureFont(string spriteSheetPath, string characters, int tileWidth, int tileHeight)
    {
        _textureName = spriteSheetPath;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;

        // Load texture
        _textureId = Game.Instance.textures.LoadTexture(
            spriteSheetPath,
            out _textureData
        );

        // Create character lookup
        for (int i = 0; i < characters.Length; i++)
        {
            char c = characters[i];

            if (!_charMap.ContainsKey(c))
                _charMap.Add(c, i);
        }
    }

    public void DrawText(string text, float x, float y, float scale = 1.0f)
    {
        float drawX = x;

        int columns = _textureData.Width / _tileWidth;

        foreach (char c in text)
        {
            // Space
            if (c == ' ')
            {
                drawX += _tileWidth * scale;
                continue;
            }

            // Ignore unknown chars
            if (!_charMap.ContainsKey(c))
                continue;

            int index = _charMap[c];

            int srcX = (index % columns) * _tileWidth;
            int srcY = (index / columns) * _tileHeight;

            Rectangle<int> src = new Rectangle<int>(
                srcX,
                srcY,
                _tileWidth,
                _tileHeight
            );

            Rectangle<int> dest = new Rectangle<int>(
                (int)drawX,
                (int)y,
                (int)(_tileWidth * scale),
                (int)(_tileHeight * scale)
            );

            Game.Instance.textures.RenderUI(
                _textureId,
                src,
                dest
            );

            drawX += _tileWidth * scale;
        }
    }
}