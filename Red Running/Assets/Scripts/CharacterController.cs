using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    Sprite[] idleAnimation;
    [SerializeField]
    Sprite[] runAnimation;
    [SerializeField]
    Sprite[] jumpAnimation;

    SpriteRenderer spriteRenderer;

    int idleAnimCounter = 0;
    int runAnimCounter = 0;

    float horizontal = 0;
    float idleAnimTime = 0;
    float runAnimTime = 0;
    Rigidbody2D physics;
    Vector3 vec;
    bool jumpControl = true;

    GameObject camera;
    Vector3 cameraFirstPos;
    Vector3 cameraLastPos;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        physics = GetComponent<Rigidbody2D>();

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFirstPos = camera.transform.position - transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpControl)
            {
                physics.AddForce(new Vector2(0, 500));
                jumpControl = false;
            }
        }
    }
    void FixedUpdate()
    {
        CharacterMove();
        Animation();
    }

    void LateUpdate()
    {
        CameraControl();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        jumpControl = true;
    }

    void CharacterMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal*5, physics.velocity.y, 0);
        physics.velocity = vec;
    }

    //metodlara çek bu kısımları
    void Animation()
    {
        if (jumpControl)
        {
            if (horizontal == 0)
            {
                idleAnimTime += Time.deltaTime;
                if (idleAnimTime > 0.05f)
                {
                    spriteRenderer.sprite = idleAnimation[idleAnimCounter++];
                    if (idleAnimCounter == idleAnimation.Length)
                    {
                        idleAnimCounter = 0;
                    }
                    idleAnimTime = 0;
                }

            }
            else if (horizontal > 0)
            {
                runAnimTime += Time.deltaTime;
                if (runAnimTime > 0.01f)
                {
                    spriteRenderer.sprite = runAnimation[runAnimCounter++];
                    if (runAnimCounter == runAnimation.Length)
                    {
                        runAnimCounter = 0;
                    }
                    runAnimTime = 0;
                }
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }

            else if (horizontal < 0)
            {
                runAnimTime += Time.deltaTime;
                if (runAnimTime > 0.01f)
                {
                    spriteRenderer.sprite = runAnimation[runAnimCounter++];
                    if (runAnimCounter == runAnimation.Length)
                    {
                        runAnimCounter = 0;
                    }
                    runAnimTime = 0;
                }
                transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
            }
        }
        else
        {
            if (physics.velocity.y > 0)
            {
                spriteRenderer.sprite = jumpAnimation[0];
            }
            else
            {
                spriteRenderer.sprite = jumpAnimation[1];
            }
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
            else if(horizontal < 0)
            {
                transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
            }
        }
    }

    void CameraControl()
    {
        cameraLastPos = cameraFirstPos + transform.position;
        //camera.transform.position = cameraLastPos;
        camera.transform.position = Vector3.Lerp(camera.transform.position, cameraLastPos, 0.08f);
    }
}
