using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    [SerializeField] [Range(0.05f, 2)] private float forceMultiplier = 0.3f;
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    private Rigidbody rb;

    private bool isShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchDown(touch.position);
                    break;

                case TouchPhase.Moved:
                    OnTouchDrag(touch.position);
                    break;

                case TouchPhase.Ended:
                    OnTouchUp(touch.position);
                    break;
            }
        }
    }

    private void OnTouchDown(Vector3 position)
    {
        touchStartPos = position;
    }

    private void OnTouchDrag(Vector3 position)
    {
        Vector3 forceInit = position - touchStartPos;
        Vector3 forceV = new Vector3(forceInit.x, forceInit.y, forceInit.y) * forceMultiplier;

        if (!isShoot)
        {
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
        }
    }

    private void OnTouchUp(Vector3 position)
    {
        DrawTrajectory.Instance.HideLine();
        touchEndPos = position;
        Shoot(touchEndPos - touchStartPos);
    }

    private float torqueMultiplier = 1f;

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;

        rb.AddForce(new Vector3(Force.x, Force.y, Mathf.Abs(Force.y * 2f)) * forceMultiplier);
        Vector3 torque = new Vector3(0, Force.x, 0) * torqueMultiplier;
        rb.AddTorque(torque, ForceMode.Impulse);
        isShoot = true;
        Spawner.Instance.NewSpawnCardRequest();
        StartCoroutine(WaitAndDestroy(gameObject, 2f));
    }
    
    private static IEnumerator WaitAndDestroy(GameObject gameObject, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
    
}