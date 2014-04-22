using System;
using System.Configuration;

namespace VP.Tests
{
    static class TestPump
    {
        /// <summary>
        /// Pumps all given instances once
        /// </summary>
        public static void AllOnce(params Instance[] instances)
        {
            foreach (var inst in instances)
                inst.Pump();
        }

        /// <summary>
        /// Pumps all given instances until either the given assertion returns true, or
        /// until a 10 second timeout is reached
        /// </summary>
        public static bool AllUntil(Func<bool> assert, params Instance[] instances)
        {
            var start = DateTime.Now;

            while (DateTime.Now.Subtract(start).TotalMilliseconds < 10000)
            {
                foreach (var inst in instances)
                    inst.Pump();

                if ( assert() )
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Pumps all given instances until a 5 second timeout is reached
        /// </summary>
        public static void AllUntilTimeout(params Instance[] instances)
        {
            var start = DateTime.Now;

            while (DateTime.Now.Subtract(start).TotalMilliseconds < 5000)
                foreach (var inst in instances)
                    inst.Pump();
        }
        
    }
}
