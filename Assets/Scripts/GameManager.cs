using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager> {

    public bool gameOver;
    List<Enums.Players> favorsFromGods;
    public static event System.Action<Enums.Players> OnWinner;

    public void Start() {
        gameOver = false;
        favorsFromGods = new List<Enums.Players>();
    }

    public bool UseFavor(Enums.Players player) {
        if (favorsFromGods.Contains(player))
        {
            // Remove all this player's favors, they always need to ask for more favors after an event
            favorsFromGods.RemoveAll(p => p == player);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddFavor(Enums.Players targetPlayer) {
        favorsFromGods.Add(targetPlayer);
    }

    public void WinGame(Enums.Players winner) {
        if (!gameOver)
        {
            gameOver = true;
            Debug.Log(winner.ToString() + " wins!");
            if (OnWinner != null) OnWinner(winner);
            StartCoroutine(ResetGame());
        }
        else Debug.Log("There's already a winner!");
    }

    IEnumerator ResetGame() {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(0);
    }

}
