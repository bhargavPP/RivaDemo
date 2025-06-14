using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryLeakDemo
{
    public class LeakyClass
    {
        public LeakyClass()
        {
            // Subscribe to a static event
            LeakyPublisher.StaticEvent += HandleEvent;
        }

        ~LeakyClass()
        {
            Console.WriteLine("LeakyClass finalized (GC collected).");
        }

        private void HandleEvent(object sender, EventArgs e)
        {
            // Do nothing
        }
    }

    public static class LeakyPublisher
    {
        // Static event holding references to LeakyClass instances
        public static event EventHandler StaticEvent;

        public static void RaiseEvent()
        {
            StaticEvent?.Invoke(null, EventArgs.Empty);
        }
    }
}
