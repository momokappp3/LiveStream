using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.BodyTracking;
using Assets.Scripts._System;

public class SaveDataAdmin : MonoBehaviour
{
    /*
    [SerializeField] SOGameLevel sOGameLevel;
    [System.Serializable]
    public class CSaveDataTotal
    {
        /// <summary>
        /// バージョン番号の詳細
        /// ※上位の数が変わったら下位はリセットする
        /// </summary>
        public enum EVersionCodes
        {
            Measure_ID,     ///ゲーム全体バージョン
            Games_Added,    ///ミニゲームがMeasure_ID更新初期から幾つ追加されたか
            Method_Added,   ///機能追加数
            BugFixed_Num,   ///バグ修正数
        }

        [System.Serializable]
        public class CKinectSetting
        {
            public string mName;
            public DepthMode mDepthMode;
            public Vector3 mRotate;
            public Vector3 mTranslate;
            public float mRoomScale;
            public float mHeight;
        }

        [System.Serializable]
        public class CGameSetting
        {
            [System.Serializable]
            public class CGameListSetting
            {
                public string Name; ///ゲームコード
                public enum ETimeLimit
                {
                    sec60,
                    sec90,
                    sec180,
                };

                [System.Serializable]
                public class CIsUsingKinect
                {
                    public string Name;   ///Kinectのシリアル番号
                    public bool mUsing;         ///このゲームで使うか？
                }

                public SOGameLevel.ETimeLimit mTimeLimit;
                public CIsUsingKinect[] mIsActiveKinects = new CIsUsingKinect[1];
                public CGameListSetting()
                {
                    mTimeLimit = SOGameLevel.ETimeLimit.sec180;
                    for (var num = 0; num < mIsActiveKinects.Length; num++)
                    {
                        mIsActiveKinects[num] = new CIsUsingKinect();
                        mIsActiveKinects[num].Name = "";
                        mIsActiveKinects[num].mUsing = true;
                    }
                }
                public override string ToString()
                {
                    return JsonUtility.ToJson(this, true);
                }
            }
            public bool mLowHeight = true;        ///低身長
            public bool mIsDownLevel = false;     ///インタラクティブなレベルダウン
                                                  ///ゲーム毎の設定
            public List<CGameListSetting> cGameListSetting = new List<CGameListSetting>();
            public override string ToString()
            {
                return JsonUtility.ToJson(this, true);
            }
        }

        [System.Serializable]
        public class CSettingScreen
        {
            public CntrSaveDataScreen.EDisplays[] displayLocation = new CntrSaveDataScreen.EDisplays[Enum.GetNames(typeof(CntrSaveDataScreen.EDropDown)).Length];
            public bool IsFullScreen = false;
        }

        ///ProjectSettingの値
        public string Version = "0.01";
        /// Kinectの設定
        public CKinectSetting[] cKinectSetting;// = new CKinectSetting[2];
        /// ゲームの設定
        public CGameSetting cGameSetting;
        ///マルチスクリーンの設定
        public CSettingScreen cSettingScreen;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    /// <summary>
    /// SaveData本体
    /// </summary>
    public CSaveDataTotal SaveDataTotal = new CSaveDataTotal();
    Dictionary<CSaveDataTotal.EVersionCodes, string> savedVersion;
    Dictionary<CSaveDataTotal.EVersionCodes, string> appliVersion;

    // Start is called before the first frame update
    void Start()
    {
#if false
        PlayerPrefs.DeleteAll();
#endif
        if (PlayerPrefs.HasKey("GamePlaySettings"))
        {///セーブデータにゲームのセッティング情報があった
            Debug.Log(" HasKey");
            ///セーブデータのロード
            SaveDataTotal = JsonUtility.FromJson<CSaveDataTotal>(PlayerPrefs.GetString("GamePlaySettings"));
            ///Version番号の解析
            CSaveDataTotal.EVersionCodes[] versionCodes = (CSaveDataTotal.EVersionCodes[])Enum.GetValues(typeof(CSaveDataTotal.EVersionCodes));
            //Rangeで0から要素数までのシーケンスを作成し、ToDictionaryでDictionaryを作る
            savedVersion = Enumerable.Range(0, versionCodes.Length).ToDictionary(i => versionCodes[i], i => SaveDataTotal.Version.Split('.')[i]);
            appliVersion = Enumerable.Range(0, versionCodes.Length).ToDictionary(i => versionCodes[i], i => Application.version.Split('.')[i]);
            if (SaveDataTotal.Version != "" &&
                savedVersion[CSaveDataTotal.EVersionCodes.Measure_ID] == appliVersion[CSaveDataTotal.EVersionCodes.Measure_ID] &&
                savedVersion[CSaveDataTotal.EVersionCodes.Games_Added] == appliVersion[CSaveDataTotal.EVersionCodes.Games_Added] &&
                savedVersion[CSaveDataTotal.EVersionCodes.Method_Added] == appliVersion[CSaveDataTotal.EVersionCodes.Method_Added] &&
                SaveDataTotal.cGameSetting.cGameListSetting.Count != 0)
            {///セーブデータとバージョンが一致
             ///ロードしたデータで管理画面をセット
                this.GetComponent<CntrSaveDataKinect>().UpdateFromLoadedData();
                this.GetComponent<CntrSaveDataGames>().UpdateFromLoadedData();
                this.GetComponent<CntrSaveDataScreen>().UpdateFromLoadedData();
                Debug.Log("SaveData Found");
            }
            else
            {///バージョンが上がったらすべてリセット
             ///一旦セーブデータをクリア
             ///CGameSettingの初期化（ちょっとダメ）
                resetAdminData();
                Debug.Log("Game Version Up to date.");
            }
        }
        else
        {///データがないので初期値でセット
            resetAdminData();
            Debug.Log("Made New SaveData");
        }
        Debug.Log(SaveDataTotal.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 管理画面情報のセーブ
    /// </summary>
    public void SaveAdminData()
    {
        ///CKinectSettingの初期化

        ///CSettingScreenの初期化

        ///アプリケーションバージョンの更新
        SaveDataTotal.Version = Application.version;
        ///JSON化してセーブ
        var savedata = JsonUtility.ToJson(SaveDataTotal);
        PlayerPrefs.SetString("GamePlaySettings", savedata);
        PlayerPrefs.Save();
        ///書き込んだものでキャッシュを再設定
        Debug.Log("SaveData Updated");
        SaveDataTotal = JsonUtility.FromJson<CSaveDataTotal>(PlayerPrefs.GetString("GamePlaySettings"));
        Debug.Log(SaveDataTotal.ToString());
    }

    private void resetAdminData()
    {
        ///CGameSettingの初期化（ちょっとダメ）
        SaveDataTotal.cGameSetting.cGameListSetting.Clear();
        for (int cnt = 0
        ; cnt < Enum.GetValues(typeof(SOGameLevel.EGameLists)).Length
        ; cnt++)
        {
            SaveDataTotal.cGameSetting.cGameListSetting.Add(new CSaveDataTotal.CGameSetting.CGameListSetting());
            SaveDataTotal.cGameSetting.cGameListSetting[cnt].Name = ((SOGameLevel.EGameLists)cnt).ToString();
            SaveDataTotal.cGameSetting.cGameListSetting[cnt].mTimeLimit = SOGameLevel.ETimeLimit.sec180;
        }
        if (!sOGameLevel.IsMockup)
        {///モックアップ版の場合はPlayerPrefsを更新しない
         ///一旦セーブデータをクリア
            PlayerPrefs.DeleteAll();
            SaveAdminData();
        }
    }
    */
}
