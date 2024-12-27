using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spinning_Wheel
{
    internal class WheelSpin
    {
        private GraphicsView wheel;
        private CancellationTokenSource cancellationTokenSource;

        //Spin
        private const double MaxSpinSpeed = 25.0;
        private const int SpinDuration = 3000; //Milliseconds
        public double currentAngle;
        
        public WheelSpin(GraphicsView _wheel)
        {
            wheel = _wheel;
            currentAngle = 0f;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task SpinAsync()
        {
            cancellationTokenSource.Cancel();

            double elapsedTime = 0;

            while (elapsedTime < SpinDuration)
            {
                double progress = elapsedTime / SpinDuration;
                double speedMultiplier = Math.Sin((1 - progress) * Math.PI / 2); //Sine wave from 1 to 0
                double speed = MaxSpinSpeed * speedMultiplier;

                currentAngle += speed;
                currentAngle %= 360; //Ensure angle does not exceed 360 degrees

                wheel.Rotation = currentAngle;

                await Task.Delay(16); //Wait for 16ms to get a 60fps animation
                elapsedTime += 16;
            }
        }
    }
}
