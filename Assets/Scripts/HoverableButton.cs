using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;

public class HoverableButton : UnityEngine.UI.Button, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonHoveredEvent OnMouseEnter { get; set; }
    public ButtonHoveredEvent OnMouseExit { get; set; }

    public HoverableButton()
    {
        OnMouseEnter = new ButtonHoveredEvent();
        OnMouseExit = new ButtonHoveredEvent();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke();
    }

    public class ButtonHoveredEvent : UnityEvent
    {
    }
}
