using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private enum UIState { NotJoined, Joined, Ready}
	private List<UIState> currentState = new List<UIState>();
	public UIPlayerManager Player1;
	public UIPlayerManager Player2;
	public UIPlayerManager Player3;
	public UIPlayerManager Player4;
	// Use this for initialization
	void Start () 
	{	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			switch(currentState[0])
			{
				case UIState.NotJoined:
					currentState.Insert(0, UIState.Joined);
					Debug.Log("UIState for 0: " + currentState[0]);
					break;
				case UIState.Joined:
					currentState.Insert(0, UIState.Ready);
					Debug.Log("UIState for 0: " + currentState[0]);
					break;
				case UIState.Ready:
					break;
			}
		}
		//Dpad controls for Player 1
		if(Input.GetKeyDown(KeyCode.Q) && currentState[0] == UIState.Joined)
		{
			Debug.Log("Dpad in Joined for 0: " + currentState[0]);
			var playerSelect = this.transform.FindChild("PlayerSelect");
			var player1 = playerSelect.transform.FindChild("Player1");
			var mechImage = player1.transform.FindChild("MechPreview");
			mechImage.GetComponent<Image>().color = new Color(0f,0f,0f);
		}
	
	}
}
