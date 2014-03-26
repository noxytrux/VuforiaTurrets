using UnityEngine;
using System.Collections;

public class BulletLogic : MonoBehaviour {

	// Use this for initialization

	public GameObject explosionPrefab;
	public GameObject smokeExplosionPefab;

	private Vector3 acc;
	private Vector3 freeFall;

	private Vector3 lPos;
	private Vector3 dir;
	private float OY;
	private float OX;
	private float balisticRotation;
	private float lifeTime = 0.0f;

	Quaternion quaternion1;
	Quaternion quaternion2;
	Quaternion quaternion3;
	
	void Start () {

		balisticRotation = 0.0f;
		acc = transform.right * 800.0f;
		freeFall = -transform.up;
	}
	
	// Update is called once per frame
	void Update () {

		lifeTime += Time.deltaTime;

		transform.position += acc * Time.deltaTime; 

		//our simplified free fall formula (remember that UP is your camera facing!)
		acc += freeFall * Time.deltaTime * 700.0f;

		balisticRotation += Time.deltaTime * 10.0f;

		dir = transform.position - lPos;

		lPos = transform.position;

		dir.Normalize();
		
		OY = Mathf.Atan2( -dir.z , dir.x ) * Mathf.Rad2Deg;
		OX = (Mathf.Atan2(  dir.y , Mathf.Sqrt( dir.x * dir.x + dir.z * dir.z ) ) + Mathf.PI) * Mathf.Rad2Deg;
		
		quaternion1 = Quaternion.Euler( 0.0f, OY, 0.0f ) ;
		quaternion2 = Quaternion.Euler( 0.0f, 0.0f, OX);
		quaternion3 = Quaternion.Euler( balisticRotation * Mathf.Rad2Deg, 0.0f, 0.0f ) ;

		transform.rotation = quaternion1 * quaternion2 * quaternion3;;

		if( lifeTime > 20.0f ){

			Destroy(this.gameObject);
		}

	}

	void OnCollisionEnter(Collision collision) {

		if( ( collision.gameObject.name == "RedPlane" ) || 
		    ( collision.gameObject.name == "BluePlane" ) ) {

			Debug.Log("Skip collision");
		}
		else{

			DefaultTrackableEventHandler handler = collision.gameObject.transform.root.gameObject.GetComponent<DefaultTrackableEventHandler>();
			handler.ApplyDamage();
		}

		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

		Instantiate(explosionPrefab, transform.position, rot);
		Instantiate(smokeExplosionPefab, transform.position, rot);
		
		Destroy(this.gameObject);
	}
}
