using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    [SerializeField] private float multiplierValue;
    [SerializeField] private Color multiplierColor;
    [SerializeField] private Renderer[] myRends;
    
    void Start()
    {
        SetColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name+"hit");
        if (collision.transform.tag=="PickUP")
        {
            GameController.instance.UpdateMultiplier(multiplierValue);
        }
    }

    void SetColor()
    {
        for (int i = 0; i < myRends.Length; i++)
        {
            myRends[i].material.SetColor("_Color",multiplierColor);
        }
    }
}
