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

		Debug.Log(gameObject.name + " Shooting!");


		GameObject bullet = (GameObject)Instantiate(bulletPrefab, gunTransform.position + gunTransform.forward * 30.0f, gunTransform.rotation);
		
		if(bullet.collider) {
			Physics.IgnoreCollision(gunTransform.collider, bullet.collider);
		}
		
		bullet.rigidbody.AddRelativeForce(gunTransform.forward * 400.0f);
	}
}
