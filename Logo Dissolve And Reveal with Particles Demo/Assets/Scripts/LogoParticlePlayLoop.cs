using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoParticlePlayLoop : MonoBehaviour
{

    private ParticleSystem m_Particle;

    private void Awake()
    {
        m_Particle = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Particle.Play();
        StartCoroutine(PlayParticleLoop());
    }

    private IEnumerator PlayParticleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_Particle.main.duration);
            m_Particle.Play();
        }
    }
}
