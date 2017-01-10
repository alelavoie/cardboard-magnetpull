/*
LICENSE
-------

The MIT License (MIT)

Copyright (c) 2016 Alexandre Lavoie

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetPull : MonoBehaviour {
  
  int dataSetLength = 4;   
  float dataGatheringFrequency = 0.06f;  
  float angleDetectionTreshold = 9.0f;  
  float magnitudeDetectionTreshold = 70.0f;
  
  List<Vector3> readings = new List<Vector3>();  
  Vector3 actualVector;
 
  float diffMagnitude;  
  float diffAngle;    
 
  [HideInInspector]
  public bool isMagnetPulled;  
  [HideInInspector]
  public bool magnetPulled;
  [HideInInspector]
  public bool magnetReleased;
  [HideInInspector]
  public bool magnetTriggered;  
  [HideInInspector]
  public float timePulled = 0;
  
  bool magnetJustPulled;
  bool magnetJustReleased;
  bool magnetJustTriggered;
	
  public KeyCode simulateMagnetKey = KeyCode.Space;
  private static MagnetPull sdk = null;
  
  public static MagnetPull SDK {    
    get {
      if (sdk == null) {
        sdk = UnityEngine.Object.FindObjectOfType<MagnetPull>();
      }
      return sdk;
    }    
  }	
	
  void Start () {
	  
    isMagnetPulled = false;
    Input.compass.enabled = true;
    actualVector = Input.compass.rawVector;
    StartCoroutine("compassVariation");
    magnetPulled = false;
	  
  }
	
	
  void Update () {	
	  
    //If the default triggered event is fired, reset everything to ensure synchronism. 
    if (Cardboard.SDK.Triggered) {
      if (isMagnetPulled && !magnetJustReleased) {
        magnetJustReleased = true;
        isMagnetPulled = false;
        readings.Clear();
      }
    }   
	  
    if (isMagnetPulled) {
      timePulled += Time.deltaTime;
    }   
	  
    //Simulate magnet in editor
    if (Input.GetKeyDown(simulateMagnetKey)) {
      magnetJustPulled = true;
      isMagnetPulled = true;
      timePulled = 0;
    }	  
    if (Input.GetKeyUp(simulateMagnetKey)) {
      if (isMagnetPulled) {
        magnetJustReleased = true;
        isMagnetPulled = false;
      }
    }
	  
  }
  
	
  IEnumerator compassVariation() {    
	  
    int count = readings.Count;
    actualVector = Input.compass.rawVector;   
	  
    if(count >= (dataSetLength)) {
	    
      readings.RemoveAt(0);      
      readings.Add(actualVector);
      
      diffMagnitude = readings[(dataSetLength - 1)].magnitude - readings[0].magnitude;     
      diffAngle = Vector3.Angle(readings[(dataSetLength - 1)], readings[0]);
      
      //When the magnet moves, both the angle and the magnitude of the compass rawVector
      //varies. Knowing that, to detect a change of the magnet position, we start by checking
      //if there is a notable angle variation and then we validate by making sure there is
      //also a notable magnitude variation.
      if (diffAngle >= angleDetectionTreshold) {
        if (isMagnetPulled) {
          if (diffMagnitude < -magnitudeDetectionTreshold) {
            magnetJustReleased = true;
            isMagnetPulled = false;
            readings.Clear();
          }      
        } else {
          if (diffMagnitude > magnitudeDetectionTreshold) {
            magnetJustPulled = true;
            isMagnetPulled = true;
            timePulled = 0;
            readings.Clear();
          }          
        }        
      }       
      
      yield return new WaitForSeconds(dataGatheringFrequency);
      StartCoroutine("compassVariation");
    
    } else { //Still building dataset.	    
      readings.Add(actualVector);
      yield return new WaitForSeconds(dataGatheringFrequency);
      StartCoroutine("compassVariation");	    
    }
	  
  }
	
	
  void LateUpdate() {
	  
    if (magnetPulled) {
      magnetPulled = false;
    }
    if (magnetJustPulled) {
      magnetPulled = true;
      magnetJustPulled = false;
    }
    if (magnetReleased) {
      magnetReleased = false;
    }
    if (magnetJustReleased) {
      magnetReleased = true;
      magnetJustReleased = false;
    }    
	  
  }
	
}
