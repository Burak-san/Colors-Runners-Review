using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Signals;
using Controllers.Collectable;
using System.Threading.Tasks;

public class CollectableManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    public ColorType ColorType;
    public CollectableMatchType MatchType;
    public CollectableMeshController CollectableMeshController;

    #endregion

    #region Serialized Variables

    [SerializeField] private CollectableAnimationController animationController;
    [SerializeField] private CollectableMovementController movementController;
    #endregion

    #region Private Variables

    #endregion
    #endregion

    private void Awake()
    {
        ColorOnInit();
    }

    public void SetAnim(CollectableAnimationStates animationStates)
    {
        animationController.ChangeCollectableAnimation(animationStates);
    }

    public void ColorOnInit()
    {
        CollectableMeshController.GetColor(ColorType);
    }

    public void ChangeColor(ColorType colorType)
    {
        ColorType = colorType;
        CollectableMeshController.GetColor(ColorType);
    }

    public void DelistFromStack()
    {
        StackSignals.Instance.onEnterDroneArea?.Invoke(transform.GetSiblingIndex());
    }

    public void SetCollectablePositionOnDroneArea(Transform DroneCheckColorArea)
    {
        movementController.MoveToColorArea(DroneCheckColorArea);
    }

    public async void IncreaseStackAfterDroneArea()
    {
        await Task.Delay(3000);
        RemoveOutline(false);
        StackSignals.Instance.onIncreaseStack?.Invoke(gameObject);
        SetAnim(CollectableAnimationStates.Running);
    }
    public void RemoveOutline(bool outlineBool)
    {
        CollectableMeshController.OutlineChanger(outlineBool);
    }

    public void EnterTurretArea(GameObject otherGameObject)
    {
        CollectableMeshController.CompareColorType(otherGameObject, ColorType);
    }

    public void SendCollectableTransform()
    {
        TurretAreaSignals.Instance.onTurretDetect.Invoke(gameObject);
    }

    public void ExitTurretArea()
    {
        TurretAreaSignals.Instance.onExitTurretArea.Invoke();
    }

    public async void SetActiveFalse()
    {
        await Task.Delay(2000);
        Destroy(gameObject);
        await Task.Delay(100);
        StackSignals.Instance.onCheckStack?.Invoke();
    }
}
