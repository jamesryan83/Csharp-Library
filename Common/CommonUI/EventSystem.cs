using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CommonUI
{
	//// list of built in wpf commands...
	//// http://stackoverflow.com/a/1691082/4359306


	// Relay Command - for use with UI Commands
	public class RelayCommand : ICommand
	{
		private Action<object> action;

		public RelayCommand(Action<object> action)
		{
			this.action = action;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			action(parameter);
		}
	}






	public delegate void EventSystemEventHandler(object sender, EventSystemEventArgs args);
	

	// Event Arguments class
	public class EventSystemEventArgs : EventArgs
	{
		public object data { get; set; }  // item object to store all your trinkets

		public EventSystemEventArgs(object data)
		{
			this.data = data;
		}
	}
	

	// Create events, register callbacks and raise events to run all callbacks
	public static class EventSystem
	{		
		// List to hold all events <eventName, eventHandler>
		private static Dictionary<string, EventSystemEventHandler> eventList = new Dictionary<string, EventSystemEventHandler>();
		private static Dictionary<string, EventSystemEventHandler> pendingEventList = new Dictionary<string, EventSystemEventHandler>();  // kind of like failed events
		
		
		// Add an event to the list
		public static void CreateEvent(string eventName, EventSystemEventHandler handler)
		{
			if (eventList.ContainsKey(eventName) == false)
			{
				eventList.Add(eventName, handler);				
			}

			// Try to add any failed events from the pending list to the proper event list
			// It's run here incase the new eventName coming in matches a pending event
			for (int i = 0; i < pendingEventList.Count; i++)
			{				
				string key = pendingEventList.Keys.ElementAt(i);
				if (RegisterCallbackOnEvent(key, pendingEventList[key]) == true)
					pendingEventList.Remove(key);			
			}
		}
				

		// Add a callback to an event
		public static bool RegisterCallbackOnEvent(string eventName, EventSystemEventHandler callback)
		{			
			// Add callback to event
			if (eventList.ContainsKey(eventName) == true && eventList[eventName] == null)
			{				
				eventList[eventName] += callback;
				return true;
			}

			// Add event to pending event list, try to add again in CreateEvent
			else
			{
				if (pendingEventList.ContainsKey(eventName) == false)
					pendingEventList.Add(eventName, callback);

				return false;
			}
		}
		

		// If using ICommand this is not required as ICommand calls this
		// Raise an event, which runs all registered callbacks for that event
		public static void RaiseEvent(string eventName, object sender = null, EventSystemEventArgs e = null)
		{
			if (eventList.ContainsKey(eventName) == true && eventList[eventName] != null)
			{				
				eventList[eventName](sender, e);

				// TODO : check this does something
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
			}
		}
	}	

}
