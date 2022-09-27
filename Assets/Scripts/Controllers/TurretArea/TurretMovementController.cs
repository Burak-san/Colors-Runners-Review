using System;
using Enums;
using Keys;
using Signals;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Controllers.TurretArea
{
    public class TurretMovementController : MonoBehaviour
    {

        #region SelfVariables

        #region Serialized Variables

        [SerializeField] private Transform turretAreaTransform;
        [FormerlySerializedAs("timeIncreasedSpeed")] [SerializeField] private float turretActiveSpeed;
        [SerializeField] private float invokeRate;

        #endregion
        
        #region Private Variables

        private Quaternion _direction;
        private Vector3 _shotPos;
        private Vector3 _collectablePos;
        private Vector3 _shotDistance;
        private float _randomClampStartPos;
        private float _randomClampEndPos;
        private TurretState _turretState;
        private float _turretStartXPos;
        private float _turretEndXPos;
        private float _turretStartZPos;
        private float _turretEndZPos;

        #endregion

        #endregion

        private void Awake()
        {
            _turretStartXPos = turretAreaTransform.transform.parent.transform.position.x -
                               turretAreaTransform.GetChild(0).transform.localScale.x;
            _turretEndXPos = turretAreaTransform.transform.parent.transform.position.x +
                             turretAreaTransform.GetChild(1).transform.localScale.x;
            _turretStartZPos = turretAreaTransform.transform.parent.transform.position.z -
                               turretAreaTransform.GetChild(0).transform.localScale.z / 2;
            _turretEndZPos = turretAreaTransform.transform.parent.transform.position.z +
                             turretAreaTransform.GetChild(1).transform.localScale.z / 2;
        }

        private void Start()
        {
            InvokeRepeating("TurretPatrolling",0,invokeRate);
        }

        public void DetectCollectable(GameObject detectedCollectable)
        {
            _collectablePos = detectedCollectable.transform.position;
            _turretState = TurretState.Active;
        }

        public void StopTurret()
        {
            CancelInvoke("TurretPatrolling");
            _turretState = TurretState.Patrol;
        }

        private void TurretPatrolling()
        {
            _randomClampStartPos = Random.Range(_turretStartXPos, _turretEndXPos);
            _randomClampEndPos = Random.Range(_turretStartZPos, _turretEndZPos);
        }

        private void FixedUpdate()
        {
            ChangeTurretMovementWithState(_turretState);
        }

        private void ChangeTurretMovementWithState(TurretState turretState)
        {
            switch (turretState)
            {
                case TurretState.Patrol:
                    _shotPos = new Vector3(_randomClampStartPos, 0, _randomClampEndPos);
                    _shotDistance = _shotPos - transform.position;
                    _direction = Quaternion.LookRotation(_shotDistance);
                    transform.rotation = Quaternion.Lerp(transform.rotation, _direction, Mathf.Lerp(0,1,Time.fixedDeltaTime));
                    break;
                case TurretState.Active:
                    _shotPos = _collectablePos + new Vector3(0, 1, 0);
                    _shotDistance = _shotPos - transform.position;
                    _direction = Quaternion.LookRotation(_shotDistance);
                    transform.rotation = Quaternion.Lerp(transform.rotation, _direction, Mathf.Lerp(0,1,turretActiveSpeed*Time.fixedDeltaTime));
                    ShotCollectable();
                    break;
            }
        }

        private void ShotCollectable()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                int RandomInt = Random.Range(0, 100);
                if (hit.transform.CompareTag("Collected"))
                {
                    if (RandomInt <= 3)
                    {
                        StackSignals.Instance.onDecreaseStack?.Invoke(new ObstacleCollisionGOParams()
                        {
                            Collected = hit.transform.gameObject,
                        });
                    }
                }
            }
        }
    }
}