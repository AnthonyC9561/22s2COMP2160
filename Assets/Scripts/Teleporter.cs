using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Color teleporterColour;
    [SerializeField] private Color teleportWaveColour;
    private ParticleSystem teleporterWaves;

    private void Start()
    {//setting colours at start 
        teleporterWaves = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule teleporterParticles = GetComponent<ParticleSystem>().main;
        teleporterParticles.startColor = teleportWaveColour;//sets particle colours

        MeshRenderer teleporter = GetComponentInChildren<MeshRenderer>();
        teleporter.material.SetColor(ConstantStrings.Color, teleporterColour);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ConstantStrings.Player))
        {//if condition to ensure only the player collides with it and not other gameobjects
            teleporterWaves.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(ConstantStrings.Player))
        {
            teleporterWaves.Stop();
        }
    }
}
