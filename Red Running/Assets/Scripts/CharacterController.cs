using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    Sprite[] idleAnimation;
    [SerializeField]
    Sprite[] runAnimation;
    [SerializeField]
    Sprite[] jumpAnimation;
    [SerializeField]
    Text lifeText;
    [SerializeField]
    Image blackBackgorund;

    SpriteRenderer spriteRenderer;

    int idleAnimCounter = 0;
    int runAnimCounter = 0;
    int life = 10;

    float horizontal = 0;
    float idleAnimTime = 0;
    float runAnimTime = 0;

    float blackBackroundTimer = 0;
    float mainMenuTimer = 0;

    Rigidbody2D physics;
    Vector3 vec;
    bool jumpControl = true;

    GameObject camera;
    Vector3 cameraFirstPos;
    Vector3 cameraLastPos;

    void Start()
    {
        Time.timeScale = 1;

        spriteRenderer = GetComponent<SpriteRenderer>();
        physics = GetComponent<Rigidbody2D>();

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFirstPos = camera.transform.position - transform.position;

        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("levelId"))
        {
            PlayerPrefs.SetInt("levelId", SceneManager.GetActiveScene().buildIndex);
        }

        lifeText.text = "RED : " + life;
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
        if (life <= 0)
        {
            Time.timeScale = 0.4f;
            lifeText.enabled = false;
            blackBackroundTimer += 0.03f;
            blackBackgorund.color = new Color(0, 0, 0, blackBackroundTimer);
            mainMenuTimer += Time.deltaTime;
            if(mainMenuTimer > 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void LateUpdate()
    {
        CameraControl();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        jumpControl = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "TagBullet")
        {
            life--;
            lifeText.text = "RED : " + life;
        }

        if(col.gameObject.tag == "TagMac")
        {
            life -= 10;
            lifeText.text = "RED : " + life;
        }

        if (col.gameObject.tag == "TagSaw")
        {
            life -= 10;
            lifeText.text = "RED : " + life;
        }
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
