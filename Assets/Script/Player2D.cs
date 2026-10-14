using UnityEngine;
using UnityEngine.SceneManagement; // Требуется для перезагрузки сцены

public class Player2D : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 7f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private bool onGround;

    [Header("Game Stats")]
    private int coins = 0;
    private int coinsTotal = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Автоматически считаем все монетки на сцене по тегу "Coin"
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    void Update()
    {
        // Прыжок по нажатию Пробела, если персонаж на земле
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            onGround = false;
        }
    }

    void FixedUpdate()
    {
        // Горизонтальное движение через физику
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
    }

    // Обработка твердых столкновений (Земля, Враг)
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Game over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Обработка триггеров (Монетки, Выход)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            Debug.Log("Coins: " + coins + " / " + coinsTotal);
        }

        if (other.CompareTag("Exit"))
        {
            if (coins >= coinsTotal)
            {
                Debug.Log("You win!");
            }
            else
            {
                Debug.Log("Collect all coins first! " + coins + " / " + coinsTotal);
            }
        }
    }
}