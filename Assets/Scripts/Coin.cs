using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 1;
    [SerializeField] private float _rotationSpeed = 150;

    private bool _collected = false;

    // Update is called once per frame
    void Update()
    {
        CoinRotation(_rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected) { return; }

        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            _collected = true;
            GameManager.Instance.AddCoins(_value);
        }
        else
        {
            Debug.LogWarning("[Coin] GameManager.Instance is null!");
        }

        // Play coin pickup sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoinPickup();
        }

        // Destroy the coin object after being collected
        Destroy(gameObject);
    }

    private void CoinRotation(float speed)
    {
        speed = speed * Time.deltaTime;
        this.transform.Rotate(new Vector3(0, speed, 0));
    }
}
