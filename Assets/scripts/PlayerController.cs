using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction
{
    UP,
    RIGHT,
    DOWN,
    LEFT
};

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rigidbody = null;

    [SerializeField] private Fire Fire = null;
    [SerializeField] private Animator Animator = null;

    [SerializeField] private bool CanMoveInAir = true;

    [SerializeField] private float MaxRunspeed = 5.0f;
    [SerializeField] private float SecondsToMaxSpeed = 0.5f;
    [SerializeField] private float SecondsToNoSpeed = 0.5f;

    [SerializeField] private KeyCode RightKey = KeyCode.D;
    [SerializeField] private KeyCode LeftKey = KeyCode.A;
    [SerializeField] private KeyCode JumpKey = KeyCode.W;

    [SerializeField] private BoxCollider2D Collider = null;

    [SerializeField] private float GroundCheckRayLEngth = 0.1f;

    [SerializeField] private float JumpForce = 500.0f;

    [SerializeField] private float FireDamage = 0.25f;

    [SerializeField] private List<GameObject> BurnObjects = new List<GameObject>();

    [SerializeField] private List<AudioClip> WalkSounds = new List<AudioClip>();
    [SerializeField] private float PitchVariety = 0.1f;
    [SerializeField] private AudioSource[] Sources = null;

    [SerializeField] private GameObject[] DeathFires;
    [SerializeField] private GameObject BurnUpSmoke = null;
    [SerializeField] private float DeathFireSpreadDelay = 0.05f;
    [SerializeField] private float SmokeScreenTime = 0.5f;
    [SerializeField] private float BurnupRestartTimer = 1.0f;


    private bool OnGround = false;
    private bool BeenBurnt = false;

    void Start()
    {
        if (Fire == null)
            Fire = FindObjectOfType<Fire>();
        InitialSceneHealthSetup(GameManager.PlayerHealth);
        GameManager.PaperSceneStartHealth = GameManager.PlayerHealth;
    }

    void Update()
    {
        Vector2 velocity = Rigidbody.velocity;

        CheckForGround();

        if(Input.GetKey(RightKey))
        {
            velocity = RunAcceleration(velocity, Direction.RIGHT);
        }

        if (Input.GetKey(LeftKey))
        {
            velocity = RunAcceleration(velocity, Direction.LEFT);
        }

        if(OnGround && !Input.GetKey(LeftKey) && !Input.GetKey(RightKey))
        {
            velocity = RunDeceleration(velocity);
        }

        if(!OnGround && CanMoveInAir && !Input.GetKey(LeftKey) && !Input.GetKey(RightKey))
        {
            velocity = AirDeceleration(velocity);
        }

        if(Input.GetKeyDown(JumpKey))
        {
            Jump();
        }

        Rigidbody.velocity = velocity;

        SetAnimations();
    }

    private void CheckForGround()
    {
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - Collider.size.y/2);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, GroundCheckRayLEngth);
        OnGround = hit && hit.collider.CompareTag("Platform");
    }

    private void SetAnimations()
    {
        if(OnGround)
        {
            Animator.SetBool("OnGround", true);
        }
        else
        {
            Animator.SetBool("OnGround", true);
        }

        if (OnGround
            &&
            (Input.GetKey(LeftKey)
        || Input.GetKey(RightKey)))
        {
            Animator.SetBool("IsWalking", true);
        }
        else
        {
            Animator.SetBool("IsWalking", false);
        }

    }

    private Vector2 RunAcceleration(Vector2 pVelocity, Direction pDirection)
    {
        if (!CanMoveInAir
           && !OnGround)
            return pVelocity;
        if (pVelocity.magnitude >= MaxRunspeed)
            return pVelocity;
        if (pDirection == Direction.RIGHT)
        {
            pVelocity += (Vector2.right * (MaxRunspeed / SecondsToMaxSpeed) * Time.deltaTime);
        }
        else if (pDirection == Direction.LEFT)
        {
            pVelocity += (Vector2.left * (MaxRunspeed / SecondsToMaxSpeed) * Time.deltaTime);
        }

        if (OnGround)
        {
            PlayWalkSound();
        }

        return pVelocity;
    }

    private void PlayWalkSound()
    {
        AudioSource freeSource = null;
        foreach (AudioSource s in Sources)
        {
            if (!s.isPlaying)
            {
                freeSource = s;
            }
        }

        if (freeSource == null)
            return;

        freeSource.clip = WalkSounds[Random.Range(0, WalkSounds.Capacity)];
        freeSource.pitch = 1 + Random.Range(-PitchVariety, PitchVariety);

        freeSource.Play();
    }

    private Vector2 RunDeceleration(Vector2 pVelocity)
    {
        float velocityMagnitude = pVelocity.magnitude;
        velocityMagnitude -= (MaxRunspeed / SecondsToNoSpeed) * Time.deltaTime;
        if (velocityMagnitude < 0)
        {
            velocityMagnitude = 0;
        }
        pVelocity.x = pVelocity.normalized.x * velocityMagnitude;
        return pVelocity;
    }

    private Vector2 AirDeceleration(Vector2 pVelocity)
    {
        float horizontalMagnitude = pVelocity.magnitude;

        horizontalMagnitude -= (MaxRunspeed / SecondsToNoSpeed) * Time.deltaTime;
        if (horizontalMagnitude < 0)
        {
            horizontalMagnitude = 0;
        }

        pVelocity.x = pVelocity.normalized.x * horizontalMagnitude;
        return pVelocity;
    }

    private void Jump()
    {
        if(OnGround)
        {
            Rigidbody.AddForce(JumpForce * Vector2.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D pCollider)
    {
        if (pCollider.CompareTag("Fire"))
        {
            if (!BeenBurnt)
            {
                Burn();
            }
        }
        if (pCollider.CompareTag("FireExitTrigger"))
        {
            Fire.ExitLevel();
        }
        if (pCollider.CompareTag("PuddleCutsceneTrigger"))
        {
            Animator.SetBool("IsWalking", false);
            Fire.StartPuddleCutScene();
            Rigidbody.velocity = Vector2.zero;
            pCollider.gameObject.SetActive(false);
            enabled = false;
        }
        if (pCollider.CompareTag("BurnSequenceTrigger"))
        {
            Animator.SetBool("IsWalking", false);
            Rigidbody.velocity = Vector2.zero;
            enabled = false;
        }

        if (pCollider.CompareTag("Water"))
        {
            GameManager.ReloadScene();
        }
    }

    private void Burn()
    {
        GameManager.PlayerHealth -= FireDamage;

        Fire.ChangeHealth(FireDamage);

        BeenBurnt = true;
        SetHealth(GameManager.PlayerHealth);
    }

    private void InitialSceneHealthSetup(float pCurrentHealth)
    {
        if (pCurrentHealth >= 1.0f)
        {
            return;
        }
        if (pCurrentHealth > 0.33f)
        {
            BurnObjects[0].SetActive(true);
            foreach (FireParticle f in BurnObjects[0].GetComponentsInChildren<FireParticle>())
            {
                f.gameObject.SetActive(false);
            }
            BurnObjects[1].SetActive(false);
        }
        else if (pCurrentHealth > 0.0f)
        {
            BurnObjects[0].SetActive(true);
            foreach (FireParticle f in BurnObjects[0].GetComponentsInChildren<FireParticle>())
            {
                f.gameObject.SetActive(false);
            }
            BurnObjects[1].SetActive(true);
            foreach (FireParticle f in BurnObjects[1].GetComponentsInChildren<FireParticle>())
            {
                f.gameObject.SetActive(false);
            }
        }
    }
    
    private void SetHealth(float pCurrentHealth)
    {
        if(pCurrentHealth >= 1.0f)
        {
            return;
        }
        if (pCurrentHealth > 0.33f)
        {
            BurnObjects[0].SetActive(true);
            BurnObjects[1].SetActive(false);
        }
        else if (pCurrentHealth > 0.0f)
        {
            BurnObjects[0].SetActive(true);
            BurnObjects[1].SetActive(true);
        }
        else if (pCurrentHealth <= 0.0f)
        {
            StartCoroutine(BurnUp());
        }
    }

    private IEnumerator BurnUp()
    {
        BurnObjects[2].SetActive(true);
        foreach (GameObject g in DeathFires)
        {
            g.SetActive(true);
            yield return new WaitForSeconds(DeathFireSpreadDelay);
        }
        BurnUpSmoke.SetActive(true);
        yield return new WaitForSeconds(SmokeScreenTime);
        BurnUpSmoke.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject g in BurnObjects)
        {
            g.SetActive(false);
        }
        enabled = false;
        yield return new WaitForSeconds(BurnupRestartTimer);
        GameManager.PlayerHealth = 1.0f;
        GameManager.FireHealth = 0.5f;
        GameManager.LoadScene(0);
        yield return null;
    }
}
