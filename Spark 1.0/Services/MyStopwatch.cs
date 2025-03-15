using System;

namespace Spark.Services
{
    public class MyStopwatch : System.Diagnostics.Stopwatch
    {
        public TimeSpan StartOffset { get; private set; }

        public MyStopwatch() : base()
        {
            StartOffset = TimeSpan.Zero;
        }

        public MyStopwatch(TimeSpan startOffset)
        {
            StartOffset = startOffset;
        }

        public new TimeSpan Elapsed
        {
            get
            {
                return base.Elapsed + StartOffset;
            }
        }


        public new long ElapsedMilliseconds
        {
            get
            {
                return base.ElapsedMilliseconds + (long)StartOffset.TotalMilliseconds;
            }
        }

        public new long ElapsedTicks
        {
            get
            {
                return base.ElapsedTicks + StartOffset.Ticks;
            }
        }
    }
}
