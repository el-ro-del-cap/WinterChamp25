using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StratagemArrow : MonoBehaviour
{
    public Image arrowImage;

    public Color defaultColor = Color.white;
    public Color pressedColor = Color.yellow;
    public Color wrongColor = Color.red;

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

    public void DoWrongColorAnimation(float duration = 1f) {
        arrowImage.color = wrongColor;
        StartCoroutine(WrongColorAnimCR(duration));

    }

    private IEnumerator WrongColorAnimCR(float duration) { //Esto no es lo más eficiente, hacer los cálculos para cada flecha separada, pero whatever
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time < endTime) {
            float lerpPoint = EasingsScript.EaseInBack(Mathf.InverseLerp(startTime, endTime, Time.time));
            arrowImage.color = Color.Lerp(wrongColor, defaultColor, lerpPoint);
            yield return null;
        }
        arrowImage.color = defaultColor;
    }

}
