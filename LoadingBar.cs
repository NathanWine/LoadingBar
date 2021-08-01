using System;
using System.Text;
using System.Threading;

namespace LoadingBar
{
    class LoadingBar : IDisposable, IProgress<double>
    {
        private double _progress;
        private double prevProgress;
        private readonly int BLOCKS;
        private int prevBlocks;
        private readonly int STARTING_POINT;
        private readonly Timer timer;
        private StringBuilder sb;
        private const string animation = @"-\|/";
        private int animState;
        public double Progress { 
            get
            {
                return _progress;
            }
            set
            {
                _progress = Math.Clamp(value, 0.0, 1.0);
            }
        }

        public LoadingBar(int startingPoint=0, int blocks=15, int refreshRate=200)
        {
            _progress = 0.0;
            prevProgress = 0.0;
            BLOCKS = blocks;
            STARTING_POINT = startingPoint;
            prevBlocks = 0;
            sb = new StringBuilder();
            timer = new Timer(UpdateDisplay, null, 0, refreshRate);
            animState = 0;
            InitDisplay();
        }

        private void InitDisplay()
        {
            sb.Append('[');
            for (int i = 0; i < BLOCKS; i++)
            {
                sb.Append('-');
            }
            sb.Append("] 00.0%");
            Console.Write(sb);
        }

        private void UpdateDisplay(object state)
        {
            lock (timer)
            {
                int currentNumBlocks = (int) Math.Floor(_progress * BLOCKS);

                if (prevBlocks != currentNumBlocks)
                {
                    sb.Clear();
                    Console.SetCursorPosition(STARTING_POINT + prevBlocks + 1, Console.CursorTop);
                    while (prevBlocks < currentNumBlocks)
                    {
                        prevBlocks++;
                        sb.Append('#');
                    }
                    for (int i = 0; i < (BLOCKS - currentNumBlocks); i++)
                    {
                        sb.Append('-');
                    }
                    sb.Append("] " + (_progress * 100).ToString("00.0#") + "%");
                    Console.Write(sb);
                }
                else if (_progress != prevProgress)
                {
                    sb.Clear();
                    prevProgress = _progress;
                    Console.SetCursorPosition(STARTING_POINT + BLOCKS + 2, Console.CursorTop);
                    sb.Append(" " + (_progress * 100).ToString("00.0#") + "%");
                    Console.Write(sb);
                }
                //else
                //{
                //    Console.SetCursorPosition(STARTING_POINT + BLOCKS + 8, Console.CursorTop);
                //}
                //sb.Append(" " + animation[animState]);
                //animState++;
                //animState %= animation.Length;
            }
        }

        public void Report(double value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            sb.Clear();
            Console.SetCursorPosition(STARTING_POINT + prevBlocks + 1, Console.CursorTop);
            while (prevBlocks < BLOCKS)
            {
                prevBlocks++;
                sb.Append('#');
            }
            sb.Append("] - Done!");
            Console.Write(sb);
        }
    }
}
