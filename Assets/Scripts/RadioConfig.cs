/* Class storing configuration of a radio and information about it */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioConfig
{
    private int radioNumber;
    private Color backgroundColor;
    private float posXSearcher;
    private bool activeSearch;
    private string[] radioArray;
    private string author;

    public RadioConfig(int radioNumber, Color backgroundColor)
    {
        this.radioNumber = radioNumber;
        this.backgroundColor = backgroundColor;
        this.posXSearcher = 0.0f;
        this.activeSearch = true;
    }

    public Color GetColor()
    {
        return this.backgroundColor;
    }

    public void SetPosX(float newPosX)
    {
        this.posXSearcher = newPosX;
    }

    public float GetPosX()
    {
        return this.posXSearcher;
    }

    public bool IsActive()
    {
        return this.activeSearch;
    }

    public void SetActive(bool newActive)
    {
        this.activeSearch = newActive;
    }

    public void SetRadioArray(string[] newArray)
    {
        this.radioArray = newArray;
    }

    public string[] GetRadioArray()
    {
        return this.radioArray;
    }

    public void SetAuthor(string newAuthor)
    {
        this.author = newAuthor;
    }

    public string GetAuthor()
    {
        return this.author;
    }
}