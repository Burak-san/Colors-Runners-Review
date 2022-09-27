using System;
using System.Threading.Tasks;
using Signals;
using UnityEngine;
using Controllers.Drone;

namespace Managers
{
    public class DroneManager : MonoBehaviour
    {
        #region Serializable Variables

        [SerializeField] private GameObject dronePrefab;
        [SerializeField] private GameObject colorArea1;
        [SerializeField] private GameObject colorArea2;
        [SerializeField] private GameObject ColliderOnExitAreaGeneral;

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            DroneAreaSignals.Instance.onColliderDisable += OnColliderDisable;
            DroneAreaSignals.Instance.onEnableFinalCollider += OnFinalAreaCollider;
        }

        private void UnSubscribeEvents()
        {
            DroneAreaSignals.Instance.onColliderDisable -= OnColliderDisable;
            DroneAreaSignals.Instance.onEnableFinalCollider -= OnFinalAreaCollider;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private async void OnColliderDisable()
        {
            dronePrefab.SetActive(true);
            await Task.Delay(2000);
            transform.GetComponent<BoxCollider>().enabled = false;
            colorArea1.GetComponent<BoxCollider>().enabled = false;
            colorArea2.GetComponent<BoxCollider>().enabled = false;
        }

        private async void OnFinalAreaCollider()
        {
            await Task.Delay(3000);
            ColliderOnExitAreaGeneral.SetActive(true);
            colorArea1.GetComponent<DroneAreaColorController>().Scale();
            colorArea2.GetComponent<DroneAreaColorController>().Scale();
            await Task.Delay(2000);
            ColliderOnExitAreaGeneral.SetActive(false);
        }
    }
}