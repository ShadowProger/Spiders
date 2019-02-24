using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public class PauseWindow : Window
    {
        public GameManager gameManager;

        private ScreenManager screenManager;
        private GameProcess gameProcess;

        private Animator anim;



        private void Awake()
        {
            screenManager = gameManager.screenManager;
            gameProcess = gameManager.gameProcess;
            anim = GetComponent<Animator>();
        }



        public override void Open()
        {
            base.Open();

            gameManager.SetCurrentWindow(OpenedWindow.OW_PAUSE);

            this.gameObject.SetActive(true);
            anim.SetTrigger("Open");
        }



        public override void Close()
        {
            base.Close();
            anim.SetTrigger("Close");
        }



        public void OnMenuButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_PAUSE)
            {
                Close();
                screenManager.CloseGameScreen();
                screenManager.gameWindow.Close();
                screenManager.levelWindow.OpenScale();
            }
        }



        public void OnPlayButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_PAUSE)
            {
                Close();
                gameProcess.GameMode = GameMode.GM_GAME;
                gameManager.SetCurrentWindow(OpenedWindow.OW_GAME);
            }
        }



        public void OnRestartButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_PAUSE)
            {
                Close();
                gameProcess.StartLevel(gameManager.levels[gameManager.currentLevel]);
                screenManager.gameWindow.InitWindow();
                gameManager.SetCurrentWindow(OpenedWindow.OW_GAME);
            }
        }
    }
}
