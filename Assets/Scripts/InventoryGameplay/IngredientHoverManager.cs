using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientHoverManager : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public IngredientSO ingredient; 
    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryViewGameManager.Instance.OnIngredientHoverEnter(rectTransform, ingredient);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryViewGameManager.Instance.OnIngredientHoverExit();
    }
}
