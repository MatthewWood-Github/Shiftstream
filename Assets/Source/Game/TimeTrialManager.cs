using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeTrialManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private CarSettings carSettings;

    public GameObject dft;
    public Camera mainCamera;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI countdownTextShadow;
    public GameObject countdown;

    public float maxTimeBeforeCountdown = 2;
    public float maxCountdownTime = 4;

    public float timeBeforeCountdown = 0;
    public float countdownTime = 4;

    ///<summary> Parent object of all cars in the race. </summary>
    [SerializeField] private Transform carsParent;

    ///<summary> List containing all cars. </summary>
    [SerializeField] private List<Transform> cars;

    public int currentRaceState = 0;

    public enum RaceState
    {
        startLine = 0,
        inProgress = 1,
        finished = 2,
    }

    private void Countdown()
    {
        timeBeforeCountdown += Time.deltaTime;
        if (timeBeforeCountdown > maxTimeBeforeCountdown)
        {
            countdown.SetActive(true);
            countdownText.enabled = true;
            countdownTextShadow.enabled = true;
            countdownTime -= Time.deltaTime;
            
            countdownText.text = Mathf.Floor(countdownTime).ToString();
            countdownTextShadow.text = Mathf.Floor(countdownTime).ToString();

            if (countdownTime < 1)
            {
                foreach (Transform car in cars)
                {
                    car.gameObject.GetComponent<CarSettings>().currentMovement.enabled = true;
                }

                countdownText.text = "GO!";
                countdownTextShadow.text = "GO!";
            }

            if (countdownTime < 0)
            {
                countdownText.enabled = false;
                countdownTextShadow.enabled = false;
                countdown.SetActive(false);

                currentRaceState = (int)RaceState.inProgress;
            }
        }
    }

    private void FinishRace()
    {
        if (carSettings.raceFinished == true)
        {
            this.GetComponent<Timer>().enabled = false;

            currentRaceState = (int) RaceState.finished;

            this.dft.SetActive(true);
            this.mainCamera.GetComponent<PlayerCamera>().EndCamera();
            this.player.GetComponent<CarSettings>().toggleMovement();

            float timeInSeconds = this.GetComponent<Timer>().timeInSeconds;

            PlayerSettings.Settings.currentTime = timeInSeconds;

            if (PlayerSettings.Settings.fastestTime == 0)
            {
                PlayerSettings.Settings.fastestTime = timeInSeconds;
            }

            if (timeInSeconds < PlayerSettings.Settings.fastestTime)
            {
                PlayerSettings.Settings.fastestTime = timeInSeconds;
            }
        }
    }
    
    void Start()
    {
        carSettings = player.GetComponent<CarSettings>();

        // Add all car objects to cars list
        for (int x = 0; x < carsParent.childCount; x++)
            cars.Add(carsParent.transform.GetChild(x));
    }

    void Update()
    {
        if (currentRaceState == 0)
            Countdown();

        if (currentRaceState == 1)
            this.GetComponent<Timer>().enabled = true;

        FinishRace();
    }
}
