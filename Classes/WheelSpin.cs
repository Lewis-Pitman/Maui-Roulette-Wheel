using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spinning_Wheel.Classes
{
    internal class WheelSpin
    {
        private GraphicsView wheel;
        private Random random;

        //Spin
        private int MaxSpinSpeed = 25;
        private int SpinDuration = 3000; //Milliseconds
        public double currentAngle;

        public WheelSpin(GraphicsView _wheel)
        {
            wheel = _wheel;
            currentAngle = 0f;
            random = new Random();
        }

        public async Task SpinAsync()
        {
            //The wheel spin happens on a background thread to improve performance
            //This may result in a laggier looking wheel, but means the spin happens for its intended duration
            //On a less powerful device, a laggy wheel spin would result in much longer duration spins

            await Task.Run(async () =>
            {
                currentAngle = 0f;

                SpinDuration = random.Next(2000, 4000); // Random spin length makes result more random
                MaxSpinSpeed = random.Next(20, 35); // Random max speed makes result more random

                double elapsedTime = 0;

                while (elapsedTime < SpinDuration)
                {
                    double progress = elapsedTime / SpinDuration;
                    double speedMultiplier = Math.Sin((1 - progress) * Math.PI / 2); // Sine wave from 1 to 0
                    double speed = MaxSpinSpeed * speedMultiplier;

                    currentAngle += speed;
                    currentAngle %= 360; // Ensure angle does not exceed 360 degrees

                    // Update the wheel's rotation on the main thread
                    wheel.Dispatcher.Dispatch(() =>
                    {
                        wheel.Rotation = currentAngle;
                    });

                    await Task.Delay(16); // Wait for 16ms to get a 60fps animation
                    elapsedTime += 16;
                }
            });
        }
    }
}
