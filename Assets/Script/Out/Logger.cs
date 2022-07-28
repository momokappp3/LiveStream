using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Assets.Scripts;

namespace Assets.Scripts._System._GameLogger
{

    public class Logger : MonoBehaviour
    {
        /*
        /// ------------------------------------
        [Header("【 ScriptableObject 】")]
        [SerializeField] SOGameLevel sOGameLevel;
        /// ------------------------------------
        [Header("【 UI 】")]
        [SerializeField] Dropdown nameList;
        [SerializeField] InputField iD;
        /// ------------------------------------
        [Header("【 CntrAdminScreen 】")]
        [SerializeField] CntrAdminScreen cntrAdminScreen;
        /// ------------------------------------
        [SerializeField] SaveDataAdmin saveDataAdmin;
        public enum EGameLists
        {
            GameFgKickDefenses,
            GameWgTouchTheNumbers,
            GameFgStepAndShoot,
            GameFgBG,
            GameFgIS,
            GamePgSJM,
            GameLength,
        }
        public static Logger Instance
        {
            get; private set;
        }

        public UserNameIdJson UserNameId
        {
            get; private set;
        }

        [Serializable]
        public class UserId
        {
            public int userId;
            public string name;
        }

        [Serializable]
        public class UserNameIdJson
        {
            public string result;
            public UserId[] userIds;
        }
#if false
        /// <summary>
        /// 管理画面＜ユーザーID＞InputField：OnValueChange
        /// </summary>
        public void SearchUserUseID()
        {
            if(string.IsNullOrEmpty(iD.text))
                return;
            var serch= Int32.Parse(iD.text).ToString("D4").Substring(0,iD.text.Length);

            var names = new List<string>();
            nameList.ClearOptions();
            foreach(var userid in UserNameId.userIds)
            {
                var id = userid.userId.ToString("D4").Substring(0,iD.text.Length);
                if(serch == id)
                {
                    var name = userid.name;
                    if(name.Length > 12)
                    {
                        name = name.Substring(0, 12);
                    }
                    names.Add(name);
                }
            }
            nameList.AddOptions(names);
            displayUI_Wall.SetUserNameWall();
        }
#endif
        // 1. ユーザーデータのリクエスト（特定のユーザー）
        // <送信データ>
        // userId: ユーザーID
        // <例>
        // 0001
        // <戻り値の例>
        // OK,桃太郎

        IEnumerator GetUserInfo(string userId)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://" + sOGameLevel.LogServerAddress + "/api_user_info.php?id=" + userId);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var str = www.downloadHandler.text;
                // Show results as text
                Debug.Log(str);
                var jsondata = JsonUtility.FromJson<UserNameIdJson>(str);
                Debug.Log("result:" + jsondata.result);
                foreach (var userid in jsondata.userIds)
                {
                    Debug.Log(userid.userId + ":" + userid.name);
                }

                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        // 2. ユーザーデータのリクエスト（ユーザーの一覧）
        // <送信データ>
        // all: 常数
        // <戻り値の例>
        // OK,0001/桃太郎,0002/桃次郎,0002/桃三郎
        // OK{,ユーザーID/ユーザー名}*n ....  コッマ区切りで全員のIDと名前を揃えて送信

        IEnumerator GetUserInfoAll()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://" + sOGameLevel.LogServerAddress + "/api_user_info.php?id=all");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var str = www.downloadHandler.text;
                // Show results as text
                Debug.Log(str);
                UserNameId = JsonUtility.FromJson<UserNameIdJson>(str);
                Debug.Log("result:" + UserNameId.result);
                var names = new List<string>();
                nameList.ClearOptions();
                foreach (var userid in UserNameId.userIds)
                {
                    Debug.Log(userid.userId + ":" + userid.name);
                    var name = userid.name;
                    if (name.Length > 12)
                    {
                        name = name.Substring(0, 12);
                    }
                    names.Add(name);
                }
                nameList.AddOptions(names);
            }
            else
            {
                cntrAdminScreen.GuestOnly();
                Debug.Log("<Color=blue>No User DATA</color>");
                Debug.Log(www.error);
            }
        }

        // 3. ゲームデータの送信
        // <送信データ>
        // userId: ユーザーID
        // tstamp: タイムスタンプ
        // gid: ゲームＩＤ
        // tlimit: 制限時間
        // diff: 難易度
        // point: 得点
        // <例>
        // 0001,2021-10-31 11:07:40,03,00,01,260
        // <戻り値の例>
        // OK

        IEnumerator SendGameInfo(string userId, string tstamp, string gid, string tlimit, string diff, string point)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://" + sOGameLevel.LogServerAddress + "/api_data_report.php?data=" + userId + "," + tstamp + "," + gid + "," + tlimit + "," + diff + "," + point);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        /// <summary>
        /// ゲームリストの更新リクエスト
        /// </summary>
        /// <returns></returns>
        IEnumerator SetGameInfoAll()
        {
            string arg = "{" + '"' + "gameIds" + '"' + ":[";

            foreach (var gameId in sOGameLevel.cGameInfo)
            {
                arg += "{"
                    + '"' + "gameId" + '"' + ":" + ((int)gameId.eGameLists).ToString() + ","
                    + '"' + "developmentCode" + '"' + ":" + '"' + gameId.eGameLists.ToString() + '"' + ","
                    + '"' + "displayTitle" + '"' + ":" + '"' + gameId.name + '"' + "},";
            }
            arg = arg.Substring(0, arg.Length - 1);
            arg += "]}";

            UnityWebRequest www = UnityWebRequest.Get("http://" + sOGameLevel.LogServerAddress + "/api_game_list_report.php?data=" + arg);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
            else
            {
                Debug.Log(www.error);
            }
        }
        [Serializable]
        public class InputJson
        {
            // 追加したAAAクラスの変数aaa。変数名は入力ファイルと揃える。
            public UserId[] aaa;
        }

        // 追加
        [Serializable]
        public class AAA
        {
            public int bbb;
            public int ccc;
        }


        void Start()
        {
            Debug.Log("Start");
            StartCoroutine("GetUserInfoAll");
            StartCoroutine("SetGameInfoAll");
            //StartCoroutine("GetUserInfo", "2");
            //var time = DateTime.Now;
            //var timeForm = time.ToString( "yyyy-MM-dd HH:mm:ss" );
            //StartCoroutine(SendGameInfo("2", timeForm, "1", "1", "1", "1000"));
        }

        public UserNameIdJson GetUserList()
        {
            StartCoroutine("GetUserInfoAll");
            return UserNameId;
        }

        public void PushGameLog(SOGameLevel.EGameLists _gameId, int _score)
        {
            if (iD.text.Length < 4)
                return;
            var time = DateTime.Now;
            var timeForm = time.ToString("yyyy-MM-dd HH:mm:ss");
            var timemode = (int)saveDataAdmin.SaveDataTotal.cGameSetting.cGameListSetting[(int)_gameId].mTimeLimit;
            var gameId = (int)_gameId;
            var difficulty = saveDataAdmin.SaveDataTotal.cGameSetting.mLowHeight ? 1 : 0;
            StartCoroutine(SendGameInfo(iD.text, timeForm, gameId.ToString(), timemode.ToString(), difficulty.ToString(), _score.ToString()));
        }
                */
    }
}
