using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Color switchOnColor = Color.green;
    [SerializeField] private Color switchOffColor = Color.red;
    [SerializeField] private bool isActivated;
    public bool IsActivated
    {
        get
        {
            return isActivated;
        }
    }

    private float timer;
    private float delay = 1f;
    private Renderer switchRenderer;

    void Start()
    {
        switchRenderer = GetComponent<Renderer>();
        SwitchOff();
        ResetTimer();
    }

    void SwitchOff()
    {
        isActivated = false;
        ResetTimer();
        switchRenderer.material.color = switchOffColor;
    }

    void SwitchOn()
    {
        isActivated = true;
        ResetTimer();
        switchRenderer.material.color = switchOnColor;
    }

    private void OnTriggerEnter(Collider other)
    { //If player collides with Switch it sends out activation/deactivationn
        timer = Time.deltaTime - timer;

        if (isActivated == false && timer <= 0f)
        {
            SwitchOn();
        }
        if (isActivated == true && timer <= 0f)
        {
            SwitchOff();
        }
    }

    void ResetTimer()
    {
        timer = delay;
    }

}


