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

		Debug.Log("Button pressed! " + vbc.VirtualButtonName);

		GameTurretController ctr = currentTurret.GetComponent<GameTurretController>();

		ctr.FireBullet();
	}
	
	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb)	
	{
		VirtualButtonBehaviour vbc = vb as VirtualButtonBehaviour;

		Debug.Log("Button released! " + vbc.VirtualButtonName);
		
	}
}
