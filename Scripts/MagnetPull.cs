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
  
  //Number of records to hold on the magnet angle and magnitude. The script compares the oldest record 
  //with the newest record in order to detect a magnet pull or release event. Note that the records are 
  //cleared after each event. If set too high, the data set will take more time to build and might miss 
  //some events. If set too low, it will detect only very small changes and might never trigger the
  //events. This, of course, is all relative to the value of dataGatheringFrequency.
  int dataSetLength = 4; 
  
  //Frequency of recording measurments. 
  float dataGatheringFrequency = 0.06f;
  
  //NOTE : The following treshold values are optimal for the device and the geo-localisation this was 
  //       tested (LG Nexus 5, Montréal)
    
  //Minimum angle variation to trigger an event
  float angleDetectionTreshold = 9.0f;
  
  //Minimum magnitude variation to trigger an event
  float magnitudeDetectionTreshold = 70.0f;
  
  List<Vector3> readings = new List<Vector3>();
  
  Vector3 actualVector;
 
  float diffMagnitude;
  
  float diffAngle;
    
  //Stays true while magnet is pulled. 
  [HideInInspector]
  public bool isMagnetPulled;
  
  //Stays true for one frame (event-like).
  [HideInInspector]
  public bool magnetPulled;
  [HideInInspector]
  public bool magnetReleased;
  [HideInInspector]
  public bool magnetTriggered;
  
  [HideInInspector]
  //Time of the pull. It keeps the value after the release event. Will be reset upon a new pull.
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
    
    //Here, we use the default Google Cardboard triggered event to make sure everything is synchronized. 
    //For example : if the has no intro and starts right away, a false pull is sometimes detected when 
    //putting the device inside the cardboard. A quick pull/release will synchronize everything. 
    if (Cardboard.SDK.Triggered) {
      if (isMagnetPulled && !magnetJustReleased) {
        //A pull was detected before the trigger, let's release. 
        magnetJustReleased = true;
        isMagnetPulled = false;
        readings.Clear();
      }
    }
    
    if (isMagnetPulled) {
      timePulled += Time.deltaTime;
    }
    
    //Used in editor only to simulate magnet
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
    //If data set is full
    if(count >= (dataSetLength)) {
      readings.RemoveAt(0);      
       readings.Add(actualVector);
      //Calculate magnitude difference between first and last vector in data set
      diffMagnitude = readings[(dataSetLength - 1)].magnitude - readings[0].magnitude;
      //Calculate angle between first and last vector in data set
      diffAngle = Vector3.Angle(readings[(dataSetLength - 1)], readings[0]);
      
      //When the magnet moves, both the angle and the magnitude of the compass rawVector
      //varies. Knowing that, to detect a change of the magnet position, we start by checking
      //if there is a notable angle variation and then we validate by making sure there is
      //also a notable magnitude variation.
      if (diffAngle >= angleDetectionTreshold) {
        if (isMagnetPulled) {
          //If magnet is released, magnitude variation should be negative.
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
    //Here, we use late update to make sure magnetPulled, magnetReleased and magnetTriggered stays true 
    //for one frame only but all frame long. 
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