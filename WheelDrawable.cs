using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Spinning_Wheel
{

    internal class WheelDrawable : IDrawable
    {
        private float wheelSize = 200;
        private ObservableCollection<Item> items;

        private Color[] colorList = { Colors.Green, Colors.Red, Colors.Yellow, Colors.Blue}; // --> Change to odd no. of colours to prevent 2 next to each other

        public WheelDrawable(ObservableCollection<Item> _items)
        {
            items = _items ?? new ObservableCollection<Item>();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            #region Clip path
            if (OperatingSystem.IsWindows())
            {
                //Add a message..?
            }
            else
            {
                PathF clipPath = new PathF();
                clipPath.AppendCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize);
                canvas.ClipPath(clipPath);
            }
            #endregion

            #region Drawing sectors
            int colourCount = 0;
            float anglePerSector = 360f / items.Count();

            #region Edge cases
            if (items.Count() <= 3)
            {
                //Edge cases (Where no. of sectors is 0, 1, 2, or 3)

                switch (items.Count())
                {
                    case 1:
                        //Background
                        canvas.FillColor = colorList[0];
                        canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize);

                        //Text
                        canvas.FontColor = Colors.White;
                        canvas.FontSize = 30;
                        canvas.DrawString(items[0].Title, dirtyRect.Center.X - (wheelSize / 2), dirtyRect.Center.Y, HorizontalAlignment.Center);

                        //White centre
                        canvas.FillColor = Colors.White;
                        canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize / 4);

                        //Border
                        canvas.StrokeColor = Colors.Gray;
                        canvas.StrokeSize = wheelSize / 10;
                        canvas.DrawEllipse(dirtyRect.Center.X - wheelSize, dirtyRect.Center.Y - wheelSize, wheelSize * 2, wheelSize * 2);
                        break;
                    case 2:
                        //Essentially having the circle background representing one sector and a clipped rectangle representing the other
                        //I have found that trying to draw arcs with Maui graphics can be tricky sometimes

                        //Sector 1
                        canvas.FillColor = colorList[0];
                        canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize);

                        //Text
                        canvas.FontColor = Colors.White;
                        canvas.FontSize = 30;
                        canvas.DrawString(items[0].Title, dirtyRect.Center.X - (wheelSize / 2), dirtyRect.Center.Y, HorizontalAlignment.Center);

                        //Sector 2
                        PathF rectPath = new PathF();

                        canvas.StrokeSize = 6;
                        canvas.StrokeColor = colorList[1];
                        canvas.FillColor = colorList[1];

                        rectPath.MoveTo(dirtyRect.Center.X, 0);
                        rectPath.LineTo(dirtyRect.Center.X, dirtyRect.Height);
                        rectPath.LineTo(dirtyRect.Width, dirtyRect.Height);
                        rectPath.LineTo(dirtyRect.Width, 0);
                        rectPath.LineTo(dirtyRect.Center.X, 0);
                        canvas.FillPath(rectPath);
                        canvas.DrawPath(rectPath);

                        //Text
                        canvas.FontColor = Colors.White;
                        canvas.FontSize = 30;
                        canvas.DrawString(items[1].Title, dirtyRect.Center.X + (wheelSize / 2), dirtyRect.Center.Y, HorizontalAlignment.Center);

                        //White centre
                        canvas.FillColor = Colors.White;
                        canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize / 4);

                        //Border
                        canvas.StrokeColor = Colors.Gray;
                        canvas.StrokeSize = wheelSize / 10;
                        canvas.DrawEllipse(dirtyRect.Center.X - wheelSize, dirtyRect.Center.Y - wheelSize, wheelSize * 2, wheelSize * 2);

                        break;
                    case 3:
                        //Exactly the same as an ordinary value, except the lines being drawn are extended further, to make
                        //sure they fill the entire circle

                        for(int i = 0; i < 3; i++)
                        {
                            PathF path = new PathF();

                            canvas.StrokeSize = 6;
                            canvas.StrokeColor = colorList[colourCount];
                            canvas.FillColor = colorList[colourCount];

                            path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y);

                            //First line
                            float x1 = (float)(dirtyRect.Center.X + wheelSize * 3f * Math.Cos(Math.PI * (anglePerSector * i) / 180.0));
                            float y1 = (float)(dirtyRect.Center.Y + wheelSize * 3f * Math.Sin(Math.PI * (anglePerSector * i) / 180.0));
                            path.LineTo(x1, y1);

                            path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y);

                            //Second line
                            float x2 = (float)(dirtyRect.Center.X + wheelSize * 3f * Math.Cos(Math.PI * (anglePerSector * (i + 1)) / 180.0));
                            float y2 = (float)(dirtyRect.Center.Y + wheelSize * 3f * Math.Sin(Math.PI * (anglePerSector * (i + 1)) / 180.0));
                            path.LineTo(x2, y2);

                            path.LineTo(x1, y1);
                            canvas.FillPath(path);

                            //Explanation
                            //Math.PI * (anglePerSector * (i + 1)) / 180.0) Converts the angle we want to turn to from degrees to radians
                            //Cosine and sine formulas multipled by the angle we want (in radians) gives us coordinates that lie on the circle. We add this to the centre of the circle

                            canvas.DrawPath(path);

                            //Text
                            if (items.Count() <= 30)
                            {
                                string text = items[i].Title;

                                float textAngle = anglePerSector * (i + 0.5f);
                                //(The angle in the middle of the sector, e.g. for a 90 degree sector the middle would be 45 degrees)

                                float textX = (float)(dirtyRect.Center.X + (wheelSize / 2) * Math.Cos(Math.PI * textAngle / 180.0));
                                float textY = (float)(dirtyRect.Center.Y + (wheelSize / 2) * Math.Sin(Math.PI * textAngle / 180.0));
                                //Coordinates are the same as before, except half the length meaning they are in the centre

                                canvas.SaveState();
                                canvas.Translate(textX, textY);
                                canvas.Rotate(textAngle);

                                canvas.FontColor = Colors.White;
                                canvas.FontSize = items.Count() <= 20 ? 150 / items.Count() : items.Count() / 3f; //Effort to keep text readable

                                canvas.DrawString(text, 0, 0, HorizontalAlignment.Center);
                                canvas.RestoreState();

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

                        break;
                    default:
                        //0

                        //Background
                        canvas.FillColor = Colors.DarkGray;
                        canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, wheelSize);

                        //Text
                        canvas.FontColor = Colors.White;
                        canvas.FontSize = 30;
                        canvas.DrawString("Add items using the box below", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);

                        //Border
                        canvas.StrokeColor = Colors.Gray;
                        canvas.StrokeSize = wheelSize / 10;
                        canvas.DrawEllipse(dirtyRect.Center.X - wheelSize, dirtyRect.Center.Y - wheelSize, wheelSize * 2, wheelSize * 2);
                        break;
                }
            }
            #endregion
            else
            {
                for (int i = 0; i < items.Count(); i++)
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
                    if (items.Count() <= 30)
                    {
                        string text = items[i].Title;

                        float textAngle = anglePerSector * (i + 0.5f);
                        //(The angle in the middle of the sector, e.g. for a 90 degree sector the middle would be 45 degrees)

                        float textX = (float)(dirtyRect.Center.X + (wheelSize / 2) * Math.Cos(Math.PI * textAngle / 180.0));
                        float textY = (float)(dirtyRect.Center.Y + (wheelSize / 2) * Math.Sin(Math.PI * textAngle / 180.0));
                        //Coordinates are the same as before, except half the length meaning they are in the centre

                        canvas.SaveState();
                        canvas.Translate(textX, textY);
                        canvas.Rotate(textAngle);

                        canvas.FontColor = Colors.White;
                        canvas.FontSize = items.Count() <= 20 ? 150 / items.Count() : items.Count() / 3f; //Effort to keep text readable

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
            #endregion
        }
    }
}
