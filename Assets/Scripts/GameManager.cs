using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

namespace Manybits
{
    public enum OpenedWindow { OW_MENU, OW_GAME, OW_LEVEL, OW_PAUSE, OW_WIN, OW_RATEUS }

    public class GameManager : MonoBehaviour
    {
        public CameraManager cameraManager;
        public ScreenManager screenManager;
        public GameProcess gameProcess;
        public ShareAndRate shareAndRate;
        public GpgsController gpgsController;
        public AdsController adsController;
        public GameObject scene;
        public GameObject gui;
        public Window startWindow;

        public List<Level> levels = new List<Level>();

        public int lastOpenedLevel = 0;
        public int lastPlayedLevel = -1;
        public int currentLevel;

        public bool isRated = false;
        public bool isFirstRateShown = false;
        public bool isSecondRateShown = false;
        public bool isThirdRateShown = false;

        public string path;
        private string fileNameProgress = "GameProgress.bin";

        public OpenedWindow currentWindow;

        public const float MAX_ADVERT_TIME = 120f;
        public float advertTime;



        void Awake()
        {
            cameraManager.gameObject.SetActive(true);
            screenManager.gameObject.SetActive(true);
            shareAndRate.gameObject.SetActive(true);
            adsController.gameObject.SetActive(true);
            gpgsController.gameObject.SetActive(true);

#if UNITY_ANDROID
            path = Application.persistentDataPath;
#endif

#if UNITY_EDITOR
            path = Application.dataPath;
#endif
        }



        void Start()
        {
            LoadLevels();
            LoadProgress();

            if (lastOpenedLevel < levels.Count)
            {
                levels[lastOpenedLevel].access = LevelAccess.LA_UNLOCKED;
            }

            Debug.Log("LastOpened = " + lastOpenedLevel);
            Debug.Log("isRated = " + isRated);
            Debug.Log("isFirstRateShown = " + isFirstRateShown);
            Debug.Log("isSecondRateShown = " + isSecondRateShown);
            Debug.Log("isThirdRateShown = " + isThirdRateShown);

            scene.SetActive(true);
            gui.SetActive(true);

            advertTime = 60f;
        }



        private void Update()
        {
            advertTime -= Time.deltaTime;
        }



        private void LoadLevels()
        {
            Debug.Log("[GameManager] LoadLevels");
            XmlDocument xml = new XmlDocument();

            xml.LoadXml(Resources.Load("Levels").ToString());

            XmlNodeList dataList = xml.GetElementsByTagName("level");

            int levelsCount = 0;
            foreach (XmlNode item in dataList)
            {
                Level level = new Level();
                level.Load(item);
                levels.Add(level);
                levelsCount++;
                level.number = levelsCount;
                level.access = LevelAccess.LA_LOCKED;
            }
        }



        private bool LoadProgress()
        {
            Debug.Log("[GameManager] LoadProgress");

            string path = Path.Combine(this.path, fileNameProgress);

            if (File.Exists(path))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    try
                    {
                        bool loadedIsRated = reader.ReadBoolean();
                        bool loadedIsFirstRateShown = reader.ReadBoolean();
                        bool loadedIsSecondRateShown = reader.ReadBoolean();
                        bool loadedIsThirdRateShown = reader.ReadBoolean();

                        int loadedLastOpenedLevel = reader.ReadInt32();
                        float loadedPointScale = reader.ReadSingle();
                        int loadedLevelsCount = reader.ReadInt32();
                        int levelsCount = Mathf.Min(loadedLevelsCount, levels.Count);

                        List<Level> loadedLevels = new List<Level>();
                        for (int i = 0; i < levelsCount; i++)
                        {
                            Level level = new Level();
                            level.moves = reader.ReadInt32();
                            level.time = reader.ReadSingle();
                            level.stars = reader.ReadInt32();
                            level.access = (LevelAccess)reader.ReadInt32();
                            loadedLevels.Add(level);
                        }

                        Debug.Log("Load is successful");

                        isRated = loadedIsRated;
                        isFirstRateShown = loadedIsFirstRateShown;
                        isSecondRateShown = loadedIsSecondRateShown;
                        isThirdRateShown = loadedIsThirdRateShown;

                        lastOpenedLevel = loadedLastOpenedLevel;
                        gameProcess.pointScale = loadedPointScale;

                        for (int i = 0; i < levelsCount; i++)
                        {
                            levels[i].moves = loadedLevels[i].moves;
                            levels[i].time = loadedLevels[i].time;
                            levels[i].stars = loadedLevels[i].stars;
                            levels[i].access = loadedLevels[i].access;
                        }
                        loadedLevels.Clear();
                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        public void SaveProgress()
        {
            Debug.Log("[GameManager] SaveProgress");
            string path = Path.Combine(this.path, fileNameProgress);

            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                writer.Write(isRated);
                writer.Write(isFirstRateShown);
                writer.Write(isSecondRateShown);
                writer.Write(isThirdRateShown);
                writer.Write(lastOpenedLevel);
                writer.Write(gameProcess.pointScale);
                writer.Write(levels.Count);
                for (int i = 0; i < levels.Count; i++)
                {
                    writer.Write(levels[i].moves);
                    writer.Write(levels[i].time);
                    writer.Write(levels[i].stars);
                    writer.Write((int)levels[i].access);
                }
            }
        }



        public void SetCurrentWindow(OpenedWindow newWindow)
        {
            currentWindow = newWindow;
        }
    }
}
