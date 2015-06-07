using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatText : MonoBehaviour 
{
    private bool isInitialized;

    private void Awake()
    {
        GetComponent<Animator>().enabled = false;
    }

    public void Init(string text, Color color, Vector3 worldSpacePosition)
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Text>().text = ColorToHex(color) + text + "</color>";

        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(worldSpacePosition);
        Vector2 screenSpacePosition = new Vector2((viewportPoint.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                                                    (viewportPoint.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));

        float delta = 15f;
        GetComponent<RectTransform>().anchoredPosition = screenSpacePosition + new Vector2(Random.Range(-delta, delta), Random.Range(-delta, delta));

        isInitialized = true;
    }

    private void Update()
    {
        if(isInitialized && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
    }

    private string ColorToHex(Color32 color)
    {
        return "<color=#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + ">";
    }
}