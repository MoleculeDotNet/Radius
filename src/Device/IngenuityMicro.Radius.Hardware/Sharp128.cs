using System;
using Microsoft.SPOT.Hardware;

using IngenuityMicro.Radius.Fonts;

namespace IngenuityMicro.Radius.Hardware
{
    public struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    /// <summary>
    /// Sharp memory Lcd driver
    /// </summary>
    public class Sharp128 : IDisplay
    {
        private SPI _spi;
        private OutputPort Pwr = new OutputPort(Pin.PA2, true);

        public enum TextAlign { Left, Center, Right };
        public enum VerticalAlign { Top, Center, Bottom };
        public enum TextWrap { None, Wrap };

        private const byte M0 = 0x01;
        private const byte M1 = 0x02;

        readonly byte[] _lineData = new byte[1 + 1 + 16 + 2];
        readonly byte[] _displayData = new byte[1 + 128 * 18 + 1];

        bool _frameInversion;

        public void PowerDisplay(bool power)
        {
            Pwr.Write(power);
        }


        public Sharp128(Cpu.Pin cs = Pin.PA1, SPI.SPI_module spi = SPI.SPI_module.SPI2)
        {
            // initialise line data array
            _lineData[0] = ReverseBits((byte)(M0 | M1)); // update flags
            _lineData[1] = 0; // line data
            for (int i = 0; i < 16; i++)
            {
                _lineData[i + 2] = (byte)i;
            }
            _lineData[18] = 0; // dummy data
            _lineData[19] = 0; // dummy data

            // initialise display data array
            _displayData[0] = ReverseBits((byte)(M0 | M1)); // update flags
            for (int line = 0; line < 128; line++)
            {
                int offset = 1 + line * 18;
                _displayData[offset] = ReverseBits((byte)(line + 1));
                for (int i = offset + 1; i < offset + 1 + 16; i++)
                {
                    _displayData[i] = 0; // (i % 2 == 0) ? (byte)255 : (byte)0;
                }
                _displayData[offset + 17] = 0;// dummy data
            }
            _displayData[_displayData.Length - 1] = 0; // end dummy

            SPI.Configuration spiConfig = new SPI.Configuration(cs, true, 0, 0, false, true, 1000, spi);
            _spi = new SPI(spiConfig);
            _spi.Write(_displayData);
        }

        public void Render()
        {
            _frameInversion = !_frameInversion;
            if (_frameInversion)
            {
                _displayData[0] = ReverseBits((byte)(M0 | M1));
            }
            else
            {
                _displayData[0] = ReverseBits((byte)(M0));
            }
            _spi.Write(_displayData);
        }

        public void ClearAll(bool color)
        {
            DrawRect(0, 0, 127, 127, color);
        }

        public int Clip(int val, int min, int max)
        {
            return val < min ? min : (val > max ? max : val);
        }


        public void DrawRect(int x1, int y1, int x2, int y2, bool fill)
        {
            for (int i = x1; i <= x1 + x2; i++)
            {
                for (int j = y1; j <= y1 + y2; j++)
                {
                    DrawPixel(i, j, fill);
                }
            }
        }

