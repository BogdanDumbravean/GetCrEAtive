using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool begin, playerBegin, over;
    public float secondsBeforeStart, secondsForPlayerStart;
    public Transform player;
    public GameObject gameOverCanvas;

    void Start() {
        StartCoroutine(StartRace());
        begin = false;
        playerBegin = false;
        over = false;
    }

    void Update() {
        if (player.transform.position.x >= 205) {
            over = true;
            gameOverCanvas.SetActive(true);
        }
    }

    IEnumerator StartRace() {
        yield return new WaitForSeconds(secondsBeforeStart);
        begin = true;
        yield return new WaitForSeconds(secondsForPlayerStart);
        playerBegin = true;
    }
}
