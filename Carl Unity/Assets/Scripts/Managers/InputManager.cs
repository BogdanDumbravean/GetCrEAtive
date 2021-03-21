using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UiManager))]
public class InputManager : MonoBehaviour
{
    private enum Button {
        None,
        Left,
        Right
    }

    private UiManager uiManager;
    [SerializeField] private MovementController playerController;
    private Button lastPressed;
    private float gameOverTimer;

    private void Start() {
        lastPressed = Button.None;
        uiManager = GetComponent<UiManager>();
        gameOverTimer = 1f;
    }

    private void Update()
    {
        if(GameManager.over) {
            if(gameOverTimer <= 0) {
                if(Input.anyKey)
                    uiManager.GoMenu();
            } else {
                gameOverTimer -= Time.deltaTime;
            }

        }
        PauseMenu();
        if(!GameManager.playerBegin || GameManager.over)
            return;

        GoForward();
        SwitchLanes();
        DetectCollision();
    }

    private void PauseMenu() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(UiManager.GameInSettings){
                return;
            }
            else if(UiManager.GameIsPaused){
                uiManager.Resume();
            }
            else
            {
                uiManager.Pause();
            }
        }
    }

    private void DetectCollision() {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(playerController.transform.position.x - .7f, playerController.transform.position.y),
            Vector2.right, 
            .2f, 
            playerController.carLayermask);

        if(hit.collider == null)
            return;
        playerController.Tumble();
    }

    private void GoForward() {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(lastPressed == Button.Left) {
                playerController.Tumble();
            } else {
                playerController.IncreaseSpeed();
            }
            lastPressed = Button.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if(lastPressed == Button.Right) {
                playerController.Tumble();
            } else {
                playerController.IncreaseSpeed();
            }
            lastPressed = Button.Right;
        }
    }

    private void SwitchLanes() {
        if(Input.GetKeyDown(KeyCode.W)) {
            playerController.MoveLaneUp();
        } else if(Input.GetKeyDown(KeyCode.S)) {
            playerController.MoveLaneDown();
        }
    }
}
