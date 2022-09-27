using UnityEngine;
using DG.Tweening;
using Enums;
using Managers;

namespace Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private PlayerManager playerManager;
        #endregion

        public void SetColor(ColorType colorType)
        {
            skinnedMeshRenderer.material = Resources.Load<Material>($"Materials/CollectableColors/{colorType}");
        }

        public void IncreaseSize()
        {
            var PlayerManagerScale = playerManager.transform.localScale;

            if(PlayerManagerScale.x < Vector3.one.x * 2f)
            {
                playerManager.transform.DOScale(Vector3.one * 2f, 2f);
            }
        }

        public void DecreaseSize()
        {
            var PlayerManagerScale = playerManager.transform.localScale;

            if(PlayerManagerScale.x > Vector3.one.x * 1f)
            {
                playerManager.transform.DOScale(Vector3.one, 2f);
            }
        }
    }
}