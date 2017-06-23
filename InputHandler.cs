using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace template_P3
{
    static class InputHandler
    {
        static KeyboardState kbState, kbStatePrev;
        static MouseState mState;
        static Camera camera;
        static Vector2 mPos, prevMPos;
        static GameObject linkedObject;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public static void Init(Camera camera)
        {
            InputHandler.camera = camera;
            InputHandler.linkedObject = camera;
            kbState = Keyboard.GetState();
            kbStatePrev = kbState;
            mState = Mouse.GetCursorState();
            mPos = new Vector2(Game.Width, Game.Height) / 2;
            prevMPos = mPos;
        }

        public static void Update()
        {
            Vector2 window = GetWindowLocation();
            kbStatePrev = kbState;
            kbState = Keyboard.GetState();
            mState = Mouse.GetCursorState();
            //Console.WriteLine(new Vector2(mState.X, mState.Y) - GetWindowLocation());

            prevMPos = mPos;
            mPos = new Vector2(mState.X, mState.Y) - new Vector2(window.X, window.Y);

            Vector2 deltaMouse = (mPos - prevMPos)/120f;
            //Console.WriteLine(deltaMouse);
            camera.Rotation -= new Vector3(deltaMouse.Y, deltaMouse.X, 0);
            Mouse.SetPosition(Game.Width / 2 + window.X, Game.Height / 2 + window.Y);
            mPos = prevMPos;

            if (KeyDown(Key.Comma))
                linkedObject.Rotation -= new Vector3(0.02f, 0, 0f);

            if (KeyDown(Key.Period))
                linkedObject.Rotation += new Vector3(0.02f, 0, 0);

            if (KeyPressed(Key.L))
            {
                if (camera.Parent == null)
                {
                    camera.Parent = linkedObject;
                    camera.Position = new Vector3(0, 10, 0);
                }
                else
                {
                    camera.Parent = null;
                    camera.Position = new Vector3(0, 1, 0);
                }
            }
        }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        public static Vector2 GetWindowLocation()
        {
            Process lol = Process.GetCurrentProcess();
            IntPtr ptr = lol.MainWindowHandle;
            Rect NotepadRect = new Rect();
            GetWindowRect(ptr, ref NotepadRect);
            return new Vector2(NotepadRect.Left, NotepadRect.Top);
        }

        public static bool KeyDown(Key key)
        {
            return kbState.IsKeyDown(key);
        }

        public static bool KeyPressed(Key key)
        {
            return kbState.IsKeyDown(key) && !kbStatePrev.IsKeyDown(key);
        }

        public static GameObject LinkedObject
        {
            get { return linkedObject; }
            set { linkedObject = value; }
        }
    }
}
