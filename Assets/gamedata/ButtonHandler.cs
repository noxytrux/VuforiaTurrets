using UnityEngine;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour, IVirtualButtonEventHandler {

	public GameObject currentTurret;

	void Start () {
	
		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		
		for (int i = 0; i < vbs.Length; ++i)
		{
			vbs[i].RegisterEventHandler(this);
			Debug.Log("Registered:" + vbs[i].VirtualButtonName);
		}
	}
	
	void Update () {
	//TODO: add colddown to fire!

	}
	
	public void OnButtonPressed (VirtualButtonAbstractBehaviour vb)
	{
		VirtualButtonBehaviour vbc = vb as VirtualButtonBehaviour;
		DefaultTrackableEventHandler trackableEvent = GetComponent<DefaultTrackableEventHandler>();

		GameTurretController ctr = currentTurret.GetComponent<GameTurretController>();

		Debug.Log("OnButtonPressed() " + vbc.VirtualButtonName);

		if( (vbc.VirtualButtonName == "redShootButton") ||
		    (vbc.VirtualButtonName == "shootButtonBlue") ) {

			if(trackableEvent.isAlive) {
				ctr.FireBullet();
			}
		}
		else  if( (vbc.VirtualButtonName == "blueUpButton") ||
		          (vbc.VirtualButtonName == "redUpButton") ) {
			
			trackableEvent.OnTurretUp();
		}
		else  if( (vbc.VirtualButtonName == "blueDownButton") ||
		          (vbc.VirtualButtonName == "redDownButton") ) {
			
			trackableEvent.OnTurretDown();
		}

	}
	
	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb)	
	{
		VirtualButtonBehaviour vbc = vb as VirtualButtonBehaviour;

		Debug.Log("OnButtonReleased() " + vbc.VirtualButtonName);

		DefaultTrackableEventHandler trackableEvent = GetComponent<DefaultTrackableEventHandler>();

		trackableEvent.StopInteraction();
	}
}
