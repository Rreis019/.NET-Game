using System;
using System.Diagnostics;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Game
{
    private Sdl _sdl;
    private IntPtr _window;
    private IntPtr _renderer;
    private int windowWidth = 800,windowHeight = 700;

    private bool _quit = false;

    private Stopwatch _timer = new();
    private ulong _frames = 0;

    private Event _event;

    private Player _player;
    private Ground _groundTop,_groundBottom;
    private List<Coin> _coins = new List<Coin>();


    private InputManager _input;



    public Game()
    {
        _sdl = new Sdl(new SdlContext());
        _input = new InputManager(_sdl);

        _groundTop = new Ground(0, 0, windowWidth, 50);
        _groundBottom = new Ground(0, windowHeight-50, windowWidth, 50);

        _player = new Player(100,200);
        _player.SetWorldBounds(0,windowWidth/2,(float)50, (float)windowHeight-50);

        _coins.Add(new Coin(50, 70));
        _coins.Add(new Coin(500, 70));
        _coins.Add(new Coin(400, 70));
        _coins.Add(new Coin(650, 70));
        _coins.Add(new Coin(750, 70));
    }

    public void Run()
    {
        Init();

        while (!_quit)
        {
            _input.ProcessEvents();
            Update();
            Render();
        }

        Shutdown();
    }

    private unsafe void Init()
    {
        if (_sdl.Init(Sdl.InitVideo | Sdl.InitEvents) < 0)
            throw new Exception("SDL init failed");

        _window = (IntPtr)_sdl.CreateWindow(
            "The Adventure",
            Sdl.WindowposUndefined,
            Sdl.WindowposUndefined,
            windowWidth,
            windowHeight,
            (uint)WindowFlags.Resizable
        );

        if (_window == IntPtr.Zero)
            throw new Exception("Window creation failed");

        _renderer = (IntPtr)_sdl.CreateRenderer((Window*)_window, -1, (uint)RendererFlags.Accelerated);

        if (_renderer == IntPtr.Zero)
            throw new Exception("Renderer creation failed");

        _sdl.RenderSetVSync((Renderer*)_renderer, 1);

        _timer.Start();
    }

    private void HandleEvents()
    {
        while (_sdl.PollEvent(ref _event) != 0)
        {
            switch ((EventType)_event.Type)
            {
                case EventType.Quit:
                    _quit = true;
                    break;
                case EventType.Keydown:
                    Console.WriteLine($"Key down: {(KeyCode)_event.Key.Keysym.Scancode}");
                    break;
            }
        }
    }

    private void HandleCollisions()
    {
        for (int i = _coins.Count - 1; i >= 0; i--)
        {
            if (_player.Intersects(_coins[i]))
            {
                _coins[i].IsActive = false;
            }
        }

        _coins.RemoveAll(c => !c.IsActive);
   
    }

    private void Update()
    {
        var elapsed = _timer.Elapsed;

        _timer.Restart();

        float dt = (float)elapsed.TotalSeconds;

        _player.Update(dt,_input);

        foreach (var coin in _coins)
            coin.Update(dt,_input);

        HandleCollisions();
     }

    private unsafe void Render()
    {
        var r = (Renderer*)_renderer;

        _sdl.SetRenderDrawColor(r, 255, 255, 255, 255);
        _sdl.RenderClear(r);


        _player.Render(_renderer,_sdl);
        _groundTop.Render(_renderer, _sdl);
        _groundBottom.Render(_renderer, _sdl);

        foreach (var coin in _coins)
            coin.Render(_renderer, _sdl);



        _sdl.RenderPresent(r);
        _frames++;
    }

    private unsafe void Shutdown()
    {
        _sdl.DestroyRenderer((Renderer*)_renderer);
        _sdl.DestroyWindow((Window*)_window);
        _sdl.Quit();
    }
}