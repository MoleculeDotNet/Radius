using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius
{
    public interface IDisplay
    {
        void Render();
        void ClearAll(bool color);
        void DrawRect(int x1, int y1, int x2, int y2, bool fill);
        void DrawBitmap(int x, int y, byte[] bitmap, int w, int h, bool color);
        void DrawText(string s, int x, int y, int size, bool inverted);
        void DrawText(String s, int start, int end, int x, int y, int size, bool inverted);
        void DrawLine(int x0, int y0, int x1, int y1, bool color);
        void DrawCircle(int x0, int y0, int r, bool color);
        void DrawChar(int x, int y, char ch, int size, bool inverted);
        void DrawRectangle(byte[] byteArray, int[] par1);
    }
}
