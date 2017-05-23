using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;

namespace Invaders.Infrastructure
{
    [Flags]
    public enum eInputButtons
    {
        // Mouse buttons:
        Left = 65536,
        Middle = 131072,
        Right = 262144,
        XButton1 = 524288,
        XButton2 = 1048576,

        // GamePad buttons:
        DPadUp = Buttons.DPadUp,
        DPadDown = Buttons.DPadDown,
        DPadLeft = Buttons.DPadLeft,
        DPadRight = Buttons.DPadRight,
        Start = Buttons.Start,
        Back = Buttons.Back,
        LeftStick = Buttons.LeftStick,
        RightStick = Buttons.RightStick,
        LeftShoulder = Buttons.LeftShoulder,
        RightShoulder = Buttons.RightShoulder,
        A = Buttons.A,
        B = Buttons.B,
        X = Buttons.X,
        Y = Buttons.Y,

        LeftThumbstickLeft = Buttons.LeftThumbstickLeft,
        RightTrigger = Buttons.RightTrigger,
        LeftTrigger = Buttons.LeftTrigger,
        RightThumbstickUp = Buttons.RightThumbstickUp,
        RightThumbstickDown = Buttons.RightThumbstickDown,
        RightThumbstickRight = Buttons.RightThumbstickRight,
        RightThumbstickLeft = Buttons.RightThumbstickLeft,
        LeftThumbstickUp = Buttons.LeftThumbstickUp,
        LeftThumbstickDown = Buttons.LeftThumbstickDown,
        LeftThumbstickRight = Buttons.LeftThumbstickRight,
    }

    public interface IInputManager
    {
        KeyboardState KeyboardState { get; }

        MouseState MouseState { get; }

        Rectangle BoundsMouse { get; }

        bool KeyPressed(Keys i_Key);

        bool KeyReleased(Keys i_Key);

        bool MouseMoved();

        bool MouseLeftButtonPressed();

        bool MouseRightButtonPressed();

        bool MouseWheelScrolledUp();

        bool MouseWheelScrolledDown();

        bool MouseIntersectWithComponent(Component2D i_Component2D);
    }
}