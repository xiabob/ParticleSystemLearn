using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindParticleSystem : MonoBehaviour
{
    private ParticleSystem[] m_ParticleSystems;
    private float[] m_simulationTimes;
    public float StartTime = 2.0f;
    public float SimulationSpeedScale = 1.0f;

    private void Initialize()
    {
        m_ParticleSystems = GetComponentsInChildren<ParticleSystem>(false);
        m_simulationTimes = new float[m_ParticleSystems.Length];
    }

    private void ResetParticleSystems()
    {
        // reset time
        for (int i = 0; i < m_simulationTimes.Length; i++)
        {
            m_simulationTimes[i] = 0;
        }

        // reset all particle system, set simulate time to end
        m_ParticleSystems[0].Simulate(StartTime, true, false, true);
    }

    private void OnEnable()
    {
        if (null == m_ParticleSystems)
        {
            Initialize();
            ResetParticleSystems();
        }
    }


    // Update is called once per frame
    void Update()
    {
        // stop and clear current particle systems 
        m_ParticleSystems[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // play particle system manual by codes
        for (int i = 0; i < m_simulationTimes.Length; i++)
        {
            ParticleSystem particleSystem = m_ParticleSystems[i];
            float simulationTime = m_simulationTimes[i];

            bool useAutoRandomSeed = particleSystem.useAutoRandomSeed;
            particleSystem.useAutoRandomSeed = false;

            particleSystem.Play(false);

            float deltaTime = particleSystem.main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            simulationTime -= (deltaTime * particleSystem.main.simulationSpeed) * SimulationSpeedScale;
            m_simulationTimes[i] = simulationTime;

            float currentSimulationTime = StartTime + simulationTime;
            particleSystem.Simulate(currentSimulationTime, false, false, true);

            particleSystem.useAutoRandomSeed = useAutoRandomSeed;

            if (currentSimulationTime < 0.0f)
            {
                particleSystem.Play(false);
                particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            // Debug.Log("currentSimulationTime:" + currentSimulationTime + " simulationTime:" + simulationTime);
        }
    }
}
