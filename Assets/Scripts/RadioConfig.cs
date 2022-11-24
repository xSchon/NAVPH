using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioConfig{
    private int radioNumber;
    private Color backgroundColor;
    private float posXSearcher;
    private string radioMessage;
    private bool activeSearch;

    public RadioConfig(int radioNumber, Color backgroundColor){
        this.radioNumber = radioNumber;
        this.backgroundColor = backgroundColor;
        this.posXSearcher = 0.0f;
        this.radioMessage = "";
        this.activeSearch = true;
    }

    public int getNumber(){
        return this.radioNumber;
    }

    public Color getColor(){
        return this.backgroundColor;
    }

    public void setPosX(float newPosX){
        this.posXSearcher = newPosX;
    }

    public float getPosX(){
        return this.posXSearcher;
    }

    public void setMessage(string newMessage){
        this.radioMessage = newMessage;
    }

    public string getMessage(){
        return this.radioMessage;
    }

    public bool isActive(){
        return this.activeSearch;
    }

    public void setActive(bool newActive){
        this.activeSearch = newActive;
    }
}