using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStack : MonoBehaviour
{
    [SerializeField] public int value;
    [SerializeField] Color pickUpColor;
    [SerializeField] private Rigidbody pickUpRB;
    [SerializeField] private Collider pickUpCollider;


    private void OnEnable()
    {
        Playercontroller.Kick += MyKick;
    }

    private void OnDisable()
    {
        Playercontroller.Kick -= MyKick;
    }

    private void MyKick(float forceSent)
    {
        transform.parent=null;
        pickUpCollider.enabled = true;
        pickUpRB.isKinematic = false;
        pickUpRB.AddForce(new Vector3(0,forceSent,forceSent));
    }

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color",pickUpColor);
    }

    
    void Update()
    {
        
    }

    public Color GetColor()
    {
        return pickUpColor;
    }
}
