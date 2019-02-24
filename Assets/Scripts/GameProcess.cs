using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace Manybits
{
    public enum GameMode {GM_NONE, GM_GAME, GM_PAUSE, GM_GAMEOVER}

    public class GameProcess : MonoBehaviour
    {
        public GameManager gameManager;
        private CameraManager cameraManager;
        private ScreenManager screenManager;
        private GpgsController gpgsController;
        private AdsController adsController;

        public GameObject pointPref;
        public GameObject linePref;

        public Transform pointsHolder;
        public Transform linesHolder;

        private List<Point> points = new List<Point>();
        private List<Line> lines = new List<Line>();

        public Rect moveArea;
        private Rect fieldRect;
        float fieldWidth;

        private Vector2 pointSize;
        public float pointScale = 0.2f;
        private const float MIN_POINT_SCALE = 0.1f;
        private const float MAX_POINT_SCALE = 0.4f;

        private GameMode gameMode;
        public GameMode GameMode
        {
            get
            {
                return gameMode;
            }
            set
            {
                gameMode = value;
            }
        }

        public float gameTime;
        public int moves;

        public int touchesCount;
        public bool isLineCross;

        public int starsCount = 0;



        void Awake()
        {
            Debug.Log("[GameProcess] Awake");
            cameraManager = gameManager.cameraManager;
            screenManager = gameManager.screenManager;
            gpgsController = gameManager.gpgsController;
            adsController = gameManager.adsController;

            int space = 0;
            int top = 200;
            int bottom = 110;
            int left = 0;
            int right = 0;

            float screenTop = cameraManager.ScreenHeight * 0.5f;
            float screenLeft = -cameraManager.ScreenWidth * 0.5f;

            float areaWidth = cameraManager.ScreenWidth - (left + right + space * 2);
            float areaHeight = cameraManager.ScreenHeight - (top + bottom + space * 2);
            Vector2 areaPos = new Vector2(screenLeft + left + space, -screenTop + bottom + space);
            moveArea = new Rect(areaPos, new Vector2(areaWidth, areaHeight));

            SpriteRenderer pointSprite = pointPref.GetComponent<SpriteRenderer>();
            pointSize = pointSprite.sprite.rect.size;

            if (pointScale < MIN_POINT_SCALE)
            {
                pointScale = pointPref.transform.localScale.x;
            }
            Vector2 pointScaledSize = pointSize * pointScale;

            fieldWidth = Mathf.Min(areaWidth - pointScaledSize.x, areaHeight - pointScaledSize.y);
            fieldRect = new Rect(moveArea.center.x - fieldWidth * 0.5f, moveArea.center.y - fieldWidth * 0.5f, fieldWidth, fieldWidth);
        }



        public void StartLevel(Level level)
        {
            Debug.Log("[GameProcess] StartLevel");
            for (int i = 0; i < points.Count; i++)
                Destroy(points[i].gameObject);
            for (int i = 0; i < lines.Count; i++)
                Destroy(lines[i].gameObject);

            points.Clear();
            for (int i = 0; i < level.points.Count; i++)
            {
                Vector2 newPos = new Vector2(level.points[i].x * fieldWidth * 0.5f, level.points[i].y * fieldWidth * 0.5f);

                GameObject pointObj = Instantiate(pointPref, pointsHolder, false);
                Point point = pointObj.GetComponent<Point>();
                point.position = newPos;

                pointObj.GetComponent<ManualDrag>().gameManager = gameManager;

                pointObj.transform.position = newPos;
                pointObj.transform.localScale = new Vector3(pointScale, pointScale, 1f);
                points.Add(point);
            }

            lines.Clear();
            for (int i = 0; i < level.lines.Count; i++)
            {
                GameObject lineObj = Instantiate(linePref, linesHolder, false);
                Line line = lineObj.GetComponent<Line>();
                line.point1 = points[level.lines[i].point1Index];
                line.point2 = points[level.lines[i].point2Index];
                lines.Add(line);
            }

            GameMode = GameMode.GM_GAME;
            gameTime = 0;
            moves = 0;
        }



        void Update()
        {
            if (gameMode == GameMode.GM_GAME)
            {
                gameTime += Time.deltaTime;

                DrawRect(fieldRect, Color.yellow);

                for (int i = 0; i < lines.Count; i++)
                    lines[i].SetCross(false);

                for (int i = 0; i < lines.Count - 1; i++)
                {
                    for (int j = i + 1; j < lines.Count; j++)
                    {
                        bool isCross = IsLineCross(lines[i], lines[j]);
                        if (isCross)
                        {
                            lines[i].SetCross(isCross);
                            lines[j].SetCross(isCross);
                        }
                    }
                }

                isLineCross = false;

                for (int i = 0; i < points.Count - 1; i++)
                {
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        if (Mathf.Abs(points[i].position.x - points[j].position.x) < 0.1f &&
                            Mathf.Abs(points[i].position.y - points[j].position.y) < 0.1f)
                        {
                            isLineCross = true;
                            break;
                        }
                    }
                    if (isLineCross)
                    {
                        break;
                    }
                }

                for (int i = 0; i < lines.Count; i++)
                    if (lines[i].isCross)
                    {
                        isLineCross = true;
                        break;
                    }
                    

                if (touchesCount == 0 && isLineCross == false)
                {
                    Debug.Log("Win");
                    GameMode = GameMode.GM_GAMEOVER;

                    Level level = gameManager.levels[gameManager.currentLevel];

                    int maxTouchCount = 0;
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].touchCount > maxTouchCount)
                            maxTouchCount = points[i].touchCount;
                    }

                    starsCount = 1;
                    if (maxTouchCount < 3)
                        starsCount = 3;
                    if (maxTouchCount == 3)
                        starsCount = 2;
                    if (maxTouchCount > 3)
                        starsCount = 1;

                    if (level.access == LevelAccess.LA_UNLOCKED)
                    {
                        level.time = gameTime;
                        level.moves = moves;
                        level.stars = starsCount;
                        level.access = LevelAccess.LA_COMPLITED;
                    }
                    else
                    {
                        if (starsCount > level.stars)
                        {
                            level.stars = starsCount;
                            level.time = gameTime;
                            level.moves = moves;
                        }
                    }

                    int nextLevel = gameManager.currentLevel + 1;
                    if (nextLevel < gameManager.levels.Count)
                    {
                        if (gameManager.levels[nextLevel].access == LevelAccess.LA_LOCKED)
                        {
                            gameManager.levels[nextLevel].access = LevelAccess.LA_UNLOCKED;
                            gameManager.lastOpenedLevel = nextLevel;
                        }
                        gameManager.lastPlayedLevel = nextLevel;
                    }

                    // Leaderboard and achievements
                    int curLevel = gameManager.currentLevel + 1;
                    gpgsController.SetHighscore(curLevel);
                    Debug.Log("SetHighscore: " + curLevel);

                    if (curLevel == 10)
                    {
                        gpgsController.UnlockAchievement(GPGSIds.achievement_level_10);
                        Debug.Log("Unlock: achievement_level_10");
                    }
                    if (curLevel == 30)
                    {
                        gpgsController.UnlockAchievement(GPGSIds.achievement_level_30);
                        Debug.Log("Unlock: achievement_level_30");
                    }
                    if (curLevel == 60)
                    {
                        gpgsController.UnlockAchievement(GPGSIds.achievement_level_60);
                        Debug.Log("Unlock: achievement_level_60");
                    }
                    if (curLevel == 90)
                    {
                        gpgsController.UnlockAchievement(GPGSIds.achievement_level_90);
                        Debug.Log("Unlock: achievement_level_90");
                    }
                    if (curLevel == 120)
                    {
                        gpgsController.UnlockAchievement(GPGSIds.achievement_level_120);
                        Debug.Log("Unlock: achievement_level_120");
                    }

                    screenManager.winWindow.Open();

                    //Ads
                    if (gameManager.advertTime <= 0)
                    {
                        gameManager.advertTime = GameManager.MAX_ADVERT_TIME;
                        adsController.ShowInterstitial();
                    }

                    // Rate
                    if (!gameManager.isRated)
                    {
                        if (curLevel == 10 && !gameManager.isFirstRateShown)
                        {
                            screenManager.rateUsWindow.Open();
                            gameManager.isFirstRateShown = true;
                        }
                        if (curLevel == 30 && !gameManager.isSecondRateShown)
                        {
                            screenManager.rateUsWindow.Open();
                            gameManager.isSecondRateShown = true;
                        }
                        if (curLevel == 60 && !gameManager.isThirdRateShown)
                        {
                            screenManager.rateUsWindow.Open();
                            gameManager.isThirdRateShown = true;
                        }
                    }

                    gameManager.SaveProgress();
                }
            }
        }



        public void AddMove()
        {
            moves++;
            screenManager.gameWindow.SetMovesText(moves);
        }



        public void IncPointScale()
        {
            pointScale += 0.05f;
            if (pointScale > MAX_POINT_SCALE)
                pointScale = MAX_POINT_SCALE;
            UpdatePointScale();
        }



        public void DecPointScale()
        {
            pointScale -= 0.05f;
            if (pointScale < MIN_POINT_SCALE)
                pointScale = MIN_POINT_SCALE;
            UpdatePointScale();
        }



        public void UpdatePointScale()
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].gameObject.transform.localScale = new Vector3(pointScale, pointScale, 1f);
            }
        }



        public void DrawRect(Rect r, Color c)
        {
            Debug.DrawLine(new Vector2(r.xMin, r.yMin), new Vector2(r.xMax, r.yMin), c);
            Debug.DrawLine(new Vector2(r.xMax, r.yMin), new Vector2(r.xMax, r.yMax), c);
            Debug.DrawLine(new Vector2(r.xMax, r.yMax), new Vector2(r.xMin, r.yMax), c);
            Debug.DrawLine(new Vector2(r.xMin, r.yMax), new Vector2(r.xMin, r.yMin), c);
        }



        private float VecMult(Vector2 a, Vector2 b)
        {
            return a.x * b.y - b.x * a.y;
        }



        private bool IsLineCross(Line line1, Line line2)
        {
            Vector2 v34 = line2.point2.position - line2.point1.position;
            Vector2 v31 = line1.point1.position - line2.point1.position;
            Vector2 v32 = line1.point2.position - line2.point1.position;
            Vector2 v21 = line1.point2.position - line1.point1.position;
            Vector2 v23 = line2.point1.position - line1.point1.position;
            Vector2 v24 = line2.point2.position - line1.point1.position;

            float v1 = VecMult(v34, v31);
            float v2 = VecMult(v34, v32);
            float v3 = VecMult(v21, v23);
            float v4 = VecMult(v21, v24);

            if ((v1 * v2 < 0) && (v3 * v4 < 0))
                return true;
            else
                return false;
        }
    }
}
