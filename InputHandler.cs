using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace template_P3
{
    static class InputHandler
    {
        static KeyboardState kbState, kbStatePrev;

        public static void Init()
        {
            kbState = Keyboard.GetState();
            kbStatePrev = kbState;
        }

        public static void Update()
        {
            kbStatePrev = kbState;
            kbState = Keyboard.GetState();
        }

        public static bool KeyDown(Key key)
        {
            return kbState.IsKeyDown(key);
        }

        public static bool KeyPressed(Key key)
        {
            return kbState.IsKeyDown(key) && !kbStatePrev.IsKeyDown(key);
        }
    }
}
