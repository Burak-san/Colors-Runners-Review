using UnityEngine;
using DG.Tweening;
using Enums;
using Controllers.TurretArea;
using Managers;

namespace Controllers.Collectable
{
    public class CollectableMeshController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private CollectableManager manager;
        #endregion

        public void GetColor(ColorType colorType)
        {
            skinnedMeshRenderer.material = Resources.Load<Material>($"Materials/CollectableColors/{colorType}");
        }

        public void OutlineChanger(bool outlineOn)
        {
            if(TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                var matColor = skinnedMeshRenderer.material;
                if (outlineOn)
                {
                    matColor.DOFloat(0f, "_OutlineSize", 1f);
                }
                else
                {
                    matColor.DOFloat(100f, "_OutlineSize", 1f);
                }
            }
        }

        public void CompareColorType(GameObject otherGameObject, ColorType collectableColorType)
        {
            if(otherGameObject.GetComponent<TurretAreaColorController>().ColorType != collectableColorType)
            {
                manager.SendCollectableTransform();
            }
        }
    }
}