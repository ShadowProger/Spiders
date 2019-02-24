using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manybits
{
    public class WinWindow : Window
    {
        public GameManager gameManager;

        private ScreenManager screenManager;
        private GameProcess gameProcess;

        public Text txtTime;
        public Text txtMoves;

        public Image imgStars;
        public Sprite[] stars;

        public GameObject btnMenu;
        public GameObject btnRestart;
        public GameObject btnNext;

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

            gameManager.SetCurrentWindow(OpenedWindow.OW_WIN);

            this.gameObject.SetActive(true);

            imgStars.sprite = stars[gameProcess.starsCount];

            float time = gameProcess.gameTime;

            int sec = (int)time % 60;
            int min = (int)time / 60;
            int mSec = (int)((time - Mathf.FloorToInt(time)) * 100);

            txtTime.text = (min < 10 ? "0" + min : "" + min) + ":" + 
                (sec < 10 ? "0" + sec : "" + sec) + ":" + (mSec < 10 ? "0" + mSec : "" + mSec);

            txtMoves.text = "" + gameProcess.moves;

            if (gameManager.currentLevel == gameManager.levels.Count - 1)
            {
                btnNext.SetActive(false);
                (btnMenu.transform as RectTransform).anchoredPosition = new Vector2(-101.5f, 138);
                (btnRestart.transform as RectTransform).anchoredPosition = new Vector2(77.5f, 138);
            }
            else
            {
                btnNext.SetActive(true);
                (btnMenu.transform as RectTransform).anchoredPosition = new Vector2(-191f, 138);
                (btnRestart.transform as RectTransform).anchoredPosition = new Vector2(-12f, 138);
            }
            

            anim.SetTrigger("Open");
        }



        public override void Close()
        {
            base.Close();
            //this.gameObject.SetActive(false);
            anim.SetTrigger("Close");
        }



        public void OnMenuButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_WIN)
            {
                Close();
                screenManager.CloseGameScreen();
                screenManager.gameWindow.Close();
                screenManager.levelWindow.OpenScale();
            }
        }



        public void OnRestartButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_WIN)
            {
                Close();
                gameProcess.StartLevel(gameManager.levels[gameManager.currentLevel]);
                screenManager.gameWindow.InitWindow();
                gameManager.SetCurrentWindow(OpenedWindow.OW_GAME);
            }
        }



        public void OnNextButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_WIN)
            {
                gameManager.currentLevel++;
                Close();
                gameProcess.StartLevel(gameManager.levels[gameManager.currentLevel]);
                screenManager.gameWindow.InitWindow();
                gameManager.SetCurrentWindow(OpenedWindow.OW_GAME);
            }
        }
    }
}
