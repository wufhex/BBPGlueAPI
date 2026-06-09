using System;
using System.Collections;
using System.Collections.Generic;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Helper for discovering, mapping, and triggering level random events.
    /// </summary>
    public sealed class BBPEvents
    {
        private readonly List<BBPRandomEvent> _events = new List<BBPRandomEvent>();
        private readonly List<string> _eventNames = new List<string>();

        /// <summary>
        /// Collection of mapped random events for the current level.
        /// </summary>
        public IReadOnlyList<BBPRandomEvent> Events => _events;
        /// <summary>
        /// Human-readable names for the mapped events.
        /// </summary>
        public IReadOnlyList<string> EventNames => _eventNames;

        /// <summary>
        /// Status message describing the last mapping/trigger operation.
        /// </summary>
        public string Status { get; private set; } = "Not mapped.";
        /// <summary>
        /// Number of mapped events.
        /// </summary>
        public int Count => _events.Count;

        /// <summary>
        /// Refreshes internal state; currently a no-op placeholder.
        /// </summary>
        public void Refresh()
        {
        }

        /// <summary>
        /// Scans the current scene and environment to map available random events.
        /// </summary>
        /// <returns>True if mapping succeeded and events were found; otherwise false.</returns>
        public bool MapCurrentLevel()
        {
            _events.Clear();
            _eventNames.Clear();

            object? ec = BBP.Environment.Raw;

            if (ec == null)
            {
                Status = "No EnvironmentController.";
                return false;
            }

            IList? eventList = ReflectionUtil.GetField<IList>(ec, "events");

            if (eventList == null || eventList.Count == 0)
            {
                Status = "No level events found.";
                return false;
            }

            foreach (object? rawEvent in eventList)
            {
                if (rawEvent == null)
                    continue;

                BBPRandomEvent ev = new BBPRandomEvent(rawEvent);

                _events.Add(ev);
                _eventNames.Add($"{ev.Name} ({ev.Type})");
            }

            Status = $"Mapped {_events.Count} event(s).";
            return true;
        }

        /// <summary>
        /// Triggers a mapped event by index.
        /// </summary>
        /// <param name="index">Index into the mapped event list.</param>
        /// <returns>True if the event was triggered; otherwise false.</returns>
        public bool Trigger(int index)
        {
            if (index < 0 || index >= _events.Count)
            {
                Status = "Invalid event index.";
                return false;
            }

            return Trigger(_events[index]);
        }

        /// <summary>
        /// Triggers an event by matching a name or type.
        /// </summary>
        /// <param name="eventName">Name or type string to search for.</param>
        /// <returns>True if the event was found and triggered; otherwise false.</returns>
        public bool Trigger(string eventName)
        {
            for (int i = 0; i < _events.Count; i++)
            {
                if (
                    string.Equals(_eventNames[i], eventName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(_events[i].Name, eventName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(_events[i].Type, eventName, StringComparison.OrdinalIgnoreCase)
                )
                {
                    return Trigger(i);
                }
            }

            Status = $"Event not found: {eventName}";
            return false;
        }

        /// <summary>
        /// Triggers a BBPRandomEvent instance.
        /// </summary>
        /// <param name="ev">The event wrapper to trigger.</param>
        /// <returns>True if triggered successfully; otherwise false.</returns>
        public bool Trigger(BBPRandomEvent ev)
        {
            if (ev.Raw == null)
            {
                Status = "Invalid event.";
                return false;
            }

            object? ec = BBP.Environment.Raw;

            if (ec == null)
            {
                Status = "No EnvironmentController.";
                return false;
            }

            try
            {
                ev.Begin();

                AddCurrentEvent(ec, ev.Raw);
                AddCurrentEventType(ec, ev.Raw);

                Status = $"Triggered {ev.Name} ({ev.Type}).";
                return true;
            }
            catch (Exception ex)
            {
                Status = $"Trigger failed: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Triggers a raw event object by wrapping it in a BBPRandomEvent.
        /// </summary>
        /// <param name="rawEvent">The raw event instance from the game.</param>
        /// <returns>True if the event was triggered; otherwise false.</returns>
        public bool TriggerRaw(object rawEvent)
        {
            return Trigger(new BBPRandomEvent(rawEvent));
        }

        private static void AddCurrentEvent(object ec, object eventInstance)
        {
            IList? currentEvents = ReflectionUtil.GetField<IList>(ec, "currentEvents");

            if (currentEvents != null && !currentEvents.Contains(eventInstance))
                currentEvents.Add(eventInstance);
        }

        private static void AddCurrentEventType(object ec, object eventInstance)
        {
            IList? currentTypes = ReflectionUtil.GetField<IList>(ec, "currentEventTypes");

            if (currentTypes == null)
                return;

            object? type = ReflectionUtil.GetProperty<object>(eventInstance, "Type");

            if (type != null && !currentTypes.Contains(type))
                currentTypes.Add(type);
        }
    }
}