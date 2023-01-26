using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonComponant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform myRect;
    
    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        myRect.DOKill();
        myRect.DOScale(1.1f, 0.255f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myRect.DOKill();
        myRect.DOScale(1f, 0.20f).SetEase(Ease.InBack);
    }
}
