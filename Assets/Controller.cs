using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    private Vector3 initCamPos;
    float sin, cos;

    public float speed = 3f;
    public float jumpSpeed = 1f;
    public float maxSpeed = 100f;

    public float mouseCamDistance = 2f;

    public GameObject normal;
    public GameObject shifted;

    public Animator animator;

    public bool isShifted = false;

    private bool collided = false;

	// Use this for initialization
	void Start () {
        initCamPos = Camera.main.transform.position;

        sin = Mathf.Sin(-Camera.main.transform.rotation.eulerAngles.x * Mathf.Deg2Rad);
        cos = Mathf.Cos(-Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);

        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        float dx = (Input.mousePosition.x - Screen.width / 2) / Screen.width;
        float dy = (Input.mousePosition.y - Screen.height / 2) / Screen.height;

        float worldDx = cos * dx - sin * dy;
        float worldDy = sin * dx + cos * dy;

        // Move camera
        Vector3 campPos = transform.position + initCamPos + new Vector3(worldDx * mouseCamDistance, 0, worldDy * mouseCamDistance);
        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, campPos, Time.deltaTime * 4f);

       

        
        // Move character
        Vector2 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            direction.y += 1;
        if (Input.GetKey(KeyCode.A))
            direction.x -= 1;
        if (Input.GetKey(KeyCode.S))
            direction.y -= 1;
        if (Input.GetKey(KeyCode.D))
            direction.x += 1;
        Vector3 movement = new Vector3(cos * direction.x - sin * direction.y, 0, sin * direction.x + cos * direction.y);
        movement.Normalize();

        // Check if running or walking (or nothing)
        bool running = false;
        if (Input.GetKey(KeyCode.LeftShift)) 
            running = true;

        bool moving = false;
        if (movement.magnitude >= 1)
            moving = true;

        if (moving) {
            if (running) {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            } else {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
        } else
            animator.SetBool("isWalking", false);

        float speedMod = 1f;
        if (running)
            speedMod = 2f;


        transform.Translate(movement * speedMod * speed * Time.deltaTime, Space.World);

        

        // Rotate character
        float smoothRot = 4f;
        if (moving) // If moving, rotate in that direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(-movement.z, movement.x) * Mathf.Rad2Deg + 90, Vector3.up), Time.deltaTime * smoothRot);
        else // Else in mouse direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -Mathf.Atan2(worldDy, worldDx) * Mathf.Rad2Deg + 90, 0), Time.deltaTime * smoothRot);
       


        // Jump
        if (isShifted && collided && Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<Rigidbody>().velocity += jumpSpeed * Vector3.up;


        }

        // Shape shift
        if (Input.GetKeyDown(KeyCode.F)) {
            shiftChar(!isShifted);
            GetComponent<AudioSource>().pitch = Random.Range(.8f, 1.2f);
            GetComponent<AudioSource>().Play();
        }
    }

    public void shiftChar(bool b) {
        if (isShifted != b) {
            isShifted = !isShifted;
            shifted.SetActive(isShifted);
            normal.SetActive(!isShifted);
        }

    }

    void OnCollisionEnter(Collision collision) {
        collided = true;
    }
    void OnCollisionExit(Collision collision) {
        collided = false;
    }
}


