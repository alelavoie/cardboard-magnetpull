/**
 * This script is here only to show the direction of the compass' raw vector in the demo.
**/

using UnityEngine;
using System.Collections;

public class CompassRawVector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 Input.compass.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.compass.rawVector != Vector3.zero) {
      gameObject.transform.forward = Input.compass.rawVector;
    }
	 
	}
}
