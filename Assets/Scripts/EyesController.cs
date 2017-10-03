using UnityEngine;

public class EyesController : MonoBehaviour {
    public GameObject[] eyelids;
    public GameObject[] eyes;
    public Transform pointOfInterest;
    private Animator[] eyelidAnimators;
    private Material[] eyeMaterials;
    private float timeToNextBlink = 5.0f;

    // Use this for initialization
    void Start() {
        if (eyelids.Length > 0) {
            eyelidAnimators = new Animator[eyelids.Length];
            for (int index = 0; index < eyelids.Length; index++) {
                GameObject eyelid = eyelids[index];
                Animator eyelidAnimator = eyelid.GetComponent<Animator>();
                if (eyelidAnimator != null) {
                    eyelidAnimators[index] = eyelidAnimator;
                }
                else {
                    Debug.LogError("Failed to find animator on object " + eyelid.name);
                }
            }
        }

        if (eyes.Length > 0) {
            eyeMaterials = new Material[eyes.Length];

            for (int index = 0; index < eyes.Length; index++) {
                GameObject eye = eyes[index];

                MeshRenderer meshRenderer = eye.GetComponent<MeshRenderer>();

                if (meshRenderer != null) {
                    Material eyeMaterial = meshRenderer.material;
                    if (eyeMaterial != null) {
                        eyeMaterials[index] = eyeMaterial;
                    }
                    else {
                        Debug.LogError("Failed to find material on object " + eye.name);
                    }
                }
                else {
                    Debug.LogError("Failed to find mesh renderer on object " + eye.name);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (eyelidAnimators.Length > 0) {
            timeToNextBlink -= Time.deltaTime;

            if (timeToNextBlink <= 0.0f) {
                for (int index = 0; index < eyelidAnimators.Length; index++) {
                    eyelidAnimators[index].SetTrigger("blink");
                }

                timeToNextBlink = Random.Range(2.0f, 5.0f);
            }

            if (eyes.Length > 0) {
                if (pointOfInterest != null) {
                    for (int index = 0; index < eyes.Length; index++) {
                        // Calculate the angle between the eye and target point and use that to offset the UV coords
                        GameObject eye = eyes[index];
                        Vector3 facing = gameObject.transform.forward;

                        // Temporarily use the up vector because our test mesh is facing up
                        // TODO: Use a model that actually faces positive Z
                        //Vector3 facing = eye.transform.up;
                       
                        Vector3 directionToPointOfInterest = eye.transform.position - pointOfInterest.position;

                        Vector3 directionXZ = new Vector3(directionToPointOfInterest.x, facing.y, directionToPointOfInterest.z);
                        Vector3 directionY = new Vector3(facing.x, directionToPointOfInterest.y, facing.z);

                        float productXZ = Vector3.Dot(directionXZ, facing);
                        float productY = Vector3.Dot(directionY, gameObject.transform.up);

                        //Debug.Log("XZ = " + productXZ);
                        //Debug.Log("Y = " + productY);
                        /*
                        Quaternion rotation = Quaternion.LookRotation(directionToPointOfInterest);
                        Vector3 angles = rotation.eulerAngles;

                        float offsetX = Mathf.Clamp(Mathf.Deg2Rad * angles.x, -0.5f, 0.5f);
                        float offsetY = Mathf.Clamp(Mathf.Deg2Rad * angles.y, -0.5f, 0.5f);
                        */

                        float offsetX = Mathf.Clamp(Mathf.Deg2Rad * productXZ, -0.5f, 0.5f);
                        float offsetY = Mathf.Clamp(Mathf.Deg2Rad * productY, -0.5f, 0.5f);

                        eyeMaterials[index].SetFloat("_OffsetX", offsetX);
                        eyeMaterials[index].SetFloat("_OffsetY", offsetY);
                    }
                }
                else {
                    for (int index = 0; index < eyeMaterials.Length; index++) {
                        eyeMaterials[index].SetFloat("_OffsetX", 0.0f);
                        eyeMaterials[index].SetFloat("_OffsetY", 0.0f);
                    }
                }
            }
        }
    }
}
