using UnityEngine;
using System.Collections;

public class GameTurretController : MonoBehaviour {


	public Transform gunTransform;
	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void FireBullet () {

		GameObject bullet = (GameObject)Instantiate( bulletPrefab, 
		                                        	 gunTransform.position + gunTransform.right * 300.0f, 
		                                         	 gunTransform.rotation );
		
		if(bullet.collider) {

			Physics.IgnoreCollision(gunTransform.collider, bullet.collider);
		}
	
	}
}
