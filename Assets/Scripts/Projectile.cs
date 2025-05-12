using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    
    public Vector2 knockback=new Vector2(0,0);

    private SpriteRenderer spriteRenderer;
    private float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    private Color startColor;
    private bool fading = false;

    Rigidbody2D rb;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }


    void Start()
    {
        // yer çekimi için rigidbody dynmaic kullan
        rb.linearVelocity = new Vector2(moveSpeed.x * transform.localScale.x,moveSpeed.y );
        Invoke("StartFade", 1.5f); // 1.5 saniye sonra fade başlasın
    }
    public void StartFade()
    {
        fading = true;
        timeElapsed = 0f;
    }


    void Update()
    {
        if (fading)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, timeElapsed / fadeTime);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            if (timeElapsed >= fadeTime)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if ( damageable != null ) 
        {
        Vector2 deliveredKnockBack=transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y );
            bool gotHit = damageable.Hit(damage, deliveredKnockBack);
            if (gotHit)
                
                Debug.Log(collision.name + "hit for " + damage);
            Destroy(gameObject);
        }
    }
}
