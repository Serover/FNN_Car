using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapScript : MonoBehaviour
{
    public GameMaster gm;
    public GameObject fadeScreen;
    public Text endText;
    public Text lapText;

    private bool gameEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        lapText.text = "Laps " + gm.playerLaps + "/" + gm.laps;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded)
        {
            if (gm._AILaps == gm.laps && !gameEnded)
            {
                endText.text = "You Lose";

                fadeScreen.SetActive(true);
                gameEnded = true;
            }
            else if (gm.playerLaps == gm.laps && !gameEnded)
            {
                endText.text = "You Win";

                fadeScreen.SetActive(true);
                gameEnded = true;
            }

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (gm.gamePlaying && !gameEnded)
        {
            if (collision.transform.tag == "AI")
            {
                gm._AILaps++;


            }
            else if (collision.transform.tag == "Player")
            {
                gm.playerLaps++;

                lapText.text = "Laps " + gm.playerLaps + "/" + gm.laps;

            }
        }
    }
}
