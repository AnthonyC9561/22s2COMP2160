using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Color checkpointColour;
    [SerializeField] private Color checkpointParticleColour;
    private ParticleSystem checkpointParticleEffect;
    private Transform checkPoint;

    private void Start()
    {
        checkpointParticleEffect = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule checkpointParticles = GetComponent<ParticleSystem>().main;
        checkpointParticles.startColor = checkpointParticleColour; //Setting particle colours

        MeshRenderer checkpoint = GetComponent<MeshRenderer>();
        checkpoint.material.SetColor(ConstantStrings.Color, checkpointColour); //Setting checkpoint colour
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ConstantStrings.Player))
        {//if condition to ensure checkpoint only collides with player and nothing else (such as ground)
            GameManager.Instance.ReachedCheckpoint(transform.position);
            checkpointParticleEffect.Stop();//to let players know they touched the checkpoint
        }
    }
}
