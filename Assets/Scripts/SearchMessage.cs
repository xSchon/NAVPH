/* Serves for moving search bar in the radio and stopping it. */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchMessage : MonoBehaviour
{
    public RectTransform myRectTransform;
    public RectTransform waveClickedReactTransform;
    public float movingSpeed = 0.5f;
    private Color normalColor = new Color(83.0f / 255.0f, 255.0f / 255.0f, 84.0f / 255.0f, 156.0f / 255.0f);
    private Color stopColor = new Color(255.0f / 255.0f, 83.0f / 255.0f, 84.0f / 255.0f, 156.0f / 255.0f);
    private bool activeSearch = true;

    void Update()
    {
        if (activeSearch)
        {
            myRectTransform.localPosition += new Vector3(this.movingSpeed, 0, 0);
            if (!RectContainsAnother(waveClickedReactTransform, myRectTransform))
            {
                this.movingSpeed *= -1;
                myRectTransform.localPosition += new Vector3(2 * this.movingSpeed, 0, 0);  // bounce back immediately
            }
        }
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        /*
        Following two functions, serving for calculating if one GUI element is in the other,
        are mainly consisting of the code from the discussion on Unity forum:
        http://answers.unity.com/answers/1710725/view.html.
        I made changes to this code, however I am not a creator of big parts of GetWorldRect and
        RectContainsAnother functions. 
        */
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector2 min = corners[0];
        Vector2 max = corners[2];
        Vector2 size = max - min;

        return new Rect(min, size);
    }

    private bool RectContainsAnother(RectTransform rct, RectTransform another)
    {
        Rect r = GetWorldRect(rct);
        Rect a = GetWorldRect(another);
        return r.xMin <= a.xMin && r.xMax >= a.xMax;
    }

    public void StopSearch()
    {
        this.activeSearch = false;
        GetComponent<Image>().color = stopColor;
    }

    public void StartSearch()
    {
        this.activeSearch = true;
        GetComponent<Image>().color = normalColor;
    }

    public void SetSearch(bool searchBool)
    {
        if (searchBool)
        {
            StartSearch();
        }
        else
        {
            StopSearch();
        }
    }
}
