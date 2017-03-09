using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aClass
{
	public float aFloat;
	public void aFunc1(params object[] args)
	{
		Debug.Log ("aFunc1");
	}

	public void aFunc2(params object[] args)
	{
		Debug.Log ("aFunc2");
	}

}

public delegate void MyDelegate (params object[] args);

public class EventManagerTester : MonoBehaviour 
{
	aClass aObject;

	public EventManager thePublicEventManager;

	MyDelegate d1;
	MyDelegate d2;
	Dictionary <string, MyDelegate> eventDictionary;

	// Use this for initialization
	void Start () 
	{
		aObject = new aClass ();
		aObject.aFloat = 0.345f;

		d1 = new MyDelegate (SimpleFunc1);
		//Debug.Log("<color=magenta>d1:name: "+d1.Method.Name+"</color>");
		//Debug.Log("<color=magenta>d1:Hash: "+d1.GetHashCode()+"</color>");
		d1.Invoke ();
		d1 = SimpleFunc2;
		Debug.Log("<color=magenta>d1:name: "+d1.Method.Name+"</color>");
		Debug.Log("<color=magenta>d1:Hash: "+d1.GetHashCode()+"</color>");

		d1.Invoke ();
		d1 += SimpleFunc1;
		Debug.Log("<color=magenta>d1:name: "+d1.Method.Name+"</color>");
		Debug.Log("<color=magenta>d1:Hash: "+d1.GetHashCode()+"</color>");
		Debug.Log("<color=magenta>d1:InvocationList:Name: "+d1.GetInvocationList()[0].Method.Name+"</color>");
		Debug.Log("<color=magenta>d1:InvocationList:Hash: "+d1.GetInvocationList()[0].GetHashCode()+"</color>");
		d1 ();
		d1 -= SimpleFunc1;
		d1 ();

		eventDictionary = new Dictionary<string, MyDelegate>();
		AddToDictionary ("SimpleEvent1", aObject.aFunc1);
		AddToDictionary ("SimpleEvent1", aObject.aFunc2);
		TriggerEvent ("SimpleEvent1");
		SubtractFromDictionary ("SimpleEvent1", aObject.aFunc1);
		TriggerEvent ("SimpleEvent1");
		EventManager.VariableParamAction vpa1 = new EventManager.VariableParamAction(SimpleFunc1);
		EventManager.VariableParamAction vpa2 = new EventManager.VariableParamAction(SimpleFunc2);

		Debug.Log("<color=grey>The Public Event Manager</color>");
		EventManager.StartListening ("SimpleEvent2",  SimpleFunc1);
		EventManager.StartListening ("SimpleEvent2", aObject.aFunc2);
		EventManager.TriggerEvent ("SimpleEvent2");
		EventManager.StopListening ("SimpleEvent2", SimpleFunc1);
		EventManager.TriggerEvent ("SimpleEvent2");
		EventManager.StopListening ("SimpleEvent2", aObject.aFunc2);
		EventManager.TriggerEvent ("SimpleEvent2");
		EventManager.StartListening ("SimpleEvent2",  SimpleFunc1);
		EventManager.TriggerEvent ("SimpleEvent2");
	}

	void AddToDictionary(string eventName, MyDelegate listener)
	{
		Debug.Log("<color=brown>DICTIONARY ADDTITION</color>");

		MyDelegate thisDelegate;
		if (eventDictionary.TryGetValue (eventName, out thisDelegate)) {
			thisDelegate += listener;
			eventDictionary [eventName] = thisDelegate;
		} else 
		{
			eventDictionary.Add (eventName, listener);
		}


	}

	void SubtractFromDictionary(string eventName, MyDelegate listener)
	{
		Debug.Log("<color=brown>SUBTRACTING</color>");
		MyDelegate thisDelegate2;
		if (eventDictionary.TryGetValue (eventName, out thisDelegate2))
		{
			thisDelegate2 -= listener;
			eventDictionary[eventName] = thisDelegate2;
		} 
	}

	void TriggerEvent(string eventName)
	{
		eventDictionary[eventName]();
	}

	void CheckDictionary(string eventName, MyDelegate listener,MyDelegate listener2)
	{
		
	}

	public void SimpleFunc1(params object[] args)	
	{
		Debug.Log ("SimpleFunc1");
	}

	public void SimpleFunc2(params object[] args)
	{
		Debug.Log ("SimpleFunc2");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI ()
	{
		if(GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.2f, Screen.height * 0.1f ), "Add Event1"))
		{
			EventManager.StartListening("Test1", TestFunc);
			EventManager.StartListening("Test1", TestFunc2);
		}

		if(GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.25f, Screen.width * 0.2f, Screen.height * 0.1f ), "Fire Event1"))
		{
			EventManager.TriggerEvent("Test1", 1, "abc123", aObject);
		}

		if(GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.5f, Screen.width * 0.2f, Screen.height * 0.1f ), "Fire Event2"))
		{
			EventManager.TriggerEvent("Test1", 1);
		}

		if(GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.65f, Screen.width * 0.2f, Screen.height * 0.1f ), "Remove Listener 1"))
		{
			EventManager.StopListening("Test1", TestFunc);
		}
	}

	public void TestFunc(params object[] args)
	{
		Debug.Log ("<color=green>TestFunc</color>");
		Debug.Log ("Got some Args: count: " + args.Length);
		for (int i = 0; i < args.Length; i++) 
		{
			
			Debug.Log ("Got some Args: value: " + args[i]);

			if (args [i].GetType () == typeof(aClass)) 
			{
				Debug.Log ("User Defined Type: WOOOHOO: " + ((aClass)args[i]).aFloat);
			}

		}
	}

	public void TestFunc2(params object[] args)
	{
		Debug.Log ("<color=orange>TestFunc2</color>");
		Debug.Log ("<color=orange>Got some Args: count: " + args.Length+"</color>");
		for (int i = 0; i < args.Length; i++) 
		{

			Debug.Log ("Got some Args: value: " + args[i]);

			if (args [i].GetType () == typeof(aClass)) 
			{
				Debug.Log ("User Defined Type: WOOOHOO: " + ((aClass)args[i]).aFloat);
			}

		}
	}

}
