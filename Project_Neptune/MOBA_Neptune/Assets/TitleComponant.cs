using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TitleComponant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform myTransform = GetComponent<RectTransform>();

        myTransform.DOKill();
        myTransform.DOScale(1.085f, 4f).OnComplete(() => myTransform.DOScale(0.93f, 5.525f)).SetLoops(20, LoopType.Yoyo);
    }
}
