using UnityEngine;
using UnityEngine.UI;

public class LoadingMenu : MonoBehaviour
{
    private RectTransform rectComponent;
    private Image imageComp;

    [SerializeField] private float speed = 0.7f;

    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }

    void Update()
    {
        if (imageComp.fillAmount != 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
        }
        else
        {
            imageComp.fillAmount = 0.0f;
        }
    }
}
