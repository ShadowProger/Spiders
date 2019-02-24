using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public class MenuWindow : Window
    {
        public GameManager gameManager;

        private ScreenManager screenManager;
        private GameProcess gameProcess;
        private ShareAndRate shareAndRate;
        private GpgsController gpgsController;
        private Animator anim;



        private void Awake()
        {
            screenManager = gameManager.screenManager;
            gameProcess = gameManager.gameProcess;
            shareAndRate = gameManager.shareAndRate;
            gpgsController = gameManager.gpgsController;
            anim = GetComponent<Animator>();
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameManager.currentWindow == OpenedWindow.OW_MENU)
                {
                    Application.Quit();
                }
            }
        }



        public override void Open()
        {
            base.Open();

            gameManager.SetCurrentWindow(OpenedWindow.OW_MENU);

            this.gameObject.SetActive(true);
            anim.SetTrigger("Open");
        }



        public override void Close()
        {
            base.Close();
            anim.SetTrigger("Close");
            //this.gameObject.SetActive(false);
        }



        public void OnPlayButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_MENU)
            {
                Close();
                screenManager.levelWindow.OpenSlide();
            }
        }



        public void OnShareButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_MENU)
            {
                shareAndRate.OnAndroidTextSharingClick();
            }
        }



        public void OnLeaderButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_MENU)
            {
                gpgsController.OpenLeaderboard();
            }
        }



        public void OnAchieveButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_MENU)
            {
                gpgsController.OpenAchievements();
            }
        }
    }
}
