using UnityEngine;

public class RotateUICoin : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 800f;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        FlippingCoin(_rotateSpeed);
    }

    private void FlippingCoin(float speed)
    {
        speed = speed * Time.deltaTime;
        // Rotate only on Y axis to create a coin-flip effect
        _rect.Rotate(0f, speed, 0f);
    }

}
