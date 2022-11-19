using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class SearchMessage : MonoBehaviour
{
    public float movingSpeed;
    private RectTransform myRectTransform;
    // Start is called before the first frame update
    void Start()
    {   
       if (this.movingSpeed <= 0){
        this.movingSpeed = 0.5f; // in case of invalid input
       }
       Debug.Log(gameObject.GetComponent<Image>().color); 
       myRectTransform = GetComponent<RectTransform>();
    }


    /*
    Following two functions, serving for calculating if one GUI element is in the other,
    are mainly consisting of the code from the discussion on Unity forum:
    http://answers.unity.com/answers/1710725/view.html.
    I made changes to this code, however I am not a creator of big parts of GetWorldRect and
    RectContainsAnother functions. 
    */
    private Rect GetWorldRect(RectTransform rectTransform)
     {
         var corners = new Vector3[4];
         rectTransform.GetWorldCorners(corners);
 
         Vector2 min = corners[0];
         Vector2 max = corners[2];
         Vector2 size = max - min;
  
         return new Rect(min, size);
    }
    private bool RectContainsAnother (RectTransform rct, RectTransform another)
    {
        var r = GetWorldRect(rct);
        var a = GetWorldRect(another);
        return r.xMin <= a.xMin && r.xMax >= a.xMax;
    }
    // Update is called once per frame
    void Update()
    {
        myRectTransform.localPosition += new Vector3(this.movingSpeed, 0, 0);
        if (!RectContainsAnother(GameObject.Find("WaveButton").GetComponent<RectTransform>(), myRectTransform)){
            this.movingSpeed *= -1;
            myRectTransform.localPosition += new Vector3(2*this.movingSpeed, 0, 0);  // bounce back immediately
        }
        
    }
}
