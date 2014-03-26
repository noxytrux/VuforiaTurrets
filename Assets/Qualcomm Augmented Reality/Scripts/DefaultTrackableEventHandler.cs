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

	private bool turretUpFlag;
	private bool turretDownFlag;

	private Vector3 lastPosition;
	private float markerDistance;

	public bool isAlive;
	private float currentHealts;

	private const float maxDistanceMovement = 6.0f;

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

		turretDownFlag = false;
		turretUpFlag = false;

		lastPosition = transform.position;
		markerDistance = 0.0f;

		isAlive = true;
		currentHealts = 100.0f;
    }

	void Update()
	{
		if(isAlive) {

			modelTransform.gameObject.GetComponent<HealtBarScript>().hp = currentHealts;

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
							
							TurretUp();
						}
						else {
							
							TurretDown();
						}
						
					}
					
				}
				
			}
			
			markerDistance = Vector3.Distance( lastPosition, transform.position);
			lastPosition = transform.position;
			
			if(turretUpFlag) {
				
				TurretUp();
			}
			
			if(turretDownFlag) {
				
				TurretDown();
			}
		}

	}

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS
	
    #region PUBLIC_METHODS

	public void ApplyDamage() {

		currentHealts -= 10.0f;

		if( currentHealts <= 0.0f ) {

			//TODO: put here some nice explosion

			currentHealts = 0.0f;
			isAlive = false;

			OnTrackingLost();
		}

	}

	public void OnTurretUp() {

		Debug.Log("OnTurretUp() " + markerDistance);

		if( (markerDistance < maxDistanceMovement) && !turretDownFlag ) {
				
			turretUpFlag = true;
		}
		else {
				
			turretUpFlag = false;
		}

	}

	public void OnTurretDown() {

		Debug.Log("OnTurretDown() " + markerDistance);

		if( (markerDistance < maxDistanceMovement) && !turretUpFlag ) {
			
			turretDownFlag = true;
		}
		else{

			turretDownFlag = false;
		}

	}

	public void StopInteraction() {
	
		turretUpFlag = false;
		turretDownFlag = false;
	}

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if ((newStatus == TrackableBehaviour.Status.DETECTED ||
             newStatus == TrackableBehaviour.Status.TRACKED ||
             newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) && isAlive )
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

	void TurretUp() {

		axisAngle = turretTransform.localEulerAngles.z;
		if( axisAngle > 180.0 ) axisAngle -= 360.0f;
		
		if( axisAngle < 20.0f){
			
			turretTransform.Rotate(0,0, kMaxRotationSpeed * Time.deltaTime);
		}
		else{
			
			turretTransform.localEulerAngles = new Vector3(0,0,20.0f);
		}
	}

	void TurretDown() {

		axisAngle = turretTransform.localEulerAngles.z;
		if( axisAngle < 180.0) axisAngle += 360.0f;
		
		if( axisAngle > 340.0f){
			
			turretTransform.Rotate(0,0, -kMaxRotationSpeed * Time.deltaTime);
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

		HealtBarScript healtBar = modelTransform.gameObject.GetComponent<HealtBarScript>();
		healtBar.OnVisible();
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

		HealtBarScript healtBar = modelTransform.gameObject.GetComponent<HealtBarScript>();
		healtBar.OnHidden();
    }

    #endregion // PRIVATE_METHODS
}
