using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Contracts;

public class UIPlayerManager : MonoBehaviour {
	private enum UIState { NotJoined, Joined, Ready }
	private UIState currentState =UIState.NotJoined;
	public ICharacter character;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			switch (currentState)
			{
				case UIState.NotJoined:
					currentState = UIState.Joined;
					Debug.Log("UIState for 0: " + currentState);
					break;
				case UIState.Joined:
					currentState = UIState.Ready;
					Debug.Log("UIState for 0: " + currentState);
					break;
				case UIState.Ready:
					break;
			}
		}
		//Dpad controls for Player 1
		if (Input.GetKeyDown(KeyCode.Q) && currentState == UIState.Joined)
		{
			Debug.Log("Dpad in Joined for 0: " + currentState);
			var playerSelect = this.transform.FindChild("PlayerSelect");
			var player1 = playerSelect.transform.FindChild("Player1");
			var mechImage = player1.transform.FindChild("MechPreview");
			mechImage.GetComponent<Image>().color = new Color(0f, 0f, 0f);
		}
	
	}
}
