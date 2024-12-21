using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Spinning_Wheel
{

    internal class SectorDrawable : IDrawable
    {
        private float wheelSize = 250;
        private int numberOfSectors = 3; //Max this will ever go to is 186
        //Needs to be edge case for 1, 2, 3 sectors -> They don't appear properly

        private Color[] colorList = { Colors.Green, Colors.Red, Colors.Yellow, Colors.Blue};

        public void changeNumberOfSectors(int _numberOfSectors)
        {
            numberOfSectors = _numberOfSectors;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {


            if (OperatingSystem.IsWindows())
            {
                //slap a message saying clippath isnt supported
            }
            else
            {
                PathF clipPath = new PathF();
                clipPath.AppendCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize);
                canvas.ClipPath(clipPath);
            }

            //Drawing sectors
            int colourCount = 0;
            float anglePerSector = 360f / numberOfSectors;

            for (int i = 0; i < numberOfSectors; i++)
            {
                PathF path = new PathF();

                canvas.StrokeSize = 6;
                canvas.StrokeColor = colorList[colourCount];
                canvas.FillColor = colorList[colourCount];

                path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y);

                //First line
                float x1 = (float)(dirtyRect.Center.X + wheelSize * 1.5f * Math.Cos(Math.PI * (anglePerSector * i) / 180.0));
                float y1 = (float)(dirtyRect.Center.Y + wheelSize * 1.5f * Math.Sin(Math.PI * (anglePerSector * i) / 180.0));
                path.LineTo(x1, y1);

                path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y);
                
                //Second line
                float x2 = (float)(dirtyRect.Center.X + wheelSize * 1.5f * Math.Cos(Math.PI * (anglePerSector * (i + 1)) / 180.0));
                float y2 = (float)(dirtyRect.Center.Y + wheelSize * 1.5f * Math.Sin(Math.PI * (anglePerSector * (i + 1)) / 180.0));
                path.LineTo(x2, y2);

                path.LineTo(x1, y1);
                canvas.FillPath(path);

                //Explanation
                //Math.PI * (anglePerSector * (i + 1)) / 180.0) Converts the angle we want to turn to from degrees to radians
                //Cosine and sine formulas multipled by the angle we want (in radians) gives us coordinates that lie on the circle. We add this to the centre of the circle

                canvas.DrawPath(path);

                //Text
                if (numberOfSectors <= 30)
                {
                    string text = $"Sector {i + 1}";

                    float textAngle = anglePerSector * (i + 0.5f);
                    //(The angle in the middle of the sector, e.g. for a 90 degree sector the middle would be 45 degrees)

                    float textX = (float)(dirtyRect.Center.X + (wheelSize / 2) * Math.Cos(Math.PI * textAngle / 180.0));
                    float textY = (float)(dirtyRect.Center.Y + (wheelSize / 2) * Math.Sin(Math.PI * textAngle / 180.0));
                    //Coordinates are the same as before, except half the length meaning they are in the centre

                    canvas.SaveState();
                    canvas.Translate(textX, textY);
                    canvas.Rotate(textAngle);

                    canvas.FontColor = Colors.White;
                    canvas.FontSize = numberOfSectors <= 20 ? 150 / numberOfSectors : numberOfSectors / 3f; //Effort to keep text readable

                    canvas.DrawString(text, 0, 0, HorizontalAlignment.Center);
                    canvas.RestoreState();
                }

                colourCount = colourCount >= colorList.Length - 1 ? 0 : colourCount + 1;
            }

            //White centre
            canvas.FillColor = Colors.White;
            canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize / 4);

            //Border
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = wheelSize / 10;
            canvas.DrawEllipse(dirtyRect.Center.X - wheelSize, dirtyRect.Center.Y - wheelSize, wheelSize * 2, wheelSize * 2);
        }
    }
}
