using Plugin.Maui.Audio;
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

        //Sound
        private int numberOfItems;

        public WheelSpin(GraphicsView _wheel, int _numberOfItems)
        {
            wheel = _wheel;
            currentAngle = 0f;
            random = new Random();
            numberOfItems = _numberOfItems;
        }

        public async Task SpinAsync()
        {
            //The wheel spin happens on a background thread to improve performance
            //This may result in a laggier looking wheel, but means the spin happens for its intended duration
            //On a less powerful device, a laggy wheel spin would result in much longer duration spins

            await Task.Run(async () =>
            {
                currentAngle = 0f;

                //Sound effect
                var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("sector_click.wav"));
                int currentSector = 0;
                int calculatedSector;


                SpinDuration = random.Next(2000, 4000); //Random spin length makes result more random
                MaxSpinSpeed = random.Next(20, 35); //Random max speed makes result more random

                double elapsedTime = 0;

                while (elapsedTime < SpinDuration)
                {
                    double progress = elapsedTime / SpinDuration;
                    double speedMultiplier = Math.Sin((1 - progress) * Math.PI / 2); //Sine wave from 1 to 0
                    double speed = MaxSpinSpeed * speedMultiplier;

                    currentAngle += speed;
                    currentAngle %= 360; //Ensure angle does not exceed 360 degrees

                    wheel.Dispatcher.Dispatch(() =>
                    {
                        wheel.Rotation = currentAngle;
                    });

                    //Sound:
                    double normalizedRotation = (wheel.Rotation + 360) % 360; //Same logic to find winner in viewmodel
                    double adjustedRotation = (normalizedRotation + 90) % 360; 
                    double sectorSweep = 360.0 / numberOfItems;

                    calculatedSector = (int)Math.Floor(adjustedRotation / sectorSweep);
                    
                    if(calculatedSector != currentSector)
                    {
                        currentSector = calculatedSector;
                        audioPlayer.Play();
                    }

                    await Task.Delay(16); //Wait for 16ms to get a 60fps animation
                    elapsedTime += 16;
                }
                audioPlayer.Dispose();
            });
        }
    }
}
