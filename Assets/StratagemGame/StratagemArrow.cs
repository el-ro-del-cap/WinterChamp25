using UnityEngine;
using UnityEngine.UI;

public class StratagemArrow : MonoBehaviour
{
    public Image arrowImage;

    public Color defaultColor = Color.white;
    public Color pressedColor = Color.yellow;

    public void ChangeColor(Color newColor) {
        arrowImage.color = newColor;
    }

    private void Start() {
        arrowImage.color = defaultColor;
    }

    public void PressArrow() {
        arrowImage.color = pressedColor;
    }


    public void UnPressArrow() {
        arrowImage.color = defaultColor;
    }
}
