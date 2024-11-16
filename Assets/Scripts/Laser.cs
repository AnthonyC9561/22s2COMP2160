using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLineRender;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private Color laserColour;
    [SerializeField] private float laserLength = 25f;
    [SerializeField] private Switch switchScript;

    private float startPoint = 0;
    private int startPosition = 0;
    private int endPosition = 1;
    private Gradient laserGradient = new Gradient();

    void Start()
    {
        laserLineRender = GetComponent<LineRenderer>();

        float alpha = 1.0f;
        //sets a flat colour for the entire laser beam, based on unity's documentation 2021.3
        laserGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(laserColour, startPoint) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, startPoint) }
            );
        laserLineRender.colorGradient = laserGradient;
    }

    void Update()
    {
        if (switchScript.IsActivated == true)
        {
            laserLineRender.enabled = true;
            laserLineRender.SetPosition(startPosition, laserStartPoint.position); //Setting start position of laser
            if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, laserLength))
            {
                if (hit.collider)
                {
                    laserLineRender.SetPosition(endPosition, hit.point); //Setting end point of laser with Raycast
                }

                if (hit.transform.CompareTag(ConstantStrings.Player))
                {
                    GameManager.Instance.Die(); //If player collides with Raycast it calls die method
                }
            }
            else
            {//adds start point vector to make laser and raycast align with laser 
                laserLineRender.SetPosition(endPosition, laserStartPoint.position + (transform.right * laserLength));
            }
        }
        else if (switchScript.IsActivated == false)
        {
            laserLineRender.enabled = false;
        }
    }
}

