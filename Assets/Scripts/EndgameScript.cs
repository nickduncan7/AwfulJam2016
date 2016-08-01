using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndgameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var gameManager = GameObjects.GameManager;
        var title = transform.FindChild("Title").GetComponent<Text>();
        var message = transform.FindChild("Message").GetComponent<Text>();

        if (gameManager.wonGame.Value)
        {
            title.text = "CONGRATULATIONS!";
            message.text = string.Format("You won the game by saving {0} grandpas!", gameManager.grandpasSaved);
        }
        else
        {
            title.text = "OH, NO!";
            message.text = "You couldn't save any grandpas. Better luck next time. :(";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
