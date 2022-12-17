using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary> Manages the main camera position. </summary>
public class PlayerCamera : MonoBehaviour
{
    [Header("Transforms")]
    ///<summary> Camera's look target. </summary>
    private Transform target;

    ///<summary> Camera's local position. </summary>
    private Transform pos;

    ///<summary> Target car gameobject. </summary>
    public GameObject targetCar { get; private set; }

    ///<summary> Local start position of <see cref="target"/>. </summary>
    ///<remarks> Used to reset the camera's rotation. </remarks>
    private Vector3 targetStart;

    ///<summary> Local start position of <see cref="pos"/>. </summary>
    ///<remarks> Used to reset the camera's rotation. </remarks>
    private Vector3 posStart;

    [Header("Speed")]
    ///<summary> Speed that the camera moves to <see cref="pos"/>. </summary>
    private float speed = 15;

    ///<summary> Speed that the camera rotates after race is complete. </summary>
    private float endCameraSpeed = 0.005f;

    ///<summary> Rotation speed of the camera. </summary>
    private float cameraSpinSpeed = 3000;

    ///<summary> Player's horizontal camera input. </summary>
    public float horizontal { get; set; }

    [Header("Spectate")]
    ///<summary> Parent object of all cars in the race. </summary>
    ///<remarks> Used to get all cars that you can spectate. </remarks>
    [SerializeField] private Transform carsParent;

    ///<summary> List containing all cars that you can spectate. </summary>
    [SerializeField] private List<Transform> cars;

    ///<summary> The first car which the camera is attached to. </summary>
    ///<value> The first car in the <see cref="cars"/> list. Should be set to the player. </value>
    private int carIndex = 0;

    [Header("End")]
    ///<summary> Has the game ended? </summary>
    private bool ended = false;

    ///<summary> Gets player input. </summary>
    private void GetPlayerHorizontal()
    {
        horizontal = Input.GetAxis("Camera");

        // Ensures that "horizontal" rounds to zero.
        horizontal = Mathf.Round(horizontal * 100f) / 100f;
    }

    ///<summary> Applies camera rotation about the target car. </summary>
    ///<remarks> Affected by player input. </remarks>
    private void ApplyPlayerCameraRotation()
    {
        target.transform.RotateAround(targetCar.transform.position, targetCar.transform.up, cameraSpinSpeed * horizontal * Time.deltaTime);
        pos.transform.RotateAround(targetCar.transform.position, targetCar.transform.up, cameraSpinSpeed * horizontal * Time.deltaTime);
    }

    ///<summary> Resets the camera to a forward facing rotation. </summary>
    private void ResetCamera()
    {
        if (Input.GetAxisRaw("Reset Camera") == 1)
        {
            target.transform.rotation = Quaternion.identity;
            pos.transform.rotation = Quaternion.identity;

            target.transform.localPosition = targetStart;
            pos.transform.localPosition = posStart;
        }
    }

    ///<summary> Creates a zoom effect for the camera. </summary>
    ///<remarks> Makes the player feel more effects from changes in speed. </remarks>
    private void CameraZoom()
    {
        float step = speed * Time.deltaTime;
        transform.LookAt(target.position);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos.transform.position, step);
    }

    ///<summary> Creates a zoom effect for the camera. </summary>
    private void SpectateCamera()
    {
        targetCar = cars.ElementAt(carIndex).gameObject;
        target = targetCar.transform.Find("CameraTarget");
        pos = targetCar.transform.Find("CameraPosition");

        if (Input.GetButtonDown("Spectate"))
        {
            if (carIndex == cars.Count - 1)
                carIndex = 0;
            else
                carIndex += 1;
        }
    }

    ///<summary> Changes the camera target and speed after the race has ended. </summary>
    public void EndCamera()
    {
        // Fire once.
        if (ended == false)
        {
            pos.transform.localPosition = Vector3.Scale(posStart, new Vector3(1.5f, 1.2f, 1.5f));
            target.transform.localPosition = Vector3.Scale(targetStart, new Vector3(1.5f, 1.2f, 1.5f));
            ended = true;
        }
            
        horizontal = 0;
        horizontal += endCameraSpeed;
    }

    void Start()
    {
        // Add all car objects to cars list
        for (int x = 0; x < carsParent.childCount; x++)
            cars.Add(carsParent.transform.GetChild(x));

        targetCar = cars.ElementAt(carIndex).gameObject;

        target = targetCar.transform.Find("CameraTarget");
        pos = targetCar.transform.Find("CameraPosition");

        targetStart = target.transform.localPosition;
        posStart = pos.transform.localPosition;
    }

    private void Update()
    {
        if (targetCar.GetComponent<CarSettings>().raceFinished == true)
            return;

        SpectateCamera();
        GetPlayerHorizontal();
        ResetCamera();
        
    }

    void FixedUpdate()
    {
        CameraZoom();
        ApplyPlayerCameraRotation();
    }
}
