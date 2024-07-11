using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class WorkoutManager : MonoBehaviour
{
    public string jsonFileName = "WorkoutInfoJSONAssignment.json";
    public Root workoutData;
    public TMP_Text titleText;
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public GameObject ballPrefab;

    void Start()
    {
        LoadJson();
        GenerateUI();
    }

    void LoadJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            workoutData = JsonUtility.FromJson<Root>(dataAsJson);
            Debug.Log("JSON Data Loaded");
            Debug.Log("Project Name: " + workoutData.ProjectName);
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    void GenerateUI()
    {
        if (titleText != null)
        {
            titleText.text = workoutData.ProjectName;
        }

        foreach (var workout in workoutData.workoutInfo)
        {
            GameObject button = Instantiate(buttonPrefab, buttonParent);
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = workout.workoutName + "\n" + workout.description;
            }
            else
            {
                Debug.LogError("TMP_Text component not found in button prefab.");
            }
            button.GetComponent<Button>().onClick.AddListener(() => SpawnBalls(workout.workoutDetails));
        }
    }

    void SpawnBalls(List<WorkoutDetail> workoutDetails)
    {
        foreach (var detail in workoutDetails)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3((float)GetBallDirection(detail.ballDirection), 1, 0);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, (float)detail.speed);
        }
    }

    float GetBallDirection(float ballDirection)
    {
        if (ballDirection == 0.5f)
        {
            return 0.5f;
        }
        else if (ballDirection == 0f)
        {
            return 0f;
        }
        else if (ballDirection == -0.5f)
        {
            return -0.5f;
        }
        else
        {
            return 0f; // Default case, can be modified as needed
        }
    }
}

[System.Serializable]
public class Root
{
    public string ProjectName;
    public int numberOfWorkoutBalls;
    public List<WorkoutInfo> workoutInfo;
}

[System.Serializable]
public class WorkoutDetail
{
    public int ballId;
    public float speed;
    public float ballDirection;
}

[System.Serializable]
public class WorkoutInfo
{
    public int workoutID;
    public string workoutName;
    public string description;
    public string ballType;
    public List<WorkoutDetail> workoutDetails;
}
