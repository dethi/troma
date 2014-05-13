using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public struct Timer
    {
        public uint ID;
        public double StartedTime;
        public double Duration;
        public EventHandler Callback;
    }

    public class TimerManager
    {
        private static uint totalTime;
        private static uint elapsedTime;

        private static uint nextID;
        private static List<Timer> timerBuffer;
        private static List<Timer> currentTimerBuffer;

        public static void Initialize()
        {
            totalTime = 0;
            elapsedTime = 0;

            nextID = 0;
            timerBuffer = new List<Timer>();
            currentTimerBuffer = new List<Timer>();
        }

        public static void Update(GameTime gameTime)
        {
            elapsedTime = (uint)gameTime.ElapsedGameTime.Milliseconds;
            totalTime += elapsedTime;

            currentTimerBuffer.Clear();
            currentTimerBuffer.AddRange(timerBuffer);

            foreach (Timer t in currentTimerBuffer)
            {
                if (totalTime - t.StartedTime > t.Duration)
                {
                    if (t.Callback != null)
                        t.Callback(null, null);
                    timerBuffer.Remove(t);
                }
            }
        }

        public static void Add(uint duration, EventHandler callback)
        {
            Timer t = new Timer
            {
                ID = nextID,
                StartedTime = totalTime,
                Duration = duration,
            };

            t.Callback += callback;

            nextID++;
            timerBuffer.Add(t);
        }
    }
}
