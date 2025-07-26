using Unity.VisualScripting;
using UnityEngine;

public class EasingsScript {
    
    public static float EaseOutExpo(float x) {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
}

    public static float EaseOutQuint(float x) {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    public static float EaseOutQuart(float x) {
        return 1 - Mathf.Pow(1 - x, 4);

    }

    public static float EaseOutCirc(float x) {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));

    }

    public static float EaseOutCubic(float x) {
        return 1 - Mathf.Pow(1 - x, 3);

    }

    public static float EaseOutQuad(float x) {
        return 1 - (1 - x) * (1 - x);

    }

    public static float EaseInQuart(float x) {
        return x * x * x * x;

    }

    public static float EaseInQuad(float x) {
        return x * x;
    }

    public static float EaseInBack(float x) {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;

    }

}
