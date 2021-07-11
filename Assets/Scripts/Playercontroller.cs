using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] private Color myColor;
    [SerializeField] private Renderer[] myRends;
    [SerializeField] private bool isPlaying;
    [SerializeField] private float forwardSpeed;
    private Rigidbody myRB;
    [SerializeField] private float lerpSpeed;
    private Transform parentPickup;
    [SerializeField] private Transform stackposition;
    private bool atEnd;

    [SerializeField] private float forwardForce;
    [SerializeField] private float forceAdder;
    [SerializeField] private float forceReducer;

    public static Action<float> Kick;
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        SetColor(myColor);
    }

    private void Update()
    {
        if (isPlaying)
        {
            MoveForward();
        }

        if (atEnd)
        {
            forwardForce -= forceReducer * Time.deltaTime;
            if (forwardForce < 0)
                forwardForce = 0;
        }

        if (Input.GetMouseButton(0))
        {
            if (atEnd)
            {
                forwardForce += forceAdder;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (atEnd)
                return;
            if (isPlaying==false)
            {
                isPlaying = true;
            }
            MoveSideways();
        }
    }


    void SetColor(Color colorIn)
    {
        myColor = colorIn;
        for (int i = 0; i < myRends.Length; i++)
        {
            myRends[i].material.SetColor("_Color",myColor);
        }
    }

    void MoveForward()
    {
        myRB.velocity = Vector3.forward * forwardSpeed;
    }

    void MoveSideways()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100))
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(hit.point.x, transform.position.y, transform.position.z), lerpSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="ColorWall")
        {
            SetColor(other.GetComponent<ColorWall>().GetColor());
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="FinishLineStart")
        {
            atEnd = true;
        }

        if (other.tag=="FinishLineEnd")
        {
            myRB.velocity=Vector3.zero;
            isPlaying = false;
            //Debug.Log(forwardForce+"forward force");
            LaunchStack();
        }

        if (atEnd)
            return;
        
        Debug.Log(other.tag);
        if (other.tag=="PickUP")
        {
            Transform otherTransform = other.transform.parent;

            if (myColor == otherTransform.GetComponent<PickUpStack>().GetColor())
            {
                GameController.instance.UpdateScore(otherTransform.GetComponent<PickUpStack>().value);


            }
            else
            {
                GameController.instance.UpdateScore(otherTransform.GetComponent<PickUpStack>().value * -1);
                Destroy(other.gameObject);
                if (parentPickup != null)
                {
                    if (parentPickup.childCount > 1)
                    {
                        parentPickup.position -=
                            Vector3.up * parentPickup.GetChild(parentPickup.childCount - 1).localScale.y;
                        Destroy(parentPickup.GetChild(parentPickup.childCount - 1).gameObject);
                    }
                    else
                    {
                        Destroy(parentPickup.gameObject);
                    }
                }
                return;
            }
            Rigidbody otherRB = otherTransform.GetComponent<Rigidbody>();
            otherRB.isKinematic = true;
            other.enabled = false;
            if (parentPickup==null)
            {
                parentPickup = otherTransform;
                parentPickup.position = stackposition.position;
                parentPickup.parent = stackposition;
            }
            else
            {
                parentPickup.position += Vector3.up * (otherTransform.localScale.y);
                otherTransform.position = stackposition.position;
                otherTransform.parent = stackposition;
            }
        }
    }

    void LaunchStack()
    {
        Camera.main.GetComponent<CameraStackColor>().SetTarget(parentPickup);
        Kick(forwardForce);
    }
}
