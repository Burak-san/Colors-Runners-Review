using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Controllers.Collectable
{
    public class CollectableMovementController : MonoBehaviour
    {

        #region Serialized Variables

        [SerializeField] private CollectableManager manager;
        #endregion

        public void MoveToColorArea(Transform coloredDroneArea)
        {
            var RandomZ = Random.RandomRange(-(coloredDroneArea.localScale.z / 2 - 6), (coloredDroneArea.localScale.z / 2 - 2));
            Vector3 newPos = new Vector3(
                coloredDroneArea.position.x,
                coloredDroneArea.position.y + 0.5f,
                coloredDroneArea.position.z + RandomZ);
            gameObject.transform.DOMove(newPos, 2f).OnComplete(() =>
            {
                manager.SetAnim(Enums.CollectableAnimationStates.Crouching);
            });


        }
    }
}