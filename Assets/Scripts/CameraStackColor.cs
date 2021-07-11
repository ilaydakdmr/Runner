using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStackColor : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float deltaZ;
    void Start()
    {
        deltaZ = transform.position.z - target.position.z;
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + deltaZ);
    }

    public void SetTarget(Transform parentPickup)
    {
        throw new System.NotImplementedException();
    }
}
