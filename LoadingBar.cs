using System;
using System.Text;
using System.Threading;

namespace LoadingBar
{
    class LoadingBar : IDisposable
    {
        private readonly int BLOCKS;
        private readonly int STARTING_POINT;
        private int prevBlocks;
        private int animState;
        private double currentProgress;
        private double prevProgress;
        private readonly string label;
        private const string animation = @"-\|/";
        private Timer timer;
        private readonly StringBuilder sb;

        public double Progress { 
            get
            {
                return currentProgress;
            }
            set
            {
                currentProgress = Math.Clamp(value, 0.0, 1.0);
            }
        }

        /// <summary>
        /// Class to display progress while performing some task. Intended to be used in a "using" block.
        /// </summary>
        /// <param name="text">Label text to display while loading. ": " is added automatically.</param>
        /// <param name="blocks">Number of blocks that the loading bar is made up of.</param>
        /// <param name="refreshRate">How often the loading bar display updates in milliseconds.</param>
        public LoadingBar(string text="Loading", int blocks=15, int refreshRate=100)
        {
            label = text + ": ";
            prevBlocks = 0;
            animState = 0;
            currentProgress = 0.0;
            prevProgress = 0.0;
            BLOCKS = blocks;
            STARTING_POINT = label.Length;
            sb = new StringBuilder();
            InitDisplay(refreshRate);
        }

        private void InitDisplay(int refreshRate)
        {
            sb.Append(label + '[');
            for (int i = 0; i < BLOCKS; i++)
            {
                sb.Append('-');
            }
            sb.Append("] 00.0%");
            Console.Write(sb);
            timer = new Timer(UpdateDisplay, null, 0, refreshRate);
        }

        private void UpdateDisplay(object state)
        {
            lock (timer)
            {
                sb.Clear();
                double progressSnapshot = currentProgress;
                if (progressSnapshot != prevProgress)
                {
                    int currentNumBlocks = (int)Math.Floor(currentProgress * BLOCKS);
                    if (prevBlocks != currentNumBlocks)
                    {
                        Console.SetCursorPosition(STARTING_POINT + prevBlocks + 1, Console.CursorTop);  // +1 for '['
                        while (prevBlocks < currentNumBlocks)
                        {
                            prevBlocks++;
                            sb.Append('#');
                        }
                        for (int i = 0; i < (BLOCKS - currentNumBlocks); i++)
                        {
                            sb.Append('-');
                        }
                        sb.Append(']');
                    }
                    else
                    {
                        prevProgress = progressSnapshot;
                        Console.SetCursorPosition(STARTING_POINT + BLOCKS + 2, Console.CursorTop);      // + 2 for "[]"
                    }
                    sb.Append(" " + (progressSnapshot * 100).ToString("00.0") + "%");
                }
                else
                {
                    Console.SetCursorPosition(STARTING_POINT + BLOCKS + 8, Console.CursorTop);          // + 8 for "] xx.xx%"
                }
                sb.Append(" " + animation[animState]);
                animState++;
                animState %= animation.Length;
                Console.Write(sb);
            }
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
