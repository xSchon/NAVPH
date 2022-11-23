using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioConfig{
    private int radioNumber;
    private Color backgroundColor;
    private float posYSearcher;
    private string radioMessage;

    public RadioConfig(int radioNumber, Color backgroundColor, float posYSearcher, string radioMessage){
        this.radioNumber = radioNumber;
        this.backgroundColor = backgroundColor;
        this.posYSearcher = posYSearcher;
        this.radioMessage = radioMessage;
    }

    public int getNumber(){
        return this.radioNumber;
    }

    public Color getColor(){
        return this.backgroundColor;
    }

    public void setPosY(float newPosY){
        this.posYSearcher = newPosY;
    }

    public float getPosY(){
        return this.posYSearcher;
    }

    public void setMessage(string newMessage){
        this.radioMessage = newMessage;
    }

    public string getMessage(){
        return this.radioMessage;
    }
}