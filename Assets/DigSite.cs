using UnityEngine;
using System.Collections;

public class DigSite : MonoBehaviour {

    public GameObject textPrefab;
    public GameObject disableOnDig;

    GameObject text;

    bool collided;

    Controller controller;

    float digTime;
    bool dug = false;

    Vector3 target;

    float waitTime = 2.5f;

	// Use this for initialization
	void Start () {
        text = (GameObject) GameObject.Instantiate(textPrefab);
        text.transform.parent = transform;
        text.transform.position = transform.position;

        text.GetComponentInChildren<TextMesh>().text = "Press E to dig";
        text.SetActive(false);

        controller = GameObject.Find("CharContainer").GetComponent<Controller>();

        digTime = float.MaxValue;

        if (disableOnDig == null)
            waitTime = 1f;
    }
	
	// Update is called once per frame
	void Update () {
	    if (collided && controller.isShifted && Input.GetKeyDown(KeyCode.E)) {
            controller.enabled = false;
            digTime = Time.time;
            dug = true;

            if (disableOnDig == null) {
                text.GetComponentInChildren<TextMesh>().text = "Found nothing";
            } else
                target = disableOnDig.transform.position - Camera.main.transform.forward * 6f;

            Animator animator = controller.animator;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        if (dug && disableOnDig != null) {
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, target, Time.deltaTime * 3f);
        }

        // After 3 seconds, disable the target and the dig site
        if (Time.time - 1.5 > digTime && disableOnDig != null) {
            disableOnDig.SetActive(false);
        }
        if (Time.time - waitTime > digTime) {
            controller.enabled = true;
            gameObject.SetActive(false);
        }
	}

    void OnCollisionEnter(Collision collision) {
        if (controller.isShifted) {
            text.SetActive(true);
            collided = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        text.SetActive(false);
        collided = false;
    }
}
