using System.Collections;
using System.Collections.Generic;
using Data.ValueObject;
using DG.Tweening;
using UnityEngine;

namespace Commands.Stack
{
    public class CollectableScaleUpCommand
    {

        #region Self Variables

        #region Private Variables

        private List<GameObject> _stacklist;
        private StackData _stackData;

        #endregion

        #endregion
        public CollectableScaleUpCommand(ref List<GameObject> stacklist, ref StackData stackData)
        {
            _stacklist = stacklist;
            _stackData = stackData;
        }
        public IEnumerator Execute(GameObject _collectable)
        {
            {
                for (int i = 0; i < _stacklist.Count; i++)
                {
                    int index = i;
                    Vector3 scale = Vector3.one * _stackData.ScaleFactor;
                    Vector3 scaleReverse = Vector3.one * _stackData.ReverseScaleFactor;
                    _stacklist[index].transform.DOScale(scale, _stackData.ScaleUpDelay).SetEase(Ease.Flash);
                    _stacklist[index].transform.DOScale(scaleReverse, _stackData.ScaleUpDelay).SetDelay(_stackData.ScaleUpDelay).SetEase(Ease.Flash);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}