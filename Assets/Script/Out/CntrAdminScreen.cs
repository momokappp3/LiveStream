using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using Assets.Scripts.Game.UI;
using Assets.Scripts._System;

namespace Assets.Scripts._System
{
    /// <summary>
    /// 管理画面のメインコントロール
    /// </summary>
	public class CntrAdminScreen : MonoBehaviour
    {
        /*
        /// ------------------------------------
        [Header("【 ScriptableObject 】")]
        [SerializeField] SOGameLevel sOGameLevel;
        [SerializeField] CanvasGroup canvasGroupGames;
        [SerializeField] Button buttonSetup;
        [SerializeField] CntrSaveDataGames saveDataGames;
        /// ------------------------------------
        [Header("【 UI 】")]
        [SerializeField] DisplayUI_Wall displayUI_Wall;
        [SerializeField] InputField userID;
        [SerializeField] Dropdown nameList;
        [SerializeField] Transform parentGameList;  ///ゲームボタンの親
        [SerializeField] GameObject gameList;       ///ゲームボタンのPrefab
        [SerializeField] Button saveButton;
        [SerializeField] Text gameModeText;
        /// ------------------------------------
        [Header("【 Logger 】")]
        [SerializeField] _GameLogger.Logger logger;
        /// ------------------------------------
        [SerializeField] SceneTransition sceneTransition;

        private GameObject instantiatedObject;
        public List<GameObject> InstanceGameLists = new List<GameObject>();
        public int EGameLists { get; private set; }
        //UnityEngine.Events.UnityAction inputFieldAction;
        //UnityEngine.Events.UnityAction dropDownAction;

        private void Awake()
        {
            userID.onEndEdit.AddListener(SearchUserUseID);
            nameList.onValueChanged.AddListener(ChangeUser);
        }

        private void Start()
        {
            ///モックアップの時はセーブ出来ないようにする
            ///※ゲーム以外のパラメータはIsMockupをfalseの状態でセーブすればモックアップでも使える
            sceneTransition.ActivateStartButton(!sOGameLevel.IsMockup);
            ///モックアップ対応版ゲーム選択メニューの作成
            float offsetY = 0f;
            int contents = sOGameLevel.IsMockup ? sOGameLevel.cPrototypeGameInfo.Length : sOGameLevel.cGameInfo.Length;
            Vector2 sizeDelta = parentGameList.GetComponent<RectTransform>().sizeDelta;
            parentGameList.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, contents * 100f);
            var cntrSaveDataGames = this.GetComponent<CntrSaveDataGames>();
            for (int cnt = 0; cnt < contents; cnt++)
            {
                InstanceGameLists.Add(Instantiate(gameList, parentGameList));
                string nameStr = sOGameLevel.IsMockup ? sOGameLevel.cPrototypeGameInfo[cnt].name : sOGameLevel.cGameInfo[cnt].name;
                InstanceGameLists[cnt].transform.Find("TitleButton").Find("Text").GetComponent<Text>().text = nameStr;
                InstanceGameLists[cnt].transform.localPosition = new Vector3(0f, offsetY, 0f);
                InstanceGameLists[cnt].GetComponent<AdminGameList>().cntrAdminScreen = this;
                InstanceGameLists[cnt].GetComponent<AdminGameList>().IntEGameLists = sOGameLevel.IsMockup ? (int)sOGameLevel.cPrototypeGameInfo[cnt].eGameLists : (int)sOGameLevel.cGameInfo[cnt].eGameLists;
                InstanceGameLists[cnt].GetComponent<AdminGameList>().cntrSaveDataGames = saveDataGames;
                for (int i = 0; i < 3; i++)
                {
                    cntrSaveDataGames.cSettingGamesCommon.cSettingGames[cnt].Timelimit[i]
                        = InstanceGameLists[cnt].GetComponent<AdminGameList>().TimeLimit[i].transform.Find("Toggle").GetComponent<Toggle>();
                }
                offsetY -= 100f;
            }
            if (sOGameLevel.IsMockup)
            {
                saveButton.interactable = false;
                gameModeText.text = "モックアップ版";
                gameModeText.transform.parent.GetComponent<Image>().color = Color.red;
            }
            else
            {
                saveButton.interactable = true;
                gameModeText.text = "製品版";
                gameModeText.transform.parent.GetComponent<Image>().color = Color.white;
            }
        }
        private void Update()
        {
            if (userID.text.Length < 4)
                sceneTransition.ActivateStartButton(false);
            else
            {

                if (sceneTransition.IsInGame)
                {
                    sceneTransition.ActivateStartButton(false);
                }
                else
                {
                    if (!buttonSetup.interactable)
                    {
                        sceneTransition.ActivateStartButton(false);
                    }
                    else
                    {
                        sceneTransition.ActivateStartButton(true);
                    }
                }
            }
            if (sOGameLevel.IsMockup)
            {
                saveButton.interactable = false;
            }
        }

        /// <summary>
        /// loggerからデータが拾えなかったときにユーザー名とIDをロックする
        /// </summary>
        public void GuestOnly()
        {
            userID.onEndEdit.RemoveListener(SearchUserUseID);
            nameList.onValueChanged.RemoveListener(ChangeUser);
            userID.readOnly = true;
            nameList.enabled = false;
            userID.interactable = false;
            nameList.interactable = false;
        }

        /// <summary>
        /// 管理画面Startボタン：OnClick
		/// </summary>
        public void CntrGameStart()
        {
            if (userID.text.Length == 4)
            {
                sceneTransition.ActivateStartButton(true);
                sceneTransition.StartGame();
            }
        }

        /// <summary>
        /// 管理画面＜ユーザー名＞dropDown：OnValueChange
        /// ユーザー名登録と表示ID更新
        /// </summary>
        public void ChangeUser(int _result)
        {
            //if (logger.UserNameId == null) return;
            ///選択されたユーザー名（桁成型後）
			var dispname = displayUI_Wall.SetUserNameWall();
            string userName = "";
            foreach (var user in logger.UserNameId.userIds)
            {
                userName = user.name;
                ///DBのユーザー名を桁成型
                if (user.name.Length > sOGameLevel.UserNameColumn)
                    userName = user.name.Substring(0, sOGameLevel.UserNameColumn);
                if (userName == dispname)
                {
                    ///onValueChanged は呼び出されない
                    userID.onValueChanged.RemoveListener(SearchUserUseID);
                    userID.text = user.userId.ToString("D4");
                    userID.onValueChanged.AddListener(SearchUserUseID);

                    sceneTransition.ActivateStartButton(true);
                }
            }
        }
        /// <summary>
        /// 管理画面＜ユーザーID＞InputField：OnValueChange
        /// </summary>
        public void SearchUserUseID(string _result)
        {
            //if (logger.UserNameId == null) return;
            if (string.IsNullOrEmpty(userID.text))
                return;
            ///入力されたID
            var serch = Int32.Parse(userID.text);
            if (serch == 0) return;
            Debug.Log(userID.text);
            Debug.Log(Int32.Parse(userID.text).ToString("D4"));
            Debug.Log(Int32.Parse(userID.text).ToString("D4").Substring(0, userID.text.Length));

            //var names = new List<string>();
            //nameList.ClearOptions();
            int count = 0;
            foreach (var userid in logger.UserNameId.userIds)
            {
                ///DBからユーザーIDの取得（成型）
                //var id = userid.userId.ToString("D4").Substring(0, userID.text.Length);
                var id = userid.userId;
                if (serch == id)
                {
                    ///合致したユーザー名を成型して取得
                    //var name = userid.name;
                    //if(name.Length > sOGameLevel.UserNameColumn)
                    //{
                    //    name = name.Substring(0, sOGameLevel.UserNameColumn);
                    //}
                    break;
                }
                count++;
            }
            nameList.value = count;
            //nameList.AddOptions(names);
            //displayUI_Wall.SetUserNameWall();
            sceneTransition.ActivateStartButton(true);
        }

        public void LoadSetUp()
        {
            EGameLists = -1;

            Destroy(instantiatedObject);

            var aGameOut = sOGameLevel.GameOut_SetUp;
            if (aGameOut != null)
                Addressables.LoadAssetAsync<GameObject>(aGameOut).Completed += ObjectLoadDone;
            else
                Assert.IsNotNull(aGameOut, "aGameInはnullです！");
        }
        /// <summary>
        /// モックアップ版対応ゲームローダー
        /// </summary>
        /// <param name="_eGameLists">ゲームＩＤ</param>
        public void LoadGame(int _eGameLists)
        {
            EGameLists = _eGameLists;

            canvasGroupGames.interactable = false;
            var setting = this.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting;
            displayUI_Wall.gameObject.SetActive(true);

            buttonSetup.interactable = true;
            foreach (var gameList in InstanceGameLists)
            {
                gameList.transform.Find("TitleButton").GetComponent<Button>().interactable = false;
            }

            Destroy(instantiatedObject);

            if (sOGameLevel.IsMockup)
            {///モックアップ版ロード
                var aGameIn = getAssetReferencePrototype((SOGameLevel.EPrototypeGameLists)_eGameLists);
                if (aGameIn != null)
                    Addressables.LoadAssetAsync<GameObject>(aGameIn).Completed += ObjectLoadDone;
                else
                    Assert.IsNotNull(aGameIn, "aGameInはnullです！");
            }
            else
            {///製品版ロード
                var aGameIn = getAssetReference((SOGameLevel.EGameLists)_eGameLists);
                if (aGameIn != null)
                    Addressables.LoadAssetAsync<GameObject>(aGameIn).Completed += ObjectLoadDone;
                else
                    Assert.IsNotNull(aGameIn, "aGameInはnullです！");
            }
        }
        private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject loadedObject = obj.Result;
                Debug.Log("Successfully loaded object.");
                instantiatedObject = Instantiate(loadedObject);
                instantiatedObject.transform.GetComponent<SetParam>().MSceneTransition = sceneTransition;
                instantiatedObject.transform.GetComponent<SetParam>().MKinectMain = this.GetComponent<KinectMain>();
                instantiatedObject.transform.GetComponent<SetParam>().MDisplayUI_Wall = displayUI_Wall;
                instantiatedObject.transform.GetComponent<SetParam>().MLogger = this.GetComponent<_GameLogger.Logger>();
                Debug.Log("Successfully instantiated object." + instantiatedObject.name);
#if false
                if (accessoryObjectToLoad != null)
                {
                    accessoryObjectToLoad.InstantiateAsync(instantiatedObject.transform, true).Completed += op =>
                    {
                        if (op.Status == AsyncOperationStatus.Succeeded)
                        {
                            instantiatedAccessoryObject = op.Result;
                            Debug.Log("Successfully loaded and instantiated accessory object." + op.DebugName);
                        }
                    };
                }
#endif
            }
        }
        /// <summary>
        /// CGameInfoで対応するゲームを特定する（ゲームセレクトのボタン順が変更される可能性が高い為）
        /// 管理画面UIのタイトル表示も確定する
        /// </summary>
        /// <param name="_eGameLists"></param>
        /// <returns></returns>
        AssetReference getAssetReference(SOGameLevel.EGameLists _eGameLists)
        {
            ///初期化
            AssetReference aGameIn = sOGameLevel.cGameInfo[0].aGameIn;
            foreach (var game in sOGameLevel.cGameInfo)
            {
                if (game.eGameLists == _eGameLists)
                {
                    displayUI_Wall.GameTitleString(game.name);
                    Debug.Log(game.aGameIn);
                    return game.aGameIn;
                }
            }
            return null;
        }
        AssetReference getAssetReferencePrototype(SOGameLevel.EPrototypeGameLists _eGameLists)
        {
            ///初期化
            AssetReference aGameIn = sOGameLevel.cGameInfo[0].aGameIn;
            foreach (var game in sOGameLevel.cPrototypeGameInfo)
            {
                if (game.eGameLists == _eGameLists)
                {
                    displayUI_Wall.GameTitleString(game.name);
                    Debug.Log(game.aGameIn);
                    return game.aGameIn;
                }
            }
            return null;
        }
        */
#if false
        public void SetDropDownNameList()
        {
            var names = new List<string>();
            nameList.ClearOptions();
            foreach(var userid in logger.UserNameId.userIds)
            {
                Debug.Log(userid.userId + ":" + userid.name);
                var name = userid.name;
                if(name.Length > sOGameLevel.UserNameColumn)
                {
                    name = name.Substring(0, sOGameLevel.UserNameColumn);
                }
                names.Add(name);
            }
            nameList.AddOptions(names);
        }         
#endif
    }

}