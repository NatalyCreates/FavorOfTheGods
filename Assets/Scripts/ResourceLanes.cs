using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLanes : MonoBehaviour {

    public GameObject[] resourcePrefabs;

    public Transform[] lanes;

    public float laneUpdateRate = 1.0f;

    private float laneUpdateCounter = 0f;

    private bool isPaused = false;

    public Enums.Players playerInThisLane;

    void Awake() {
        Player.OnPauseMovement += OnPause;
        Player.OnResumeMovement += OnResume;
    }

    void OnDestroy() {
        Player.OnPauseMovement -= OnPause;
        Player.OnResumeMovement -= OnResume;
    }

    public void OnPause(Enums.Players pausedPlayer) {
        if (pausedPlayer == playerInThisLane)
        {
            isPaused = true;
        }
    }

    public void OnResume(Enums.Players resumedPlayer) {
        if (resumedPlayer == playerInThisLane)
        {
            isPaused = false;
        }
    }
    
    void Update()
    {
        if (!isPaused) laneUpdateCounter += Time.deltaTime;
        if (laneUpdateCounter >= laneUpdateRate && !isPaused)
        {
            laneUpdateCounter = 0f;
            foreach(Transform t in lanes)
            {
                GameObject r = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];
                PopulateLane(r, t);
            }
        }
    }

    void PopulateLane(GameObject resource, Transform lane)
    {
        GameObject resourceObject = Instantiate(resource, lane.position, Quaternion.identity, lane.transform);
        switch (lane.parent.name)
        {
        case "Lanes1":
            resourceObject.GetComponent<Resource>().playerWhoCanCollectThis = Enums.Players.Odysseus;
            break;
        case "Lanes2":
            resourceObject.GetComponent<Resource>().playerWhoCanCollectThis = Enums.Players.Achilles;
            break;
        case "Lanes3":
            resourceObject.GetComponent<Resource>().playerWhoCanCollectThis = Enums.Players.Theseus;
            break;
        }
    }
}
