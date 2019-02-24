using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manybits
{
    public class LevelWindow : Window
    {
        public GameManager gameManager;

        private ScreenManager screenManager;
        private GameProcess gameProcess;
        private List<LevelIcon> levelIcons = new List<LevelIcon>();

        public GameObject levelButtonPref;
        public GameObject content;
        private GridLayoutGroup gridLayout;

        public RectTransform viewPort;

        private float contentHeight;
        private float viewportHeight;
        private float cellSize;
        private Animator anim;



        private void Awake()
        {
            screenManager = gameManager.screenManager;
            gameProcess = gameManager.gameProcess;

            anim = GetComponent<Animator>();
        }



        private void Start()
        {
            CreateLevelsList();

            SetContentPos();
        }



        private void CreateLevelsList()
        {
            gridLayout = content.GetComponent<GridLayoutGroup>();
            int rowsCount = (gameManager.levels.Count - 1) / gridLayout.constraintCount + 1;

            cellSize = gridLayout.cellSize.y;
            contentHeight = gridLayout.cellSize.y * rowsCount + gridLayout.spacing.y * (rowsCount - 1);
            viewportHeight = viewPort.rect.height;

            Debug.Log("height = " + contentHeight);
            Debug.Log("viewPort.height = " + viewPort.rect.height);

            // Create levels list
            for (int i = 0; i < gameManager.levels.Count; i++)
            {
                GameObject levelIconObj = Instantiate(levelButtonPref, content.transform, false);
                LevelIcon levelIcon = levelIconObj.GetComponent<LevelIcon>();

                Button button = levelIconObj.GetComponent<Button>();
                int levelNumber = i;
                button.onClick.AddListener(() => { this.OnLevelSelect(levelNumber); });

                levelIcon.SetProperty(gameManager.levels[i]);
                levelIcons.Add(levelIcon);
            }

            Debug.Log("Create level's list " + gameManager.levels.Count);

            (content.transform as RectTransform).offsetMin = new Vector2(0f, 0f);
            (content.transform as RectTransform).offsetMax = new Vector2(0f, contentHeight);
        }



        private void SetContentPos()
        {
            int lastopened = gameManager.lastOpenedLevel < gameManager.levels.Count ? gameManager.lastOpenedLevel : 0;
            int lastlevel = gameManager.lastPlayedLevel == -1 ? lastopened : gameManager.lastPlayedLevel;
            int rowNumber = lastlevel / gridLayout.constraintCount;
            float rowPosY = gridLayout.cellSize.y * rowNumber + gridLayout.spacing.y * rowNumber;

            float contentPosY = rowPosY - viewportHeight * 0.5f + cellSize * 0.5f;
            if (rowPosY < viewportHeight * 0.5f)
                contentPosY = 0;
            if (contentHeight - rowPosY < viewportHeight * 0.5f)
                contentPosY = contentHeight - viewportHeight;

            (content.transform as RectTransform).anchoredPosition = new Vector2(0f, contentPosY);
        }



        public override void Open()
        {
            base.Open();

            gameManager.SetCurrentWindow(OpenedWindow.OW_LEVEL);

            this.gameObject.SetActive(true);
            RefreshLevels();
        }



        public override void Close()
        {
            base.Close();
            //this.gameObject.SetActive(false);
        }



        public void OpenSlide()
        {
            Open();
            anim.SetTrigger("OpenSlide");
        }



        public void OpenScale()
        {
            Open();
            anim.SetTrigger("OpenScale");
        }



        public void CloseSlide()
        {
            Close();
            anim.SetTrigger("CloseSlide");
        }



        public void CloseScale()
        {
            Close();
            anim.SetTrigger("CloseScale");
        }



        private void RefreshLevels()
        {
            for (int i = 0; i < levelIcons.Count; i++)
            {
                levelIcons[i].SetProperty(gameManager.levels[i]);
            }
            SetContentPos();
        }



        public void OnCloseButtonClick()
        {
            if (gameManager.currentWindow == OpenedWindow.OW_LEVEL)
            {
                CloseSlide();
                screenManager.menuWindow.Open();
            }
        }



        public void OnLevelSelect(int levelNumber)
        {
            if (gameManager.currentWindow == OpenedWindow.OW_LEVEL)
            {
                gameManager.currentLevel = levelNumber;
                CloseScale();
                screenManager.OpenGameScreen();
                screenManager.gameWindow.Open();
                gameManager.gameProcess.StartLevel(gameManager.levels[gameManager.currentLevel]);

                gameManager.lastPlayedLevel = gameManager.currentLevel;
            }
        }
    }
}
