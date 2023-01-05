using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private bool smash,invicible;
    private float currentTime;

    private int currentBrokenStacks, totalstacks;

    public GameObject splash;
    public GameObject invincibleObj;
    public Image invincibleFill;
    public GameObject fireEffect,winEffect,splashEffect;

    public enum PlayerState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public PlayerState playerState = PlayerState.Prepare;

    public AudioClip bounceOffClip, deadClip, winClip, destroyClip, iDestroyClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;

    }
    private void Start()
    {
        totalstacks = FindObjectsOfType<StackController>().Length;
    }

    void Update()
    {
        #region Playing
        if (playerState==PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                smash = true;

            }
            if (Input.GetMouseButtonUp(0))
            {
                smash = false;
            }
            if (invicible)
            {
                currentTime -= Time.deltaTime * .35f;
                if (!fireEffect.activeInHierarchy)
                {
                    fireEffect.SetActive(true);
                }
            }
            else
            {
                if (fireEffect.activeInHierarchy)
                {
                    fireEffect.SetActive(false);

                }
                if (smash)
                {
                    currentTime += Time.deltaTime * .8f;

                }
                else
                {
                    currentTime -= Time.deltaTime * .5f;

                }
            }
            if (currentTime>=0.15f || invincibleFill.color==Color.red)
            {
                invincibleObj.SetActive(true);
            }
            else
            {
                invincibleObj.SetActive(false);

            }
            if (currentTime >= 1)
            {
                currentTime = 1;
                invicible = true;
                invincibleFill.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invicible = false;
                invincibleFill.color = Color.white;
            }

            if (invincibleObj.activeInHierarchy)
            {
                invincibleFill.fillAmount = currentTime / 1;

            }
            #endregion
        }

        if (playerState == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<LevelSpawner>().NextLevel();
        }

    }

    private void FixedUpdate()
    {
        if (playerState==PlayerState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }
       
        if (rb.velocity.y>5)
        {
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
        }
    }

    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;

        if (!invicible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySFX(destroyClip, .5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySFX(iDestroyClip, .5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.instance.PlaySFX(bounceOffClip, .5f);
        }
        else
        {
            if (invicible)
            {
                if (collision.gameObject.tag=="enemy"||collision.gameObject.tag=="plane")
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();

                }
            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();

                }
                if (collision.gameObject.tag == "plane")
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);

                    ScoreManager.instance.ResetScore();
                    playerState = PlayerState.Died;
                    SoundManager.instance.PlaySFX(deadClip, .5f);


                    
                    splash = Instantiate(splashEffect, gameObject.transform.position,Quaternion.identity );
                    splash.transform.SetParent(collision.transform);
                    splash.transform.GetChild(0).GetComponent<SpriteRenderer>().material.color =
                        transform.GetChild(0).GetComponent<MeshRenderer>().material.color;


                }
            }        
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalstacks);

        if (collision.gameObject.tag=="Finish" && playerState==PlayerState.Playing)
        {
            playerState = PlayerState.Finish;
            SoundManager.instance.PlaySFX(winClip, .7f);
            GameObject finishEffect = Instantiate(winEffect, transform.position, Quaternion.identity);
            transform.SetParent(gameObject.transform);

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.tag== "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            

        }
        
    }
}
