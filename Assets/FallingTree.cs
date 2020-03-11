using UnityEngine;
using System.Collections;

public class FallingTree : MonoBehaviour {

    GameObject text;

    bool collided = false;

    bool activated = false;

    bool done = false;

    private Quaternion newRot;

    Controller controller;

	// Use this for initialization
	void Start () {
        text = GameObject.Find("TreeText");
        text.SetActive(false);

        newRot = Quaternion.Euler(0, 0, -100) * transform.rotation;
        controller = GameObject.Find("CharContainer").GetComponent<Controller>();

    }
	
	// Update is called once per frame
	void Update () {
        if (done)
            return;

	    if (collided && !controller.isShifted) {
            if (Input.GetKeyDown(KeyCode.E)) {
                controller.enabled = false;

                activated = true;
                text.SetActive(false);
            }
        }

        if (activated) {
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime);

            if (Mathf.Abs(transform.rotation.eulerAngles.x % 360) < 11) {
                done = true;
                controller.enabled = true;
            }
        }
	}

    void OnCollisionEnter(Collision collision) {
        if (!done && !controller.isShifted) {
            text.SetActive(true);
            collided = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        text.SetActive(false);
        collided = false;
    }
}
