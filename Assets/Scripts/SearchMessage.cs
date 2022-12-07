using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchMessage : MonoBehaviour
{
    public float movingSpeed;
    private RectTransform myRectTransform;
    private Color normalColor = new Color(83.0f/255.0f, 255.0f/255.0f, 84.0f/255.0f, 156.0f/255.0f);
    private Color stopColor = new Color(255.0f/255.0f, 83.0f/255.0f, 84.0f/255.0f, 156.0f/255.0f);
    private bool activeSearch = true;

    void Start()
    {   
       if (this.movingSpeed <= 0){
        this.movingSpeed = 0.5f; // in case of invalid input
       }
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

    void Update()
    {
        if (activeSearch){
            myRectTransform.localPosition += new Vector3(this.movingSpeed, 0, 0);
            if (!RectContainsAnother(GameObject.Find("WaveButton").GetComponent<RectTransform>(), myRectTransform)){
                this.movingSpeed *= -1;
                myRectTransform.localPosition += new Vector3(2*this.movingSpeed, 0, 0);  // bounce back immediately
            }
        }
    }

    public void stopSearch(){
        this.activeSearch = false;
        gameObject.GetComponent<Image>().color = stopColor;
    }
    
    public void startSearch(){
        this.activeSearch = true;
        gameObject.GetComponent<Image>().color = normalColor;
    }

    public void setSearch(bool searchBool){
        if(searchBool){
            startSearch();
        } else {
            stopSearch();
        }
    }

    public int getTime(){
        return GameObject.Find("DailyTimer").GetComponent<Timer>().getCurrentMinutes();
    }

}
