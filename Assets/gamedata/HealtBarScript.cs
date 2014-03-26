using UnityEngine;
using System.Collections;

public class HealtBarScript : MonoBehaviour {

    public float hp;

	public GameObject myHealtBar;
    
	private float maxhp;
	private float healtBarMaxSize;
	private int healtBarWidth;

	private GameObject myhb;
	private float offsetY = 0.1f;

	private bool visible = true;

	void Start () {

		healtBarWidth = (int)healtBarMaxSize;
		myhb = (GameObject)Instantiate(myHealtBar,transform.position,transform.rotation) as GameObject;
        maxhp = 100.0f;
		healtBarMaxSize = 200.0f;
	}

	public void Update () {

		myhb.transform.position = Camera.main.WorldToViewportPoint(transform.position);
		Vector3 pos = myhb.transform.position;

		pos.x -= 0.1f;
		pos.y += 0.05f + offsetY;

		myhb.transform.position = pos;
		myhb.transform.localScale = Vector3.zero;

		float healtPercentage = hp/maxhp;
		if(healtPercentage < 0.0f) { healtPercentage = 0.0f; }
		if(healtPercentage > 100.0f ) { healtPercentage = 100.0f; }
	
		healtBarWidth = (int)(healtPercentage * healtBarMaxSize);

		myhb.guiTexture.color = new Color( (55.0f + ( healtBarMaxSize - healtBarWidth )) / 255.0f, (32.0f + healtBarWidth) / 255.0f , 32.0f / 255.0f, 1.0f);

		if(visible){

			myhb.guiTexture.pixelInset = new Rect(10, 10, healtBarWidth, 10);
		}
		else{

			myhb.guiTexture.pixelInset = new Rect(0, 0, 0, 0);
		}

 	}

	public void OnVisible()
	{
		visible = true;
	}

	public void OnHidden()
	{
		visible = false;
	}
}
