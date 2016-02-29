/**
 * This pseudo-controller is built to show how to use Cardboard Magnet Extended 
 * Controls (cardboard-magnetpull). It simply reads data from the MagnetPull sdk
 * and updates the UI accordingly
**/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagnetController : MonoBehaviour {
  
  //UI objects
  public GameObject uiPanel;
  public GameObject isMagnetPulled;
  public GameObject eventTextPrefab;
  public Transform eventTextAnchor;
  public GameObject timePulled;
  
  //UI text components
  Text timePulledTxt;
  Text isMagnetPulledTxt;
  
	
	void Start () {
    //Instantiate text components   
	  timePulledTxt = timePulled.GetComponent<Text>();
	  isMagnetPulledTxt = isMagnetPulled.GetComponent<Text>();
    
    Input.compass.enabled = true;
	}
	
	void Update () {
    
    /////////////////////////
    //Read timePulled value//
    /////////////////////////
    float pullTime = MagnetPull.SDK.timePulled;
    timePulledTxt.text = pullTime.ToString() + "s";

    ///////////////////////
    //Read isMagnetPulled//
    ///////////////////////
    bool isPulled = MagnetPull.SDK.isMagnetPulled;
    isMagnetPulledTxt.text = isPulled.ToString();
    
    ///////////////////////////////////
    //Listen to the magnet pull event//
    ///////////////////////////////////
    if (MagnetPull.SDK.magnetPulled) {
      //Do something here
      GameObject newEventText;
      newEventText = Instantiate(eventTextPrefab, eventTextAnchor.localPosition, eventTextAnchor.localRotation) as GameObject;
      newEventText.transform.SetParent(uiPanel.transform, false);
      newEventText.GetComponent<Text>().text = "Magnet Pulled !";
    }
    
    //////////////////////////////////////
    //Listen to the magnet release event//
    //////////////////////////////////////
    if (MagnetPull.SDK.magnetReleased) {
      //Do something here
      GameObject newEventText;
      newEventText = Instantiate(eventTextPrefab, eventTextAnchor.localPosition - new Vector3(0,18,0), eventTextAnchor.localRotation) as GameObject;
      newEventText.transform.SetParent(uiPanel.transform, false);
      newEventText.GetComponent<Text>().text = "Magnet Released !";
    }
	}
}
