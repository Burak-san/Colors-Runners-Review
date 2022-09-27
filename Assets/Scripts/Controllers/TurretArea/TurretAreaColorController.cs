using System;
using Enums;
using UnityEngine;

namespace Controllers.TurretArea
{
    public class TurretAreaColorController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public ColorType ColorType;

        #endregion

        #region Private Variables

        private MeshRenderer _meshRenderer;

        #endregion

        #endregion
        
        private void Awake()
        {
            GetReferences();
        }

        private void Start()
        {
            SetAreaMaterial(ColorType);
        }

        private void GetReferences()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void SetAreaMaterial(ColorType colorType)
        {
            _meshRenderer.material = Resources.Load<Material>($"Materials/CollectableColors/{colorType}");
        }
    }

    
    
}