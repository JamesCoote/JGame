using UnityEngine;
using System.Collections;

public class splash_script : MonoBehaviour {

	public Texture logo;

	private Color guiColor;
	
	void Start(){
		guiColor = Color.white;
	}
		
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup>4.0f){
			Application.LoadLevel ("main scene");
		}else if(Time.realtimeSinceStartup<1.0f){
			guiColor.a = Mathf.Lerp(0.0f, 1.0f, Time.realtimeSinceStartup / 1.0f);
		} else if(Time.realtimeSinceStartup>3.0f){
			guiColor.a = Mathf.Lerp(1.0f, 0.0f, Time.realtimeSinceStartup - 3.0f);
		}
	}

	void OnGUI(){

		GUI.color = guiColor;

		GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width/2,
		                         Screen.height / 2 - logo.height/2,
		                         logo.width,
		                         logo.height),logo);

		GUI.color = Color.white;

	}
}
