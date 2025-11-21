using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    private bool isMenuActive;

    [SerializeField] private CanvasGroup statsMenu;
    [SerializeField] private CanvasGroup skillsMenu;


    [SerializeField] private  Image MenuToggleImage;
    [SerializeField] private  Sprite OpenSprite;
    [SerializeField] private  Sprite CloseSprite;

    public void ToggleMenu(CanvasGroup target)
    {
        SetMenuState(statsMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(target, true);
    }

    public void ToggleMainMenu()
    {
        isMenuActive = !isMenuActive;
        SetMenuState(menuBar, isMenuActive);
        MenuToggleImage.sprite = isMenuActive ? CloseSprite : OpenSprite;

        SetMenuState(statsMenu, false);
        SetMenuState(skillsMenu, false);
    }

    private void SetMenuState(CanvasGroup group, bool active)
    {
        if (group == null) return;

        group.alpha = active ? 1 : 0;
        group.interactable = active;
        group.blocksRaycasts = active;
    }
}
