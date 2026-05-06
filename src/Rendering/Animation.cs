using Silk.NET.Maths;

namespace TheAdventure;

public class Animation
{
    private readonly List<Rectangle<int>> _frames = new();
    private readonly float _frameTime;
    private readonly bool _loop;

    private int _currentFrame;
    private float _timer;

    public float frameTime => _frameTime;
    public bool loop => _loop;
    public int frameCount => _frames.Count;

    public int _textureId;


    public Animation(
        int spriteSheetId,
        int frameWidth,
        int frameHeight,
        int frameCount,
        float frameTime,
        bool loop = true,
        int startX = 0,
        int startY = 0)
    {
        _textureId = spriteSheetId;
        _frameTime = frameTime;
        _loop = loop;

        for (int i = 0; i < frameCount; i++)
        {
            _frames.Add(new Rectangle<int>(
                startX + i * frameWidth,
                startY,
                frameWidth,
                frameHeight
            ));
        }
    }

    // ─────────────────────────────
    // CONSTRUCTOR (MANUAL FRAMES)
    // ─────────────────────────────
    public Animation(int spriteSheetId,float frameTime, bool loop, List<Rectangle<int>> frames)
    {
        _textureId = spriteSheetId;
        _frameTime = frameTime;
        _loop = loop;
        _frames = frames;
    }

    // ─────────────────────────────
    // UPDATE
    // ─────────────────────────────
    public void Update(float dt)
    {
        if (_frames.Count == 0)
            return;

        _timer += dt;

        if (_timer >= _frameTime)
        {
            _timer = 0f;
            _currentFrame++;

            if (_currentFrame >= _frames.Count)
            {
                if (_loop)
                    _currentFrame = 0;
                else
                    _currentFrame = _frames.Count - 1;
            }
        }
    }

    // ─────────────────────────────
    // GET CURRENT FRAME
    // ─────────────────────────────
    public Rectangle<int> GetCurrentFrame()
    {
        if (_frames.Count == 0)
            return new Rectangle<int>(0, 0, 0, 0);

        return _frames[_currentFrame];
    }
}