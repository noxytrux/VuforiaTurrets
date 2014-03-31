using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
	public Texture backgroundTex;
	public Texture leftUpperCorner;
	public Texture rightBottomCorner;
	public Texture nameTexture;

	public GUIStyle style;

	private float maxWidht = 0.0f;
	private float maxHeight = 0.0f;

	void Start() {

		maxWidht = Screen.width;
		maxHeight = Screen.height;
	}

	void OnGUI()
	{
		GUI.DrawTexture (new Rect (0,0,maxWidht,maxHeight), backgroundTex);
		GUI.DrawTexture (new Rect (0,0,512,336), leftUpperCorner);
		GUI.DrawTexture (new Rect (maxWidht - 640,maxHeight - 364.0f,640,364), rightBottomCorner);
		GUI.DrawTexture (new Rect (60,60,500,108), nameTexture);

		if (GUI.Button(new Rect(maxWidht - 450,maxHeight - 260,408,86), "PLAY", style))
		{
			Application.LoadLevel("mainScene");
		}

		if (GUI.Button(new Rect(maxWidht - 450,maxHeight - 120,408,86), "HELP", style))
		{
			Application.LoadLevel("helpScene");
		}

	}
}
