using System;
using System.Diagnostics;
using Silk.NET.SDL;

namespace TheAdventure;

public class Game
{
    private Sdl _sdl;
    private IntPtr _window;
    private IntPtr _renderer;

    private bool _quit = false;

    private Stopwatch _timer = new();
    private ulong _frames = 0;

    private Event _event;

    public Game()
    {
        _sdl = new Sdl(new SdlContext());
    }

    public void Run()
    {
        Init();

        while (!_quit)
        {
            HandleEvents();
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
            800,
            800,
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

    private void Update()
    {
        var elapsed = _timer.Elapsed;
        _timer.Restart();

        //Main Logic of the game

    }

    private unsafe void Render()
    {
        var r = (Renderer*)_renderer;

        _sdl.SetRenderDrawColor(r, 255, 255, 255, 255);
        _sdl.RenderClear(r);

        _sdl.SetRenderDrawColor(r, 255, 0, 0, 255);
        _sdl.RenderDrawLine(r, 0, 0, 400, 400);

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