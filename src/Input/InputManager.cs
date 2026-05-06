using System;
using Silk.NET.SDL;

namespace TheAdventure;

public class InputManager
{
    private readonly Sdl _sdl;
    private Event _event;

    // Keyboard
    private byte[] _keyboardState;
    private byte[] _previousKeyboardState;
    private unsafe byte* _nativeKeyboardState;

    // Mouse position
    private int _mouseX;
    private int _mouseY;

    // Mouse buttons
    private bool _mouseLeftDown;
    private bool _mouseLeftClicked;

    private bool _mouseRightDown;
    private bool _mouseRightClicked;

    public InputManager(Sdl sdl)
    {
        _sdl = sdl;

        unsafe
        {
            _nativeKeyboardState = _sdl.GetKeyboardState(null);
        }

        _keyboardState = new byte[(int)KeyCode.Count];
        _previousKeyboardState = new byte[(int)KeyCode.Count];
    }

public void ProcessEvents()
{
    _mouseLeftClicked = false;
    _mouseRightClicked = false;

    // 1. SAVE previous state FIRST
    for (int i = 0; i < (int)KeyCode.Count; i++)
    {
        _previousKeyboardState[i] = _keyboardState[i];
    }

    unsafe
    {
        while (_sdl.PollEvent(ref _event) != 0)
        {
            switch ((EventType)_event.Type)
            {
                case EventType.Quit:
                    Environment.Exit(0);
                    break;

                case EventType.Mousemotion:
                    _mouseX = _event.Motion.X;
                    _mouseY = _event.Motion.Y;
                    break;

                case EventType.Mousebuttondown:
                {
                    byte button = _event.Button.Button;

                    if (button == 1)
                    {
                        _mouseLeftDown = true;
                        _mouseLeftClicked = true;
                    }

                    if (button == 3)
                    {
                        _mouseRightDown = true;
                        _mouseRightClicked = true;
                    }

                    break;
                }

                case EventType.Mousebuttonup:
                {
                    byte button = _event.Button.Button;

                    if (button == 1)
                        _mouseLeftDown = false;

                    if (button == 3)
                        _mouseRightDown = false;

                    break;
                }

                case EventType.Keydown:
                {
                    // optional: force update if needed
                    break;
                }

                case EventType.Keyup:
                {
                    break;
                }
            }
        }

        // 2. Update current keyboard state
        for (int i = 0; i < (int)KeyCode.Count; i++)
        {
            _keyboardState[i] = _nativeKeyboardState[i];
        }
    }
}
    // ---------------- KEYBOARD ----------------

    public bool IsKeyDown(KeyCode key)
    {
        return _keyboardState[(int)key] != 0;
    }

    public bool IsKeyPressed(KeyCode key)
    {
        int k = (int)key;

        return _keyboardState[k] != 0 &&
               _previousKeyboardState[k] == 0;
    }

    // ---------------- MOUSE POSITION ----------------

    public (int X, int Y) GetMousePosition()
    {
        return (_mouseX, _mouseY);
    }

    // ---------------- MOUSE BUTTONS ----------------

    public bool IsMouseLeftDown() => _mouseLeftDown;
    public bool IsMouseLeftClicked() => _mouseLeftClicked;

    public bool IsMouseRightDown() => _mouseRightDown;
    public bool IsMouseRightClicked() => _mouseRightClicked;
}