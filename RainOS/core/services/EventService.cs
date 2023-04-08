using RainOS.core.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.services
{
    public class EventService
    {
        private static Dictionary<string, objects.EventCallback> subscriptions;

        private static List<KeyValuePair<string, objects.Event>> eventQueue;

        public static void Init()
        {
            subscriptions = new Dictionary<string, objects.EventCallback>();
            eventQueue = new List<KeyValuePair<string, objects.Event>>();
        }

        /// <summary>
        /// Adds an event to the queue to be processed by the eventservice.
        /// </summary>
        /// <param name="ev">the event to send</param>
        /// <param name="reciever">the reciever that should recieve the event</param>
        /// <returns>true if sucessful</returns>
        public static bool PushEvent(objects.Event ev, string reciever)
        {
            if (eventQueue == null)
                return false;

            var pair = new KeyValuePair<string, objects.Event>(reciever, ev);

            if (!eventQueue.Contains(pair))
            {
                eventQueue.Add(pair);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Registers a new event handler
        /// </summary>
        /// <param name="reciever">the reciever that the callback function should be assigned to</param>
        /// <param name="handler">the function which handles the events</param>
        /// <returns>true if successful.</returns>
        public static bool RegisterHandler(string reciever, objects.EventCallback handler)
        {
            if (subscriptions == null)
                return false;

            if (!subscriptions.ContainsKey(reciever))
            {
                subscriptions[reciever] = handler;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UnregisterHandler(string reciever)
        {
            if (subscriptions == null)
                return false;

            if (subscriptions.ContainsKey(reciever))
            {
                subscriptions.Remove(reciever);
                return true;
            }
            else
                return false;
        }

        public static void ProcessEvents()
        {
            if (subscriptions == null)
                return;

            if (eventQueue == null)
                return;

            if (subscriptions.Count == 0 || eventQueue.Count == 0)
                return;

            foreach (KeyValuePair<string, Event> kv in eventQueue)
            {
                if (subscriptions.ContainsKey(kv.Key))
                {
                    subscriptions[kv.Key](kv.Value);
                    eventQueue.Remove(kv);
                }
            }
        }
    }
}
