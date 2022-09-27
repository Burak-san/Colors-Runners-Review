using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Controllers.Particle;
using Keys;
using Signals;

namespace Managers
{
    public class ParticleManager : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private List<ParticleEmitController> emitControllers;

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            StackSignals.Instance.onDecreaseStack += OnDecreaseStack;
            ParticleSignals.Instance.onPlayerDeath += OnParticleDeath;
            ParticleSignals.Instance.onParticleBurst += OnParticleBurst;
            ParticleSignals.Instance.onParticleStop += OnParticleStop;
            ParticleSignals.Instance.onParticleLookRotation += OnParticleLookRotation;
        }

        private void UnSubscribeEvents()
        {
            StackSignals.Instance.onDecreaseStack -= OnDecreaseStack;
            ParticleSignals.Instance.onPlayerDeath -= OnParticleDeath;
            ParticleSignals.Instance.onParticleBurst -= OnParticleBurst;
            ParticleSignals.Instance.onParticleStop -= OnParticleStop;
            ParticleSignals.Instance.onParticleLookRotation -= OnParticleLookRotation;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private async void OnParticleBurst(Vector3 transform)
        {
            Vector3 newTransform = new Vector3(Random.Range(transform.x - 1.5f, transform.x + 1.5f),transform.y,transform.z);
            
            emitControllers[0].EmitParticle(newTransform);
            await Task.Delay(100);
            emitControllers[1].EmitParticle(newTransform + new Vector3(0,4,3));
        }

        private void OnParticleLookRotation(Quaternion toRotation)
        {
            emitControllers[0].LookRotation(toRotation);
        }

        private void OnDecreaseStack(ObstacleCollisionGOParams obstacleCollisionGoParams)
        {
            var collectedTransform = obstacleCollisionGoParams.Collected.transform.position;
            OnParticleDeath(collectedTransform);
        }

        private void OnParticleDeath(Vector3 collectedTransform)
        {
            emitControllers[1].EmitParticle(collectedTransform);
        }

        private void OnParticleStop()
        {
            for (int i = 0; i < emitControllers.Count; i++)
            {
                emitControllers[i].EmitStop();
            }
        }
    }
}