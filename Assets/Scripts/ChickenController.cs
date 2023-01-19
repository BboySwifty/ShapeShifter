using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    public float speed = 3.0f;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;

    private Rigidbody2D rigidbody2d;
    private float timer;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction *= -1;
            transform.Rotate(new Vector3(0, 180, 0));
            timer = changeTime;
        }

        rigidbody2d.velocity = new Vector2(speed * direction * -1, rigidbody2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController controller = collision.collider.GetComponent<PlayerController>();

        if (controller != null && !controller.IsInvisible)
        {
            controller.Kill();
        }
    }
}
