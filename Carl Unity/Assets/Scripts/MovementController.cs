using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MovementController : MonoBehaviour
{
    private float colliderRadius = 1.1f;

    public LayerMask carLayermask;
    public BoxCollider2D boxCollider2D;

    #region Lanes
    [SerializeField] private float laneSwitchSpeed;
    public int lane;                               // 0, 1 or 2
    private Vector3 initPos, pos;
    private float startTime, switchCompletion;
    private bool shouldMoveLanes;
    #endregion
    
    #region MovingRight
    public float maxSpeed, boostSpeedBonus, speedStep, dragStep, tumbleStep, tumbleStunTime;
    public ParticleSystem tumbleParticles;

    private float crtSpeed;
    private bool isTumbling;
    #endregion

    #region Components
    public AudioSource ouchSource;
    private Rigidbody2D rb2D;
    private AudioSource audioSource;
    #endregion

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        colliderRadius = boxCollider2D.size.x / 2f + .5f;
    }

    private void Start()
    {
        shouldMoveLanes = false;
        crtSpeed = 0f;
    }

    private void Update() {
        CalculateSpeed();
    }

    private void FixedUpdate() {
        rb2D.MovePosition(rb2D.position + Vector2.right * crtSpeed * Time.fixedDeltaTime);
        MoveLanes();
    }

    public float GetSpeed() {
        return crtSpeed;
    }

    public bool GetIsTumbling() {
        return isTumbling;
    }

    private void CalculateSpeed() {
        if(crtSpeed > 0 || isTumbling) {
            crtSpeed -= dragStep;
            if(isTumbling) {
                crtSpeed -= tumbleStep;
            }

            if(crtSpeed <= 0f) {
                crtSpeed = 0f;
            
                Invoke("StopTumbling", tumbleStunTime);
            }
        }
    }

    private void StopTumbling() {
        if(isTumbling) {
            isTumbling = false;
        }
    }

    private void MoveLanes() {
        if(shouldMoveLanes) {
            float dir = 1;
            if((pos - initPos).y < 0)
                dir = -1;

            rb2D.AddForce(dir * Vector2.up * laneSwitchSpeed * Time.fixedDeltaTime);
            if(dir * (pos.y - transform.position.y) < .01f)
                transform.position = new Vector3(transform.position.x, pos.y, pos.z);
            if(switchCompletion >= 1f || transform.position.y == pos.y)
                shouldMoveLanes = false;
        }
    }

    public void IncreaseSpeed() {
        if(isTumbling)
            return;

        if(crtSpeed + speedStep <= maxSpeed)
            crtSpeed += speedStep;
    }

    public void Tumble() {
        isTumbling = true;
        audioSource.Play();
        ParticleSystem.MainModule settings = tumbleParticles.main;
        settings.startColor = Color.red;
        tumbleParticles.Play();
    }

    public bool MoveLaneUp() {
        //Debug.Log("move up from " + lane);
        if(lane != 0 && !LaneOccupied(lane, lane - 1)) {
            lane--;

            initPos = transform.position;
            pos = new Vector3(
                transform.position.x, 
                Lanes.height[lane],
                Lanes.height[lane]
                );
            startTime = Time.time;
            shouldMoveLanes = true;
            return true;
        }
        return false;
    }

    public bool MoveLaneDown() {
        //Debug.Log("move down from " + lane);
        if(lane != 2 && !LaneOccupied(lane, lane + 1)) {
            lane++;
            initPos = transform.position;
            pos = new Vector3(
                transform.position.x, 
                Lanes.height[lane],
                Lanes.height[lane]
                );
            startTime = Time.time;
            shouldMoveLanes = true;
            return true;
        }
        return false;
    }

    private bool LaneOccupied(int crtLane, int newLane) {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(transform.position.x - colliderRadius, transform.position.y), 
            new Vector2(0, Lanes.height[newLane] - Lanes.height[crtLane]), 
            Mathf.Abs(Lanes.height[newLane] - Lanes.height[crtLane]), 
            carLayermask);

        RaycastHit2D hit2 = Physics2D.Raycast(
            new Vector2(transform.position.x + colliderRadius, transform.position.y), 
            new Vector2(0, Lanes.height[newLane] - Lanes.height[crtLane]), 
            Mathf.Abs(Lanes.height[newLane] - Lanes.height[crtLane]), 
            carLayermask);

        if (hit.collider != null || hit2.collider != null)
        {
            // Debug.Log("Did Hit " + hit.transform.name);
            return true;
        }
        else
        {
            // Debug.Log("Did not Hit " + hit.collider + " " + hit2.collider);
            return false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Boost") {
            ParticleSystem.MainModule settings = tumbleParticles.main;
            settings.startColor = Color.green;
            tumbleParticles.Play();
            crtSpeed += boostSpeedBonus;
            //ouchSource.Stop();
            ouchSource.Play();
        }
    }
}
