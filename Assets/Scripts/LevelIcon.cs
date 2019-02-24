using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manybits
{
    public class LevelIcon : MonoBehaviour
    {
        public Text txtLevelNumber;

        public GameObject goLocked;
        public GameObject goUnlocked;

        public Image imgStars;
        private Button button;

        public Sprite[] stars;

        // Use this for initialization
        void Awake()
        {
            button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetProperty(Level level)
        {
            txtLevelNumber.text = "" + level.number;
            switch (level.access)
            {
                case LevelAccess.LA_COMPLITED:
                    goUnlocked.SetActive(true);
                    goLocked.SetActive(false);
                    imgStars.sprite = stars[level.stars];
                    button.interactable = true;
                    break;
                case LevelAccess.LA_UNLOCKED:
                    goUnlocked.SetActive(true);
                    goLocked.SetActive(false);
                    imgStars.sprite = stars[0];
                    button.interactable = true;
                    break;
                case LevelAccess.LA_LOCKED:
                    goUnlocked.SetActive(false);
                    goLocked.SetActive(true);
                    button.interactable = false;
                    break;
            }
        }
    }
}
