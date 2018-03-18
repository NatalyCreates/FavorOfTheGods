using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Enums.Players playerId;
    Enums.PlayerState currentState;
    internal Dictionary<Enums.Resources, int> resourceCounters;

    internal float progress;
    float timeProgressed;

    int nextDisasterNum;
    Disaster lastDisaster = null;
    List<float> randomDisasterTimes;

    public static event System.Action<Enums.Players> OnPauseMovement;
    public static event System.Action<Enums.Players> OnResumeMovement;
    public static event System.Action<Enums.Players, Enums.PlayerState> OnPlayerStateChange;
    public static event System.Action<Enums.Players, Disaster, bool> OnDisasterEncounter;


    BoxCollider2D myCollider;

    void Awake() {
        myCollider = GetComponent<BoxCollider2D>();
        resourceCounters = new Dictionary<Enums.Resources, int>();
        resourceCounters[Enums.Resources.Wine] = 0;
        resourceCounters[Enums.Resources.Food] = 0;
        resourceCounters[Enums.Resources.Fleece] = 0;
        currentState = Enums.PlayerState.Movement;
        timeProgressed = 0f;
        nextDisasterNum = 0;

        randomDisasterTimes = new List<float>();
        foreach (List<float> timeRange in GlobalNums.DISASTER_TIMES)
        {
            randomDisasterTimes.Add(Random.Range(timeRange[0], timeRange[1]));
        }
    }

    void Update() {
        if (!GameManager.Instance.gameOver)
        {
            switch (currentState)
            {
            case Enums.PlayerState.Movement:
                // Increment Progress Time
                timeProgressed += Time.deltaTime;
                progress = timeProgressed / GlobalNums.TOTAL_JOURNEY_TIME;
                if (progress >= 1f) GameManager.Instance.WinGame(playerId);
                // Check for Disaster Encounters
                if (timeProgressed >= randomDisasterTimes[nextDisasterNum])
                {
                    lastDisaster = DisasterGenerator.Instance.GetRandomDisaster();
                    if (OnDisasterEncounter != null && lastDisaster != null) OnDisasterEncounter(playerId, lastDisaster, HasEnoughForSacrifice(lastDisaster));
                    nextDisasterNum++;
                    SoundManager.Instance.DisasterEncounter();
                    Debug.Log("Disaster: " + lastDisaster.text + " - Wine: " + lastDisaster.requiredWine + ", Food: " + lastDisaster.requiredFood + ", Fleece: " + lastDisaster.requiredFleece);
                    ChangeState(Enums.PlayerState.Disaster);
                    if (OnPauseMovement != null) OnPauseMovement(playerId);
                }
                // Controls
                switch (playerId)
                {
                case Enums.Players.Odysseus:
                    if (Input.GetKeyDown(KeyCode.Q)) MoveLane(true);
                    if (Input.GetKeyDown(KeyCode.A)) Pray();
                    if (Input.GetKeyUp(KeyCode.Z)) MoveLane(false);
                    break;
                case Enums.Players.Achilles:
                    if (Input.GetKeyDown(KeyCode.R)) MoveLane(true);
                    if (Input.GetKeyDown(KeyCode.F)) Pray();
                    if (Input.GetKeyUp(KeyCode.V)) MoveLane(false);
                    break;
                case Enums.Players.Theseus:
                    if (Input.GetKeyDown(KeyCode.U)) MoveLane(true);
                    if (Input.GetKeyDown(KeyCode.J)) Pray();
                    if (Input.GetKeyUp(KeyCode.M)) MoveLane(false);
                    break;
                }
                break;
            case Enums.PlayerState.Prayer:
                // Controls
                switch (playerId)
                {
                case Enums.Players.Odysseus:
                    if (Input.GetKeyDown(KeyCode.Q)) MakePrayerChoice(Enums.Players.Achilles);
                    if (Input.GetKeyDown(KeyCode.A)) MakePrayerChoice(Enums.Players.Odysseus);
                    if (Input.GetKeyUp(KeyCode.Z)) MakePrayerChoice(Enums.Players.Theseus);
                    break;
                case Enums.Players.Achilles:
                    if (Input.GetKeyDown(KeyCode.R)) MakePrayerChoice(Enums.Players.Odysseus);
                    if (Input.GetKeyDown(KeyCode.F)) MakePrayerChoice(Enums.Players.Achilles);
                    if (Input.GetKeyUp(KeyCode.V)) MakePrayerChoice(Enums.Players.Theseus);
                    break;
                case Enums.Players.Theseus:
                    if (Input.GetKeyDown(KeyCode.U)) MakePrayerChoice(Enums.Players.Odysseus);
                    if (Input.GetKeyDown(KeyCode.J)) MakePrayerChoice(Enums.Players.Theseus);
                    if (Input.GetKeyUp(KeyCode.M)) MakePrayerChoice(Enums.Players.Achilles);
                    break;
                }
                break;
            case Enums.PlayerState.Disaster:
                // Controls
                switch (playerId)
                {
                case Enums.Players.Odysseus:
                    if (Input.GetKeyDown(KeyCode.Q)) TryUseFavor();
                    if (Input.GetKeyDown(KeyCode.A)) PayResources(lastDisaster);
                    if (Input.GetKeyUp(KeyCode.Z)) TakeHit();
                    break;
                case Enums.Players.Achilles:
                    if (Input.GetKeyDown(KeyCode.R)) TryUseFavor();
                    if (Input.GetKeyDown(KeyCode.F)) PayResources(lastDisaster);
                    if (Input.GetKeyUp(KeyCode.V)) TakeHit();
                    break;
                case Enums.Players.Theseus:
                    if (Input.GetKeyDown(KeyCode.U)) TryUseFavor();
                    if (Input.GetKeyDown(KeyCode.J)) PayResources(lastDisaster);
                    if (Input.GetKeyUp(KeyCode.M)) TakeHit();
                    break;
                }
                break;
            }
        }
    }

    void ChangeState(Enums.PlayerState newState) {
        currentState = newState;
        if (OnPlayerStateChange != null) OnPlayerStateChange(playerId, newState);
    }
    
    #region MovementStateActions

    void MoveLane(bool up) {
        float valueToAdd;
        if (up) valueToAdd = GlobalNums.LANE_BETWEEN_DISTANCE;
        else valueToAdd = GlobalNums.LANE_BETWEEN_DISTANCE * -1;
        transform.localPosition = new Vector3(transform.localPosition.x,
            Mathf.Clamp(transform.localPosition.y + valueToAdd,
                GlobalNums.LANE_BETWEEN_DISTANCE * -1f,
                GlobalNums.LANE_BETWEEN_DISTANCE),
            transform.localPosition.z);
    }

    void Pray() {
        ChangeState(Enums.PlayerState.Prayer);
        if (OnPauseMovement != null) OnPauseMovement(playerId);
        SoundManager.Instance.PrayerStarted();
    }

    #endregion

    #region PrayerStateActions

    void MakePrayerChoice(Enums.Players targetPlayer) {
        ChangeState(Enums.PlayerState.Movement);
        if (targetPlayer != playerId) GameManager.Instance.AddFavor(targetPlayer);
        if (OnResumeMovement != null) OnResumeMovement(playerId);
        SoundManager.Instance.PrayerCompleted();
    }

    #endregion

    #region DisasterStateActions

    void TryUseFavor() {
        if (GameManager.Instance.UseFavor(playerId))
        {
            SoundManager.Instance.DisasterAverted();
        }
        else
        {
            SoundManager.Instance.DisasterHit();
            timeProgressed = Mathf.Clamp(timeProgressed - (GlobalNums.TOTAL_JOURNEY_TIME * 0.1f), 0, GlobalNums.TOTAL_JOURNEY_TIME);
        }
        ChangeState(Enums.PlayerState.Movement);
        if (OnResumeMovement != null) OnResumeMovement(playerId);
    }

    void PayResources(Disaster disaster) {
        if (HasEnoughForSacrifice(disaster))
        {
            resourceCounters[Enums.Resources.Wine] -= disaster.requiredWine;
            resourceCounters[Enums.Resources.Food] -= disaster.requiredFood;
            resourceCounters[Enums.Resources.Fleece] -= disaster.requiredFleece;
            SoundManager.Instance.DisasterAverted();
            ChangeState(Enums.PlayerState.Movement);
            if (OnResumeMovement != null) OnResumeMovement(playerId);
        }
        else
        {
            // Do nothing
        }
    }

    void TakeHit() {
        SoundManager.Instance.DisasterHit();
        timeProgressed = Mathf.Clamp(timeProgressed - (GlobalNums.TOTAL_JOURNEY_TIME * 0.05f), 0, GlobalNums.TOTAL_JOURNEY_TIME);
        // Resume
        if (OnResumeMovement != null) OnResumeMovement(playerId);
        ChangeState(Enums.PlayerState.Movement);
        if (OnResumeMovement != null) OnResumeMovement(playerId);
    }

    #endregion

    bool HasEnoughForSacrifice(Disaster disaster) {
        if (resourceCounters[Enums.Resources.Wine] >= disaster.requiredWine &&
            resourceCounters[Enums.Resources.Food] >= disaster.requiredFood &&
            resourceCounters[Enums.Resources.Fleece] >= disaster.requiredFleece)
        {
            return true;
        }
        else return false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Resource>() != null)
        {
            StartCoroutine(DisableCollisionForShortTime());
            Enums.Resources resourceCollected = collision.gameObject.GetComponent<Resource>().resourceType;
            resourceCounters[resourceCollected] += 1;
            //if (OnCollectedResource != null) OnCollectedResource(playerId, resourceCollected);
            Destroy(collision.gameObject);
            SoundManager.Instance.ResourceCollected();
        }
    }

    IEnumerator DisableCollisionForShortTime() {
        myCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        myCollider.enabled = true;
    }
}
