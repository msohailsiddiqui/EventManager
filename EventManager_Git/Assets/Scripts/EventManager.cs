using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour 
{

	public delegate void VariableParamAction (params object[] args);
	private Dictionary <string, VariableParamAction> eventDictionary;

	private static EventManager eventManager;

	public static EventManager instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

				if (!eventManager)
				{
					Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
				}
				else
				{
					eventManager.Init (); 
				}
			}

			return eventManager;
		}
	}

	void Init ()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, VariableParamAction>();
		}
	}

	public static void StartListening (string eventName, VariableParamAction listener)
	{
		VariableParamAction thisDelegate;
		//if (instance.eventDictionary.TryGetValue (eventName, out thisDelegate))
		if (instance.eventDictionary.ContainsKey (eventName))
		{
			
			Debug.Log("<color=brown>BEFORE added to existing Delegate, list count: "+instance.eventDictionary[eventName].GetInvocationList ().Length+"</color>");
			instance.eventDictionary[eventName] += listener;
			if (instance.eventDictionary[eventName].GetInvocationList () [1] == listener) {
				Debug.Log ("<color=brown> Found Listener</color>");
			}
			else 
			{
				Debug.Log ("<color=brown> Unknown Listener</color>");
			}
			//instance.eventDictionary[eventName] -= listener;
			Debug.Log("<color=brown>AFTER added to existing Delegate, list count: "+instance.eventDictionary[eventName].GetInvocationList ().Length+"</color>");
			//instance.eventDictionary[eventName] = thisDelegate;
//			if (instance.eventDictionary.TryGetValue (eventName, out thisDelegate)) 
//			{
//				Debug.Log("<color=yellow>Checking from dictionary, list count: "+thisDelegate.GetInvocationList ().Length+"</color>");
//					
//			}
		} 
		else
		{
			thisDelegate = new VariableParamAction (listener);
			//Debug.Log("<color=blue>Created New Delegate, list count: "+thisDelegate.GetInvocationList ().Length+"</color>");
			instance.eventDictionary.Add (eventName, listener);
		}
	}

	public static void StopListening (string eventName, VariableParamAction listener)
	{

		if (eventManager == null) return;
		VariableParamAction thisDelegate = null;
		//if (instance.eventDictionary.TryGetValue (eventName, out thisDelegate))
		if (instance.eventDictionary.ContainsKey (eventName))
		{
			Debug.Log("<color=yellow>Before removing, list count: "+instance.eventDictionary[eventName].GetInvocationList ().Length+":EventName:"+eventName+"</color>");
			if (instance.eventDictionary[eventName].GetInvocationList () [0] == listener ) 
			{
				Debug.Log ("<color=yellow> Found Listener</color>");
			}
			else 
			{
				Debug.Log ("<color=yellow> Unknown Listener</color>");
			}
			instance.eventDictionary[eventName] -= listener;
			if (instance.eventDictionary [eventName] == null) 
			{
				Debug.Log ("<color=yellow>After removing, list null::EventName:" + eventName + "</color>");
				instance.eventDictionary.Remove (eventName);
			} else {
				Debug.Log ("<color=yellow>After removing, list count: " + instance.eventDictionary [eventName] + ":EventName:" + eventName + "</color>");
			}
		}
	}

	public static void TriggerEvent (string eventName, params object[] args)
	{
		if (instance.eventDictionary.ContainsKey (eventName))
		{
//			Debug.Log("<color=green>Triggering "+ eventName+", list count: "+thisDelegate.GetInvocationList ().Length+"</color>");
			//thisDelegate.Invoke (args);
			instance.eventDictionary[eventName].Invoke(args);
		}
	}
}