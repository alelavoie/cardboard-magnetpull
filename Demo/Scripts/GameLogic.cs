using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
  public GameObject magnetPullObj;
  public GameObject magnetControllerObj;
  public GameObject canvasMain;
  public GameObject canvasIntro;
  public GameObject rawVectorObj;
  public GameObject introText;
	// Use this for initialization
	void Start () {
    if (Cardboard.SDK.Triggered) {
      
    }
    
    if (Application.isEditor) {
      introText.GetComponent<Text>().text = "Click to start";
    } else {
      introText.GetComponent<Text>().text = "Quickly pull and release the magnet to start";
    }
	}
	
	// Update is called once per frame
	void Update () {
	 if (Cardboard.SDK.Triggered) {
     magnetPullObj.SetActive(true); 
     magnetControllerObj.SetActive(true); 
     canvasMain.SetActive(true); 
     rawVectorObj.SetActive(true); 
     canvasIntro.SetActive(false); 
   }
   
	}
}
