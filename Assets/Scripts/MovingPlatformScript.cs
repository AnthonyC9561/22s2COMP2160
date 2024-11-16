using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    private bool movingTowardsEndPosition;

    void Start()
    {
        movingTowardsEndPosition = true;
    }

    private void Update()
    { //If platform reaches start/end point it sets the platform to move in other direction
        if (transform.position == startPosition.position)
        {
            movingTowardsEndPosition = false;
        }
        if (transform.position == endPosition.position)
        {
            movingTowardsEndPosition = true;
        }
    }

    void FixedUpdate()
    { //Platform movement towards start/end position depending on boolean
        if (movingTowardsEndPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, speed * Time.deltaTime);
        }
    }
}