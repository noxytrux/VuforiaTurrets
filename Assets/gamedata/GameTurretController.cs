using UnityEngine;
using System.Collections;

public class GameTurretController : MonoBehaviour {

	public Transform gunTransform;
	public GameObject bulletPrefab;
	
	public void FireBullet () {

		GameObject bullet = (GameObject)Instantiate( bulletPrefab, 
		                                        	 gunTransform.position + gunTransform.right * 300.0f, 
		                                         	 gunTransform.rotation );
		
		if(bullet.collider) {

			Physics.IgnoreCollision(gunTransform.collider, bullet.collider);
		}
	
	}
	
}
