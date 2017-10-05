using UnityEngine;

public class EyesController : MonoBehaviour {
    public GameObject[] eyelids;
    public GameObject[] eyes;
    public Transform pointOfInterest;
    private Animator[] eyelidAnimators;
    private Material[] eyeMaterials;

    [Range(0.1f, 0.5f)]
    public float uvOffsetLimit = 0.25f;

    [Range(0.01f, 1.0f)]
    public float eyeInterpolationRate = 0.1f;

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
            UpdateEyelids();
        }

        if (eyes.Length > 0) {
            UpdateEyes();
        }
    }

    private void UpdateEyelids() {
        timeToNextBlink -= Time.deltaTime;

        if (timeToNextBlink <= 0.0f) {
            for (int index = 0; index < eyelidAnimators.Length; index++) {
                eyelidAnimators[index].SetTrigger("blink");
            }

            timeToNextBlink = Random.Range(2.0f, 5.0f);
        }
    }


    private void UpdateEyes() {
        for (int index = 0; index < eyes.Length; index++) {
            Vector2 targetOffset = new Vector2(0,0);
            if(pointOfInterest != null) {
                targetOffset = DetermineEyeOffsetForPointOfInterest(index);
            }
  
            UpdateEyeToLookAtTarget(index, targetOffset);
        }
    }

    private Vector2 DetermineEyeOffsetForPointOfInterest(int index) {
        // Calculate the angle between the eye and target point and use that to offset the UV coords
        GameObject eye = eyes[index];
        Vector3 facing = eye.transform.forward;
        Vector3 directionToPointOfInterest = eye.transform.position - pointOfInterest.position;

        Vector3 directionXZ = new Vector3(directionToPointOfInterest.x, facing.y, directionToPointOfInterest.z);
        directionXZ.Normalize();

        Vector3 directionY = new Vector3(facing.x, directionToPointOfInterest.y, facing.z);
        directionY.Normalize();

        float productXZ = Vector3.Dot(directionXZ, facing);
        float productY = Vector3.Dot(directionY, eye.transform.up);

        //Debug.Log("Facing = " + facing + " Up = " + eye.transform.up);
        //Debug.Log("XZ = " + productXZ + "("+ directionXZ + ")");
        //Debug.Log("Y = " + productY + "(" + directionY + ")");

        // Result of the dot product is as follows:
        // 1 means vectors are parallel in the same direction
        // 0 means vectors are perpendicular (either side)
        // > 0 is vectors are opposing i.e. facing away from each other beyond 90 degrees either side, with -1 being directly opposite
        // We are only interested in the vector in XZ-axes where the resulting dot proiduct is > 0 i.e. in front of the object

        float targetOffsetX = 0.0f;
        float targetOffsetY = 0.0f;

        if (productXZ >= 0.0f) {
            // Do a dot product with the tangent vector of the facing direction to determine which side the direction vector is in relation to the facing direction.
            // This allows us to alter the sign of the final UV offset in the X axis to fit within our -0.5 to 0.5 range.
            float sign = (Vector3.Dot(directionXZ, eye.transform.right) > 0.0f ? -1.0f : 1.0f);
            targetOffsetX = ((1.0f - productXZ) * uvOffsetLimit) * sign;
            targetOffsetY = productY * uvOffsetLimit;
        }

        return new Vector2(targetOffsetX, targetOffsetY);
    }


    private void UpdateEyeToLookAtTarget(int index, Vector2 targetOffset) {
        // Linearly interpolate towards the target offsets to avoid instant changes in the eye offsets
        float offsetX = eyeMaterials[index].GetFloat("_OffsetX");
        float offsetY = eyeMaterials[index].GetFloat("_OffsetY");

        offsetX = Mathf.Lerp(offsetX, targetOffset.x, eyeInterpolationRate);
        offsetY = Mathf.Lerp(offsetY, targetOffset.y, eyeInterpolationRate);

        eyeMaterials[index].SetFloat("_OffsetX", offsetX);
        eyeMaterials[index].SetFloat("_OffsetY", offsetY);
    }
}
