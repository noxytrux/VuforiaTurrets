using UnityEngine;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour, IVirtualButtonEventHandler {

	// Use this for initialization
	void Start () {
	
		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		
		for (int i = 0; i < vbs.Length; ++i)
		{
			vbs[i].RegisterEventHandler(this);
			Debug.Log("Registered:" + vbs[i].VirtualButtonName);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnButtonPressed (VirtualButtonAbstractBehaviour vb)
	{
		VirtualButtonBehaviour vbc = vb as VirtualButtonBehaviour;

		Debug.Log("Button pressed! " + vbc.VirtualButtonName);
		
	}
	
	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb)	
	{
		VirtualButtonBehaviour vbc = vb as VirtualButtonBehaviour;

		Debug.Log("Button released! " + vbc.VirtualButtonName);
		
	}
}
