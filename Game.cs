using System;
using System.Diagnostics;
using Silk.NET.SDL;
using Silk.NET.Maths;


using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace TheAdventure;

public class Game
{
    private static Game _instance;

    private Sdl _sdl;
    private IntPtr _window;
    private IntPtr _renderer;
    private Event _event;
    private int windowWidth = 800,windowHeight = 700;
    private bool _quit = false;

    //Managers
    private InputManager   _input;
    private TextureManager _textures;
    private EntityManager  _entities;
    private TileManager    _tiles;
    private TileSet        _tileset;

    private Stopwatch _timer = new();
    private ulong _frames = 0;

    //Getters
    public IntPtr sdlImage { get; }
    public IntPtr window { get;  }
    public Sdl sdl => _sdl;
    public TextureManager textures => _textures;
    public IntPtr renderer => _renderer;


    public static Game Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Game();

            return _instance;
        }
    }

    public Game()
    {
        
    }


    private void Initialize()
    {
        CreateWindowAndRenderer();

        //Initialize Managers
        _input = new InputManager(_sdl);
        _textures = new TextureManager();
        _entities = new EntityManager();
        _tileset = new TileSet("assets/Terrain/Terrain (16x16).png","assets/Terrain/tiles.txt");
        _tiles = new TileManager(_tileset);

        Entity p = EntityFactory.Create(EntityId.Player,300,200);
        Entity apple = EntityFactory.Create(EntityId.Apple,100,200);

        Entity wall = (Entity)new InvisibleCollider(0,400,500,10);

        _entities.Add(p);
        _entities.Add(apple);
        _entities.Add(wall);

        _tiles.Add(new Tile(0,0,0));
        _tiles.Add(new Tile(1,32,0));
    }


    public void Run()
    {
        Initialize();

        while (!_quit)
        {
            _input.ProcessEvents();
            Update();
            Render();
        }

        Shutdown();
    }

    private unsafe void CreateWindowAndRenderer()
    {
        _sdl = new Sdl(new SdlContext());

        if (_sdl.Init(Sdl.InitVideo | Sdl.InitEvents) < 0)
            throw new Exception("SDL init failed");

        //Setup Window
        _window = (IntPtr)_sdl.CreateWindow(
            "Gravity Switcher",
            Sdl.WindowposUndefined,
            Sdl.WindowposUndefined,
            windowWidth,
            windowHeight,
            (uint)WindowFlags.Resizable
        );

        if (_window == IntPtr.Zero)
            throw new Exception("Window creation failed");

        //Setup Renderer
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

        float dt = (float)elapsed.TotalSeconds;

        _entities.Update(dt,_input);
        
     }

    private unsafe void Render()
    {
        var r = (Renderer*)_renderer;

        _sdl.SetRenderDrawColor(r, 255, 255, 255, 255);
        _sdl.RenderClear(r);

        _tiles.Render(_renderer,_sdl);
        _entities.Render(_renderer,_sdl);

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