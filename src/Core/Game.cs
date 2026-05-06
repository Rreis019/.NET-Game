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

    public Camera2D mainCamera; 

    private Sdl _sdl;
    private IntPtr _window;
    private IntPtr _renderer;
    private Event _event;
    private int baseWidth = 400,baseHeight = 224;
    private bool _quit = false;

    //Managers
    private InputManager   _input;
    private TextureManager _textures;
    private EntityManager  _entities;
    private TileManager    _tiles;
    private TileSet        _tileset;
    private ScreenManager  _screens;

    //Screens
    public GameScreen     gameScreen;
    public TitleScreen     titleScreen;

    private Stopwatch _timer = new();
    private ulong _frames = 0;

    //Getters
    public IntPtr sdlImage { get; }
    public IntPtr window { get;  }
    public Sdl sdl => _sdl;
    public TextureManager textures => _textures;
    public IntPtr renderer => _renderer;
    public EntityManager entities => _entities;
    public TileManager tiles => _tiles;
    public ScreenManager screens => _screens;

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
        _screens = new ScreenManager();

        //Initialize Screens
        gameScreen =  new GameScreen();
        titleScreen = new TitleScreen();

        _screens.SetScreen(titleScreen);

        mainCamera = new Camera2D();
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
            baseWidth * 2,
            baseHeight * 2,
            (uint)WindowFlags.Resizable
        );

        if (_window == IntPtr.Zero)
            throw new Exception("Window creation failed");

        //Setup Renderer
        _renderer = (IntPtr)_sdl.CreateRenderer((Window*)_window, -1, (uint)RendererFlags.Accelerated);

        if (_renderer == IntPtr.Zero)
            throw new Exception("Renderer creation failed");

        _sdl.RenderSetVSync((Renderer*)_renderer, 1);

        _sdl.SetHint(Sdl.HintRenderScaleQuality, "0"); // pixel perfect

        _sdl.RenderSetLogicalSize((Renderer*)_renderer, baseWidth, baseHeight);
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

        _screens.Update(dt,_input);
        
     }

    private unsafe void Render()
    {
        var r = (Renderer*)_renderer;

        _sdl.SetRenderDrawColor(r, 255, 255, 255, 255);
        _sdl.RenderClear(r);

        _screens.Render(_renderer,_sdl);

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