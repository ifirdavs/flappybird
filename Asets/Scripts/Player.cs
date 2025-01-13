using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 7.5f;
    public float gravity = -23.81f;
    public float tilt = 6.5f;

    private AudioSource hitSound, wingSound, dieSound;
    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;

    private void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        wingSound = audioSources[0];
        hitSound = audioSources[1];
        dieSound = audioSources[2];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            wingSound.Play();
            direction = Vector3.up * strength;
        }

        // Apply gravity and update the position
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // Tilt the bird based on the direction
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length) {
            spriteIndex = 0;
        }

        if (spriteIndex < sprites.Length && spriteIndex >= 0) {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle")) {
            hitSound.Play();
            GameManager.Instance.GameOver();
            dieSound.Play();

        } else if (other.gameObject.CompareTag("Scoring")) {
            GameManager.Instance.IncreaseScore();
        }
    }

}