        public void DrawBitmap(int x, int y, byte[] bitmap, int w, int h, bool color)
        {
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    var pixel = bitmap[i + (j / 8) * w];
                    pixel &= (byte)(1 << (j % 8));
                    if ((pixel) != 0)
                    {
                        DrawPixel(x + i, y + j, color);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <returns>end position</returns>
        /*
        public Point DrawString(String s, int x, int y, int size, bool wrap = false, int width = 400)
        {
            int _x = x;
            int _y = y;
            int charW = size * 6;
            int charH = size * 8;
            int rightEdge = x + width - charW;

            int lineStart = 0;
            int lineEnd = 0;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (c == '\n')
                {
                    y += charH;
                    x = _x;
                }
                else if (c == '\r')
                {
                    // skip em
                }
                //else if (c == ' ')
                //{

                //}
                else
                {
                    DrawChar(x, y, c, size, false);
                    x += charW;
                    if (wrap && (x > rightEdge))
                    {
                        y += charH;
                        x = _x;
                    }
                }

                //DrawChar(x, y, s[i], size, false);
                //x += charW;
            }

            return new Point(x,y);
        }
        */

        public struct TextLine
        {
            public int charStart;
            public int charEnd;
            public int x;
            public int y;
            public int width;

            public TextLine(int charStart, int charEnd, int x, int y, int width)
            {
                this.charStart = charStart;
                this.charEnd = charEnd;
                this.x = x;
                this.y = y;
                this.width = width;
            }
        }

        private const int MAX_TEXT_LINES = 16;
        private TextLine[] textLines = new TextLine[MAX_TEXT_LINES];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <returns>end position</returns>
        public int[] DrawString(String s, int x, int y, int size = 1, bool inverted = false, int lineHeight = 0, int width = 128, int height = 0, TextWrap wrap = TextWrap.None, TextAlign textAlign = TextAlign.Left, VerticalAlign verticalAlign = VerticalAlign.Top)
        {
            int _x = x;
            int _y = y;
            y = 0;
            int charW = size * 6;
            int charH = size * 8;
            lineHeight += charH;
            int rightEdge = x + width - charW;

            int lineStart = 0;
            int lineEnd = 0;
            int lineWidth = 0;
            bool cropBottom = height > 0;
            int bottomEdge = y + height - charH;
            bool firstWordInLine = false;
            int numLines = 0;
            int textBlockWidth = 0;

            int minX = int.MaxValue;

            if (lineHeight == 0)
            {
                lineHeight = charH;
            }

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (c == '\n')
                {
                    lineEnd = i - 1;

                    lineWidth = ((lineEnd - lineStart) + 1) * charW;
                    if (lineWidth > textBlockWidth)
                    {
                        textBlockWidth = lineWidth;
                    }
                    if (textAlign == TextAlign.Left)
                    {
                        x = _x;
                        //DrawText(s, lineStart, lineEnd, _x, y, size);
                    }
                    if (textAlign == TextAlign.Center)
                    {
                        x = _x + (int)((width - lineWidth) * 0.5f);
                        //DrawText(s, lineStart, lineEnd, _x + (int)((width - lineWidth) * 0.5f), y, size);
                    }
                    else if (textAlign == TextAlign.Right)
                    {
                        x = _x + width - lineWidth + 1;
                        //DrawText(s, lineStart, lineEnd, _x + width - lineWidth+1, y, size);
                    }
                    if (x < minX)
                    {
                        minX = x;
                    }
                    textLines[numLines] = new TextLine(lineStart, lineEnd, x, y, lineWidth);
                    numLines++;

                    y += lineHeight;
                    if (cropBottom && y > bottomEdge || numLines == textLines.Length)
                    {
                        break;
                    }
                    x = _x;
                    lineStart = i + 1;
                    lineEnd = i + 1;

                    firstWordInLine = false;
                }
                else if (c == '\r')
                {
                    // skip em
                    //lineStart = i + 1;
                    //lineEnd = i + 1;
                    //TODO: I think just skipping \r might cause problems
                }
                else
                {
                    //DrawChar(x, y, c, size, false);
                    x += charW;
                    if (x > rightEdge && firstWordInLine && wrap == TextWrap.Wrap)
                    {
                        lineWidth = ((lineEnd - lineStart) + 1) * charW;
                        if (lineWidth > textBlockWidth)
                        {
                            textBlockWidth = lineWidth;
                        }
                        if (textAlign == TextAlign.Left)
                        {
                            x = _x;
                            //DrawText(s, lineStart, lineEnd, _x, y, size);
                        }
                        if (textAlign == TextAlign.Center)
                        {
                            x = _x + (int)((width - lineWidth) * 0.5f);
                            //DrawText(s, lineStart, lineEnd, _x + (int)((width - lineWidth) * 0.5f), y, size);
                        }
                        else if (textAlign == TextAlign.Right)
                        {
                            x = _x + width - lineWidth + 1;
                            //DrawText(s, lineStart, lineEnd, _x + width - lineWidth+1, y, size);
                        }
                        if (x < minX)
                        {
                            minX = x;
                        }
                        textLines[numLines] = new TextLine(lineStart, lineEnd, x, y, lineWidth);
                        numLines++;

                        y += lineHeight;
                        if (cropBottom && y > bottomEdge || numLines == textLines.Length)
                        {
                            break;
                        }

                        lineStart = lineEnd + 1;
                        if (s[lineStart] == ' ')
                        {
                            lineStart++;
                        }
                        lineEnd = i;
                        x = _x + (lineEnd - lineStart) * charW;

                        firstWordInLine = false;
                    }
                    else
                    {
                        if (c == ' ')
                        {
                            lineEnd = i - 1;
                            firstWordInLine = true;
                        }
                    }
                }

                //DrawChar(x, y, s[i], size, false);
                //x += charW;
            }

            if (cropBottom && y > bottomEdge)
            {

            }
            else
            {
                //TODO this needs further checks whether it is neccessary
                lineEnd = s.Length - 1;
                lineWidth = ((lineEnd - lineStart) + 1) * charW;
                if (lineWidth > textBlockWidth)
                {
                    textBlockWidth = lineWidth;
                }
                if (textAlign == TextAlign.Left)
                {
                    x = _x;
                    //DrawText(s, lineStart, lineEnd, _x, y, size);
                }
                if (textAlign == TextAlign.Center)
                {
                    x = _x + (int)((width - lineWidth) * 0.5f);
                    //DrawText(s, lineStart, lineEnd, _x + (int)((width - lineWidth) * 0.5f), y, size);
                }
                else if (textAlign == TextAlign.Right)
                {
                    x = _x + width - lineWidth + 1;
                    //DrawText(s, lineStart, lineEnd, _x + width - lineWidth+1, y, size);
                }
                if (x < minX)
                {
                    minX = x;
                }
                textLines[numLines] = new TextLine(lineStart, lineEnd, x, y, lineWidth);
                numLines++;
            }

            int yOffset = 0;
            int textBlockHeight = numLines * lineHeight;
            switch (verticalAlign)
            {
                case VerticalAlign.Top:
                    yOffset = _y;
                    break;
                case VerticalAlign.Center:
                    yOffset = _y + (int)((height - textBlockHeight) * .5f);
                    break;
                case VerticalAlign.Bottom:
                    yOffset = _y + height - textBlockHeight + 1;
                    break;
            }

            for (int i = 0; i < numLines; i++)
            {
                TextLine tl = textLines[i];
                DrawText(s, tl.charStart, tl.charEnd, tl.x, tl.y + yOffset, size, inverted);
            }

            int finalRectX1 = minX;
            int finalRectY1 = yOffset;
            int finalRectX2 = finalRectX1 + textBlockWidth;
            int finalRectY2 = finalRectY1 + textBlockHeight;

            return new int[] { finalRectX1, finalRectY1, finalRectX2, finalRectY2 };
        }

        public void DrawText(string s, int x, int y, int size, bool inverted)
        {
            DrawText(s, 0, s.Length, x, y, size, inverted);
        }

        public void DrawText(String s, int start, int end, int x, int y, int size, bool inverted)
        {
            if (end >= start)
            {
                int charW = size * 6;
                for (int i = start; i <= end; i++)
                {
                    if (i >= s.Length)
                    {
                        return;
                    }
                    DrawChar(x, y, s[i], size, inverted);
                    x += charW;
                }
            }
        }

        protected void Swap(ref int a, ref int b)
        {
            var t = a; a = b; b = t;
        }

        // bresenham's algorithm - thx wikipedia
        public void DrawLine(int x0, int y0, int x1, int y1, bool color)
        {
            int steep = (System.Math.Abs(y1 - y0) > System.Math.Abs(x1 - x0)) ? 1 : 0;

            if (steep != 0)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int dx, dy;
            dx = x1 - x0;
            dy = System.Math.Abs(y1 - y0);

            int err = dx / 2;
            int ystep;

            if (y0 < y1)
            {
                ystep = 1;
            }
            else
            {
                ystep = -1;
            }

            for (; x0 < x1; x0++)
            {
                if (steep != 0)
                {
                    DrawPixel(y0, x0, color);
                }
                else
                {
                    DrawPixel(x0, y0, color);
                }

                err -= dy;

                if (err < 0)
                {
                    y0 += ystep;
                    err += dx;
                }
            }


        }

        public void DrawCircle(int x0, int y0, int r, bool color)
        {
            int f = 1 - r;
            int ddF_x = 1;
            int ddF_y = -2 * r;
            int x = 0;
            int y = r;

            DrawPixel(x0, y0 + r, color);
            DrawPixel(x0, y0 - r, color);
            DrawPixel(x0 + r, y0, color);
            DrawPixel(x0 - r, y0, color);

            while (x < y)
            {
                if (f >= 0)
                {
                    y--;
                    ddF_y += 2;
                    f += ddF_y;
                }

                x++;
                ddF_x += 2;
                f += ddF_x;

                DrawPixel(x0 + x, y0 + y, color);
                DrawPixel(x0 - x, y0 + y, color);
                DrawPixel(x0 + x, y0 - y, color);
                DrawPixel(x0 - x, y0 - y, color);

                DrawPixel(x0 + y, y0 + x, color);
                DrawPixel(x0 - y, y0 + x, color);
                DrawPixel(x0 + y, y0 - x, color);
                DrawPixel(x0 - y, y0 - x, color);
            }

        }

        public int LineHeight(int size)
        {
            return size * 8;
        }

        public void DrawChar(int x, int y, char ch, int size, bool inverted)
        {
            if ((x >= 128) || // Clip right
               (y >= 128) || // Clip bottom
               ((x + 5 * size - 1) < 0) || // Clip left
               ((y + 8 * size - 1) < 0))   // Clip top
                return;

            char c = (char)0;

            byte[] bFont = new byte[5];
            Array.Copy(DefautFont.Font, 0 + ch * 5, bFont, 0, 5);



            for (int i = 0; i < 6; i++)
            {
                byte line;
                if (i == 5)
                    line = 0x0;
                else
                    line = bFont[c * 5 + i];

                for (int j = 0; j < 8; j++)
                {
                    if ((line & 0x1) == 1)
                    {
                        if (size == 1) // default size
                            DrawPixel(x + i, y + j, !inverted);
                        else
                        {  // big size
                            int x1 = x + (i * size);
                            int y1 = y + (j * size);
                            DrawRect(x1, y1, x1 + size, y1 + size, !inverted);
                        }
                    }
                    else
                    { //if (bg != color) {
                        if (size == 1) // default size
                            DrawPixel(x + i, y + j, inverted);
                        else
                        {  // big size
                            int x1 = x + (i * size);
                            int y1 = y + (j * size);
                            DrawRect(x1, y1, x1 + size, y1 + size, inverted);
                        }
                    }
                    line >>= 1;
                }

            }
        }

        public void DrawPixel(int x, int y, bool color)
        {
            if (x > 127 || y > 127) return;
            int xloc = 2 + (y * 18) + x / 8;
            int shift = 7 - x % 8;
            if (color)
            {
                _displayData[xloc] |= (byte) (1 << shift);
            }
            else
            {
                _displayData[xloc] &= (byte)(~(1 << shift));
            }
        }
        public void DrawRectangle(byte[] byteArray, int[] par1)
        {
            int x1 = par1[0];
            int y1 = par1[1];
            int x2 = par1[2];
            int y2 = par1[3];
            byte val = (byte)par1[4];

            int x = 0;
            int y = 0;
            int offset = 0;

            for (y = y1; y <= y2; y++)
            {
                offset = 2 + y * 52;
                for (x = x1; x <= x2; x++)
                {
                    byteArray[offset + x] = val;
                }
            }
        }

        public void DrawPixel(byte[] byteArray, int[] par1)
        {
            int x = par1[0];
            int y = par1[1];
            bool color = par1[2] > 0;

            int xloc = 2 + (y * 52) + x / 8;
            int shift = x % 8;

            if (color)
                byteArray[xloc] |= (byte)(1 << shift);
            else
                byteArray[xloc] &= (byte)(~(1 << shift));
        }

        public static byte ReverseBits(byte b)
        {
            return (byte)(((b * 0x0802u & 0x22110u) | (b * 0x8020u & 0x88440u)) * 0x10101u >> 16);
        }
    }

    public enum Colour
    {
        Black,
        White
    }
}
