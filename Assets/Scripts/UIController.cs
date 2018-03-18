using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public PlayerUIInfo[] playerUIInfos;
    public Dictionary<Enums.Players, PlayerUIInfo> playerUIs;

    public DisasterUIInfo[] disasterUIInfos;

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
        if (targetState != Enums.PlayerState.Disaster)
        {
            DisasterUIInfo info = disasterUIInfos.Where(d => d.playerName == player).ToList()[0];
            info.disasterUI.SetActive(false);
        }
    }

    void OnDisasterEncounter(Enums.Players player, Disaster disaster, bool hasEnoughForSacrifice) {
        DisasterUIInfo info = disasterUIInfos.Where(d => d.playerName == player).ToList()[0];
        info.reqWineText.text = disaster.requiredWine.ToString();
        info.reqFoodText.text = disaster.requiredFood.ToString();
        info.reqFleeceText.text = disaster.requiredFleece.ToString();
        info.disasterText.text = "Disaster Strikes:\n" + disaster.text + "\nRequired Resources ->";
        if (hasEnoughForSacrifice) playerUIs[player].panels[Enums.PlayerState.Disaster].GetComponentsInChildren<Text>()[1].color = Color.white;
        else playerUIs[player].panels[Enums.PlayerState.Disaster].GetComponentsInChildren<Text>()[1].color = Color.grey;
        info.disasterUI.SetActive(true);
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
        winnerText.text = winner.ToString() + "\nWins!";
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

[System.Serializable]
public class DisasterUIInfo {
    public Enums.Players playerName;

    public GameObject disasterUI;
    public Text disasterText;
    public Text reqWineText;
    public Text reqFoodText;
    public Text reqFleeceText;
}
