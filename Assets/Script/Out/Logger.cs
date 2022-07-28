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
        [Header("�y ScriptableObject �z")]
        [SerializeField] SOGameLevel sOGameLevel;
        /// ------------------------------------
        [Header("�y UI �z")]
        [SerializeField] Dropdown nameList;
        [SerializeField] InputField iD;
        /// ------------------------------------
        [Header("�y CntrAdminScreen �z")]
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
        /// �Ǘ���ʁ����[�U�[ID��InputField�FOnValueChange
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
        // 1. ���[�U�[�f�[�^�̃��N�G�X�g�i����̃��[�U�[�j
        // <���M�f�[�^>
        // userId: ���[�U�[ID
        // <��>
        // 0001
        // <�߂�l�̗�>
        // OK,�����Y

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

        // 2. ���[�U�[�f�[�^�̃��N�G�X�g�i���[�U�[�̈ꗗ�j
        // <���M�f�[�^>
        // all: �퐔
        // <�߂�l�̗�>
        // OK,0001/�����Y,0002/�����Y,0002/���O�Y
        // OK{,���[�U�[ID/���[�U�[��}*n ....  �R�b�}��؂�őS����ID�Ɩ��O�𑵂��đ��M

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

        // 3. �Q�[���f�[�^�̑��M
        // <���M�f�[�^>
        // userId: ���[�U�[ID
        // tstamp: �^�C���X�^���v
        // gid: �Q�[���h�c
        // tlimit: ��������
        // diff: ��Փx
        // point: ���_
        // <��>
        // 0001,2021-10-31 11:07:40,03,00,01,260
        // <�߂�l�̗�>
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
        /// �Q�[�����X�g�̍X�V���N�G�X�g
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
            // �ǉ�����AAA�N���X�̕ϐ�aaa�B�ϐ����͓��̓t�@�C���Ƒ�����B
            public UserId[] aaa;
        }

        // �ǉ�
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
