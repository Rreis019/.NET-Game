using Silk.NET.Maths;

namespace TheAdventure;

public class Animator
{
    private readonly Dictionary<string, Animation> _animations = new();

    private Animation _current;

    public void Add(string name, Animation animation)
    {
        _animations[name] = animation;
    }

    public void Play(string name)
    {
        if (!_animations.ContainsKey(name))
            return;

        if (_current == _animations[name])
            return;

        _current = _animations[name];
    }

    public void Update(float dt)
    {
        _current?.Update(dt);
    }

    public Rectangle<int> GetFrame()
    {
        return _current?.GetCurrentFrame() 
               ?? new Rectangle<int>(0, 0, 0, 0);
    }

    public int GetTextureId()
    {
        return _current?._textureId ?? 0; // ou expõe property (melhor)
    }
}