using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manybits
{
    public class GameWindow : Window
    {
        public GameManager gameManager;

        private ScreenManager screenManager;
        private GameProcess gameProcess;

        public Text txtTime;
        public Text txtMoves;
        public Text txtLevelNumber;

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

            gameManager.SetCurrentWindow(OpenedWindow.OW_GAME);

            this.gameObject.SetActive(true);
            InitWindow();

            gameManager.gameProcess.gameObject.SetActive(true);
            anim.SetTrigger("Open");
        }

        

        public override void Close()
        {
            base.Close();
            //this.gameObject.SetActive(false);

            gameManager.gameProcess.gameObject.SetActive(false);
            StopTimer();
            anim.SetTrigger("Close");
        }



        public void OnPauseButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_GAME)
            {
                gameProcess.GameMode = GameMode.GM_PAUSE;
                screenManager.pauseWindow.Open();
            }
        }



        public void OnMinusButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_GAME)
            {
                gameProcess.DecPointScale();
            }
        }



        public void OnPlusButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_GAME)
            {
                gameProcess.IncPointScale();
            }
        }



        public void InitWindow()
        {
            txtLevelNumber.text = "" + (gameManager.currentLevel + 1);
            txtTime.text = "00:00";
            txtMoves.text = "moves: 0";
            StopTimer();
            RestartTimer();
        }



        private IEnumerator StartTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                float time = gameProcess.gameTime;

                //Debug.Log(time);

                int sec = (int) time % 60;
                int min = (int) time / 60;

                txtTime.text = (min < 10 ? "0" + min : "" + min) + ":" + (sec < 10 ? "0" + sec : "" + sec);
            }
        }



        public void RestartTimer()
        {
            txtTime.text = "00:00";
            StartCoroutine("StartTimer");
        }



        public void StopTimer()
        {
            StopCoroutine("StartTimer");
        }



        public void SetMovesText(int moves)
        {
            txtMoves.text = "moves: " + moves;
        }
    }
}
