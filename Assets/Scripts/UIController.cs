using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public PlayerUIInfo[] playerUIInfos;
    public Dictionary<Enums.Players, PlayerUIInfo> playerUIs;

    public GameObject winnerPanel;
    public Text winnerText;

    private void Awake() {
        Player.OnPlayerStateChange += OnPlayerStateChange;
        Player.OnDisasterEncounter += OnDisasterEncounter;
        GameManager.OnWinner += OnWinner;
    }

    private void OnDestroy() {
        Player.OnPlayerStateChange -= OnPlayerStateChange;
        Player.OnDisasterEncounter -= OnDisasterEncounter;
        GameManager.OnWinner -= OnWinner;
    }

    void Start () {
        playerUIs = new Dictionary<Enums.Players, PlayerUIInfo>
        {
            { Enums.Players.Odysseus, playerUIInfos[0] },
            { Enums.Players.Achilles, playerUIInfos[1] },
            { Enums.Players.Theseus, playerUIInfos[2] }
        };
        foreach (PlayerUIInfo info in playerUIInfos)
        {
            info.panels = new Dictionary<Enums.PlayerState, GameObject>
            {
                { Enums.PlayerState.Movement, info.ControlPanel },
                { Enums.PlayerState.Prayer, info.PrayPanel },
                { Enums.PlayerState.Disaster, info.DisasterPanel }
            };
        }

        OnPlayerStateChange(Enums.Players.Odysseus, Enums.PlayerState.Movement);
        OnPlayerStateChange(Enums.Players.Achilles, Enums.PlayerState.Movement);
        OnPlayerStateChange(Enums.Players.Theseus, Enums.PlayerState.Movement);
    }

    void OnPlayerStateChange(Enums.Players player, Enums.PlayerState targetState) {
        
        foreach (Enums.PlayerState state in playerUIs[player].panels.Keys)
        {
            playerUIs[player].panels[state].SetActive(false);
        }
        playerUIs[player].panels[targetState].SetActive(true);
    }

    void OnDisasterEncounter(Enums.Players player, Disaster disaster, bool hasEnoughForSacrifice) {
        // Update Disaster UI with the disaster flavor text and the resource cost
        // Grey out the Sacrifice option (but still show it) if player doesn't have enough
    }

    void Update() {
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        DisplayProgress(Enums.Players.Odysseus);
        DisplayProgress(Enums.Players.Achilles);
        DisplayProgress(Enums.Players.Theseus);

        DisplayResource(Enums.Players.Odysseus);
        DisplayResource(Enums.Players.Achilles);
        DisplayResource(Enums.Players.Theseus);
    }

    void DisplayResource(Enums.Players player)
    {
        playerUIs[player].wineText.text = playerUIs[player].player.resourceCounters[Enums.Resources.Wine].ToString();
        playerUIs[player].foodText.text = playerUIs[player].player.resourceCounters[Enums.Resources.Food].ToString();
        playerUIs[player].fleeceText.text = playerUIs[player].player.resourceCounters[Enums.Resources.Fleece].ToString();
    }

    void DisplayProgress(Enums.Players player)
    {
        playerUIs[player].progressImage.fillAmount = playerUIs[player].player.progress;
    }

    void OnWinner(Enums.Players winner) {
        winnerText.text = winner.ToString() + " Wins!";
        winnerPanel.SetActive(true);
    }
}

[System.Serializable]
public class PlayerUIInfo
{
    public Enums.Players playerName;
    public Player player;
    public Text wineText;
    public Text foodText;
    public Text fleeceText;
    public Image progressImage;

    public Dictionary<Enums.PlayerState, GameObject> panels;

    public GameObject ControlPanel;
    public GameObject PrayPanel;
    public GameObject DisasterPanel;
}
