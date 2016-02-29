using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour {
  public int minTextSize = 10;
  public int maxTextSize = 18;
  Text objectTextComponent;
	// Use this for initialization
	void Start () {
    objectTextComponent = gameObject.GetComponent<Text>();
    objectTextComponent.fontSize = minTextSize;
    StartCoroutine("animateText");
	}
	
  IEnumerator animateText() {
    bool growing = true;
    bool shrinking = false;
    while (growing) {
      objectTextComponent.fontSize += 1;
      if (objectTextComponent.fontSize >= maxTextSize) {
        growing = false;
        shrinking = true;
      }
      yield return new WaitForSeconds(0.01f);
    }    
    yield return new WaitForSeconds(0.2f);
    while (shrinking) {
      objectTextComponent.fontSize -= 1;
      if (objectTextComponent.fontSize <= minTextSize) {
        shrinking = false;
      }
      yield return new WaitForSeconds(0.01f);
    }
    Destroy(gameObject);
    
  }
}
