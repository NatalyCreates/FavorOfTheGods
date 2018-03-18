using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public Enums.Resources resourceType;

    public float speed;
    private float originalSpeed;
    
    internal Enums.Players playerWhoCanCollectThis;
    
    void Awake() {
        Player.OnPauseMovement += OnPause;
        Player.OnResumeMovement += OnResume;
    }

    void OnDestroy() {
        Player.OnPauseMovement -= OnPause;
        Player.OnResumeMovement -= OnResume;
    }

    void Update() 
    {
        MoveBack();	
	}

    public void OnPause(Enums.Players pausedPlayer) {
        if (pausedPlayer == playerWhoCanCollectThis)
        {
            originalSpeed = speed;
            speed = 0;
        }
    }

    public void OnResume(Enums.Players resumedPlayer) {
        if (resumedPlayer == playerWhoCanCollectThis)
        {
            speed = originalSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Remover")
        {
            Destroy(gameObject);
        }
	}

	void MoveBack()
    {
        //transform.position = new Vector2(transform.position.x, transform.position.y + speed);
        transform.Translate(-speed, 0, 0);
    }
}
