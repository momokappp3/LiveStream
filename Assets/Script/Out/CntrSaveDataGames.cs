using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.Sensor;


public class CntrSaveDataGames : MonoBehaviour
{
    /// <summary>
    /// scriptableObjectへ移行すること
    /// </summary>
    //public float[] TimeLimitList = { 60f, 90f, 180f };

    /*
    [SerializeField] SOGameLevel sOGameLevel;
    [System.Serializable]
    public class CSettingGamesCommon
    {
        [System.Serializable]
        public class CSettingGame
        {
            public string Name;
            //public TextMeshProUGUI Title;
            public Toggle[] Kinect = new Toggle[2];
            public Toggle[] Timelimit = new Toggle[3];
        }
        public CSettingGame[] cSettingGames = new CSettingGame[Enum.GetValues(typeof(SOGameLevel.EGameLists)).Length];
        public Toggle SetHighLevel;
        public Toggle LevelDown;
    }
    public CSettingGamesCommon cSettingGamesCommon;


    // Start is called before the first frame update
    void Start()
    {
        //cSettingGamesCommon = new CSettingGamesCommon();
        /*
        {
            SaveDataAdmin.CSaveDataTotal.EGameLists gameID = SaveDataAdmin.CSaveDataTotal.EGameLists.GameSetup;
            var savedata = this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting.cGameListSettings;
            for(int num = 1; num < cSettingGamesCommon.cSettingGames.Length; num++)
            {
                var disp = cSettingGamesCommon.cSettingGames[num];
                disp.Name = disp.Title.text;
            }
        }
        UpdateFromLoadedData();
        */
    //}

/*
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 管理画面のゲーム情報をセーブデータから更新
    /// </summary>
    public void UpdateFromLoadedData()
    {
        ///全ゲーム共通
        var savedata = this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting;
        cSettingGamesCommon.SetHighLevel.isOn = savedata.mLowHeight;

        var disp = cSettingGamesCommon.cSettingGames;

        ///ゲーム個別設定
        if (!sOGameLevel.IsMockup)
        {
            for (int num = 0; num < savedata.cGameListSetting.Count; num++)
            {
                savedata.cGameListSetting[num].Name = disp[num].Name = ((SOGameLevel.EGameLists)num).ToString();
                disp[num].Timelimit[(int)(savedata.cGameListSetting[num].mTimeLimit)].isOn = true;
            }
        }
        else
        {///プロトタイプなのでLoadされたデータは使わないで初期化
            savedata.cGameListSetting.Clear();
            for (int cnt = 0
            ; cnt < Enum.GetValues(typeof(SOGameLevel.EPrototypeGameLists)).Length
            ; cnt++)
            {
                savedata.cGameListSetting.Add(new SaveDataAdmin.CSaveDataTotal.CGameSetting.CGameListSetting());
                savedata.cGameListSetting[cnt].Name = disp[cnt].Name = ((SOGameLevel.EPrototypeGameLists)cnt).ToString();
                disp[cnt].Timelimit[(int)(savedata.cGameListSetting[cnt].mTimeLimit)].isOn = true;
            }
        }
        Debug.Log(this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting.ToString());
    }

    /// <summary>
    /// 管理画面の難易度情報をセーブデータに反映
    /// </summary>
    public void ToggleHighLevel()
    {
        var savedata = this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting;
        savedata.mLowHeight = cSettingGamesCommon.SetHighLevel.isOn;
    }
#if false
    public void ToggleKinectOnOff(Toggle toggle)
    {
        var savedata = this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting.cGameListSettings;
        SaveDataAdmin.CSaveDataTotal.EGameLists gameID = SaveDataAdmin.CSaveDataTotal.EGameLists.GameSetup;
        switch(toggle.transform.parent.parent.parent.name)
        {
            case "B0":
                gameID = SaveDataAdmin.CSaveDataTotal.EGameLists.GameFgKickDefenses;
                break;
            case "C0":
                gameID = SaveDataAdmin.CSaveDataTotal.EGameLists.GameFgKickDefenses;
                break;
            case "D0":
                gameID = SaveDataAdmin.CSaveDataTotal.EGameLists.GameWgTouchTheNumbers;
                break;
            case "E0":
                gameID = SaveDataAdmin.CSaveDataTotal.EGameLists.GameFgStepAndShoot;
                break;
        }
        switch(toggle.transform.parent.name)
        {
            case "K0":
                savedata[(int)gameID].mIsActiveKinects[0].mUsing = toggle.isOn;
                break;
            case "K1":
                savedata[(int)gameID].mIsActiveKinects[1].mUsing = toggle.isOn;
                break;
        }
    }
#endif
    /// <summary>
    /// 管理画面の制限時間情報が変更されたとき、セーブデータに反映
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleTimeLimit(Toggle toggle, Assets.Scripts._System.AdminGameList _gameList)
    {
        if (!toggle.isOn) return;
        var savedata = this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting.cGameListSetting;

        int gameIDint = 0;
        if (!sOGameLevel.IsMockup)
        {
            var gameID = (SOGameLevel.EGameLists)_gameList.IntEGameLists;
            Debug.Log("game:" + gameID.ToString());
            gameIDint = (int)gameID;
        }
        else
        {
            var gameID = (SOGameLevel.EPrototypeGameLists)_gameList.IntEGameLists;
            Debug.Log("game:" + gameID.ToString());
            gameIDint = (int)gameID;
        }

        switch (toggle.transform.parent.name)
        {
            case "TimeLimit060":
                savedata[gameIDint].mTimeLimit = SOGameLevel.ETimeLimit.sec60;
                break;
            case "TimeLimit090":
                savedata[gameIDint].mTimeLimit = SOGameLevel.ETimeLimit.sec90;
                break;
            case "TimeLimit180":
                savedata[gameIDint].mTimeLimit = SOGameLevel.ETimeLimit.sec180;
                break;
        }
        Debug.Log(savedata[gameIDint].mTimeLimit.ToString());
    }
*/
}
