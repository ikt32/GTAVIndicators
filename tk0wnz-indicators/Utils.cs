using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace tk0wnz_indicators
{
    public static class Utils
    {
        public static void ShowText(float x, float y, string text, float size = 0.5f)
        {
            Function.Call(Hash.SET_TEXT_FONT, 0);
            Function.Call(Hash.SET_TEXT_SCALE, size, size);
            Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, 255);
            Function.Call(Hash.SET_TEXT_WRAP, 0.0, 1.0);
            Function.Call(Hash.SET_TEXT_CENTRE, 0);
            Function.Call(Hash.SET_TEXT_OUTLINE, true);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y);
        }

        public static void ShowText3D(Vector3 location, List<string> textLines)
        {
            Function.Call(Hash.SET_DRAW_ORIGIN, location.X, location.Y, location.Z, 0);
            
            int i = 0;
            foreach (var line in textLines)
            {
                ShowText(0.0f, 0.0125f * i, line, 0.2f);
                i++;
            }

            float szX = 0.060f;
            float szY = 0.0125f * i;
            Function.Call(Hash.DRAW_RECT, 0.027f, (0.0125f * i) / 2.0f, szX, szY, 75, 75, 75, 75);
            Function.Call(Hash.CLEAR_DRAW_ORIGIN);
        }

        public static void ShowText3D(Vector3 location, float baseSize, List<string> textLines)
        {
            Vector3 cameraPos = GameplayCamera.Position;
            float distance = cameraPos.DistanceTo(location);
            float totalMult = baseSize / (distance * (GameplayCamera.FieldOfView / 60.0f));

            float height = 0.0125f * totalMult;

            Function.Call(Hash.SET_DRAW_ORIGIN, location.X, location.Y, location.Z, 0);
            int i = 0;

            float szX = 0.000f;
            foreach (var line in textLines) {
                float currWidth = getStringWidth(line, 0.2f * totalMult, 0);
                ShowText(0.0f, 0.0f + height* i, line, 0.2f * totalMult);
                if (currWidth > szX)
                    szX = currWidth;
                i++;
            }

            float szY = height * i;
            Function.Call(Hash.DRAW_RECT, 0.0f + szX/2.0f, (height* i) / 2.0f, szX, szY,
                75,75, 75,91);
            Function.Call(Hash.CLEAR_DRAW_ORIGIN);
        }

        private static float getStringWidth(string text, float scale, int font)
        {
            Function.Call(Hash._BEGIN_TEXT_COMMAND_GET_WIDTH, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.SET_TEXT_FONT,font);
            Function.Call(Hash.SET_TEXT_SCALE,scale, scale);
            return Function.Call<float>(Hash._END_TEXT_COMMAND_GET_WIDTH, true);
        }
    }

    public static class Extensions
    {
        public static unsafe UInt32 GetLightStates(this Vehicle veh)
        {
            const ulong offset = 0x928;
            return *(UInt32*)((ulong)veh.MemoryAddress + offset);
        }

        /***
         * CVeh + 0x84c (1604 - 1868) (Damage)
         * 0 - Left Headlight
         * 1 - Right Headlight
         * 2 - Left Taillight
         * 3 - Right Taillight
         * 4 - Front Left Indicator
         * 5 - Front Right Indicator
         * 6 - Rear Left Indicator
         * 7 - Rear Right Indicator
         * 8 - Left Brake
         * 9 - Right Brake
         * 10 - Center Brake
         * 11 - Left Reverse
         * 12 - Right Reverse
         * 13 - ???
         * 14 - ???
         * 15 - ???
         * 16 - ???
         * 17 - Left fog/far  | 4fog Left  1/2 | 3fog left
         * 18 - Right fog/far | 4fog Left  1/2 | 3fog center
         * 19 - ???           | 4fog Right 1/2 | 3fog right
         * 20 - ???           | 4fog Right 1/2 | 
         * 21 - Taxi
         * 22 - ???
         * 23 - ???
         * 24 - ???
         * 25 - ???
         * 26 - ???
         * 27 - ???
         * 28 - ???
         * 29 - ???
         * 30 - ???
         * 31 - ???
         *
         */

        public static unsafe UInt32 GetLightDamageStates(this Vehicle veh)
        {
            const ulong offset = 0x84c;
            return *(UInt32*)((ulong)veh.MemoryAddress + offset);
        }

        public static unsafe void SetLightDamageStates(this Vehicle veh, UInt32 value)
        {
            const ulong offset = 0x84c;
            *(UInt32*)((ulong)veh.MemoryAddress + offset) = value;
        }
    }
}
