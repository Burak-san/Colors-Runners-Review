using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Controllers.Portal
{
    public class PortalMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public ColorType Color;
        #endregion

        #region Serialized Variables
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
            SetGateMaterial(Color);
        }

        private void GetReferences()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetGateMaterial(ColorType colorType)
        {
            _meshRenderer.material = Resources.Load<Material>($"Materials/PortalColors/{colorType}");
        }
    }
}


