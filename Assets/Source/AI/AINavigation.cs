using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINavigation : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject carObj;
    [SerializeField] private BaseMove car;
    [SerializeField] private GameObject track;
    public Vector3 minimum = Vector3.zero;
    public float distanceToCheckpoint;
    
    [Header("Boundary Steering")]
    public float steerRatioInbounds;   
    public float boundaryActivationRange;
    public float distanceToBoundary;

    [Header("Checkpoint Steering")]
    public float steerRatioCheckpoints;
    public float checkpointDeadZone;
    public float distanceDeadZone;
    public float reactionAngle;

    [Header("Vertices")]
    Mesh mesh;
    private Vector3[] vertices;

    [Header("Checkpoints")]
    public Transform checkpointParent;
    public List<Transform> checkpoints;
    public int maxCheckpoints;

    public Transform target;
    public int targetIndex = 0;

    [Range(-100f, 100f)]
    public float trackx;
    [Range(-100f, 100f)]
    public float tracky;
    [Range(-100f, 100f)]
    public float trackz;

    private void CalculateDistance()
    {
        foreach (Vector3 VPos in vertices)
        {
            //Debug.DrawRay(VPos, Vector3.up * 1f, Color.red);
            if ((Vector3.Distance(carObj.transform.position, VPos) < (Vector3.Distance(carObj.transform.position, minimum))))
                minimum = VPos;
        }
        Debug.DrawRay(minimum, Vector3.up * 1f, Color.red);
        Debug.DrawLine(carObj.transform.position, minimum, Color.red, 0f);
        
    }

    public float SteerTowardsCheckpoint()
    {
        Collider c = target.GetComponent<Collider>();

        Vector3 carHoriz = carObj.transform.forward;
        Vector3 targetHoriz = c.ClosestPoint(carObj.transform.position) - carObj.transform.position;
        float angle = Vector3.Angle(carHoriz, targetHoriz);

        if (angle < checkpointDeadZone * steerRatioCheckpoints) return 0;

        distanceToCheckpoint = Vector3.Distance(carObj.transform.position, target.position);

        if (Mathf.Abs(distanceToCheckpoint) < distanceDeadZone) return 0;

        Vector3 localPos = carObj.transform.InverseTransformPoint(target.position);
        if (localPos.x < 0)
            distanceToCheckpoint *= -1;

        return steerRatioCheckpoints * Mathf.Sign(distanceToCheckpoint);
    }

    public float KeepInbounds()
    {
        distanceToBoundary = Vector3.Distance(carObj.transform.position, minimum);
        if (Mathf.Abs(distanceToBoundary) > boundaryActivationRange)
            return 0;

        Vector3 localPos = carObj.transform.InverseTransformPoint(minimum);
        if (localPos.x > 0)
            distanceToBoundary *= -1;
        
        return steerRatioInbounds / distanceToBoundary;
    }

    public int Stop()
    {
        Collider c = target.GetComponent<Collider>();

        Vector3 carHoriz = carObj.transform.forward;
        Vector3 targetHoriz = c.ClosestPoint(carObj.transform.position) - carObj.transform.position;
        float angle = Vector3.Angle(carHoriz, targetHoriz);

        distanceToBoundary = Vector3.Distance(carObj.transform.position, minimum);
        if (Mathf.Abs(distanceToBoundary) < 2)
            return 0;

        if (angle > reactionAngle) return 0;

        else return 1;
    }

    private void CheckPointCollision()
    {
        if (Mathf.Abs(Vector3.Distance(carObj.transform.position, target.position)) <= 35)
        {
            if (targetIndex == checkpoints.Count - 1)
            {
                targetIndex = 0;
                return;
            }

            target = checkpoints[++targetIndex];
        }
    }

    void Awake()
    {
        car = carObj.GetComponent<BaseMove>();
        mesh = track.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        for (int x = 0; x < vertices.Length; x++)
        {
            vertices[x].x *= trackx;
            vertices[x].y *= tracky;
            vertices[x].z *= trackz;
            vertices[x] = Quaternion.Euler(90f, 0, 0) * (vertices[x] - track.transform.position);
        }

        for (int x = 0; x < checkpointParent.transform.childCount; x++)
            checkpoints.Add(checkpointParent.GetChild(x));

        maxCheckpoints = checkpoints.Count;

        target = checkpoints[0];
    }

    private void Update()
    {
        CheckPointCollision();
    }

    private void FixedUpdate()
    {
        
        //CalculateDistance();
        Collider c = target.GetComponent<Collider>();
        Debug.DrawLine(carObj.transform.position, c.ClosestPoint(car.transform.position), Color.yellow, 0f);
    }
}
