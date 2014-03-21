/*==============================================================================
Copyright (c) 2010-2013 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES
 
    private TrackableBehaviour mTrackableBehaviour;
	private TrackableBehaviour.Status currentStatus;
	public Transform modelTransform;
	public Transform turretTransform;
	
	private Vector2 objectPos;
	private Vector2 touchPos;
	private Vector2 currentPos;
	private float ScreenWidth;

	private float kMaxRotationSpeed = 35.0f;
	private float axisAngle;

    #endregion // PRIVATE_MEMBER_VARIABLES



    #region UNTIY_MONOBEHAVIOUR_METHODS
    
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

		ScreenWidth = Screen.width;
		axisAngle = 0.0f;
    }

	void Update()
	{
		currentStatus = mTrackableBehaviour.CurrentStatus;

		if( (Input.touchCount > 0) && 
		    (currentStatus == TrackableBehaviour.Status.DETECTED ||
		     currentStatus == TrackableBehaviour.Status.TRACKED ||
		     currentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)  )
		{
			//we can proceed with calculation

			if(Input.GetTouch(0).phase == TouchPhase.Began) {
					
				objectPos = Camera.main.WorldToScreenPoint(modelTransform.position);
				touchPos = Input.GetTouch(0).position;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				if(objectPos.x > (ScreenWidth * 0.5f) && touchPos.x > (ScreenWidth * 0.5f) ||
				   objectPos.x < (ScreenWidth * 0.5f) && touchPos.x < (ScreenWidth * 0.5f) ) {

					//both finger and object are on the same side

					currentPos = Input.GetTouch(0).position;
					Vector2 translation = touchPos - currentPos;
				
					if( translation.y < 0 )	{

						axisAngle = turretTransform.localEulerAngles.z;
						if( axisAngle > 180.0 ) axisAngle -= 360.0f;

						Debug.Log("UP ( "+turretTransform.localEulerAngles.z+", "+axisAngle+" )");

						if( axisAngle < 30.0f){

							turretTransform.Rotate(0,0, kMaxRotationSpeed * Time.deltaTime);
						}
						else{

							turretTransform.localEulerAngles = new Vector3(0,0,30.0f);
						}
					}
					else {

						axisAngle = turretTransform.localEulerAngles.z;
						if( axisAngle < 180.0) axisAngle += 360.0f;
					
						Debug.Log("DOWN ( "+turretTransform.localEulerAngles.z+", "+axisAngle+" )");

						if( axisAngle > 330.0f){

							turretTransform.Rotate(0,0, -kMaxRotationSpeed * Time.deltaTime);
						}

					}

				}

			}

		}
		
		
	}

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }
	
    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS


    private void OnTrackingFound()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }


    private void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    }

    #endregion // PRIVATE_METHODS
}
