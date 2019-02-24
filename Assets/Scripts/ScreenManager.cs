using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public class ScreenManager : MonoBehaviour
    {
        public MenuWindow menuWindow;
        public LevelWindow levelWindow;
        public GameWindow gameWindow;
        public PauseWindow pauseWindow;
        public WinWindow winWindow;
        public RateUsWindow rateUsWindow;

        //public RateUsWindow rateUsWindow;

        public GameObject gameScreen;



        public void Start()
        {

        }

        public void OpenGameScreen()
        {
            gameScreen.SetActive(true);
        }

        public void CloseGameScreen()
        {
            gameScreen.SetActive(false);
        }
    }
}
