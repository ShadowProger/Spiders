using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public class RateUsWindow : Window
    {
        public GameManager gameManager;

        private ShareAndRate shareAndRate;
        private ScreenManager screenManager;

        private Animator anim;



        private void Awake()
        {
            screenManager = gameManager.screenManager;
            shareAndRate = gameManager.shareAndRate;
            anim = GetComponent<Animator>();
        }



        public override void Open()
        {
            base.Open();
            anim.SetTrigger("Open");
        }



        public override void Close()
        {
            base.Close();
            anim.SetTrigger("Close");
        }



        public void OnNotNowButtonClick()
        {
            Close();
        }



        public void OnOkButtonClick()
        {
            shareAndRate.RateUs();
            gameManager.isRated = true;
            Close();
        }
    }
}
