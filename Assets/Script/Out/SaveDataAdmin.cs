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
        /// �o�[�W�����ԍ��̏ڍ�
        /// ����ʂ̐����ς�����牺�ʂ̓��Z�b�g����
        /// </summary>
        public enum EVersionCodes
        {
            Measure_ID,     ///�Q�[���S�̃o�[�W����
            Games_Added,    ///�~�j�Q�[����Measure_ID�X�V���������ǉ����ꂽ��
            Method_Added,   ///�@�\�ǉ���
            BugFixed_Num,   ///�o�O�C����
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
                public string Name; ///�Q�[���R�[�h
                public enum ETimeLimit
                {
                    sec60,
                    sec90,
                    sec180,
                };

                [System.Serializable]
                public class CIsUsingKinect
                {
                    public string Name;   ///Kinect�̃V���A���ԍ�
                    public bool mUsing;         ///���̃Q�[���Ŏg�����H
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
            public bool mLowHeight = true;        ///��g��
            public bool mIsDownLevel = false;     ///�C���^���N�e�B�u�ȃ��x���_�E��
                                                  ///�Q�[�����̐ݒ�
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

        ///ProjectSetting�̒l
        public string Version = "0.01";
        /// Kinect�̐ݒ�
        public CKinectSetting[] cKinectSetting;// = new CKinectSetting[2];
        /// �Q�[���̐ݒ�
        public CGameSetting cGameSetting;
        ///�}���`�X�N���[���̐ݒ�
        public CSettingScreen cSettingScreen;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    /// <summary>
    /// SaveData�{��
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
        {///�Z�[�u�f�[�^�ɃQ�[���̃Z�b�e�B���O��񂪂�����
            Debug.Log(" HasKey");
            ///�Z�[�u�f�[�^�̃��[�h
            SaveDataTotal = JsonUtility.FromJson<CSaveDataTotal>(PlayerPrefs.GetString("GamePlaySettings"));
            ///Version�ԍ��̉��
            CSaveDataTotal.EVersionCodes[] versionCodes = (CSaveDataTotal.EVersionCodes[])Enum.GetValues(typeof(CSaveDataTotal.EVersionCodes));
            //Range��0����v�f���܂ł̃V�[�P���X���쐬���AToDictionary��Dictionary�����
            savedVersion = Enumerable.Range(0, versionCodes.Length).ToDictionary(i => versionCodes[i], i => SaveDataTotal.Version.Split('.')[i]);
            appliVersion = Enumerable.Range(0, versionCodes.Length).ToDictionary(i => versionCodes[i], i => Application.version.Split('.')[i]);
            if (SaveDataTotal.Version != "" &&
                savedVersion[CSaveDataTotal.EVersionCodes.Measure_ID] == appliVersion[CSaveDataTotal.EVersionCodes.Measure_ID] &&
                savedVersion[CSaveDataTotal.EVersionCodes.Games_Added] == appliVersion[CSaveDataTotal.EVersionCodes.Games_Added] &&
                savedVersion[CSaveDataTotal.EVersionCodes.Method_Added] == appliVersion[CSaveDataTotal.EVersionCodes.Method_Added] &&
                SaveDataTotal.cGameSetting.cGameListSetting.Count != 0)
            {///�Z�[�u�f�[�^�ƃo�[�W��������v
             ///���[�h�����f�[�^�ŊǗ���ʂ��Z�b�g
                this.GetComponent<CntrSaveDataKinect>().UpdateFromLoadedData();
                this.GetComponent<CntrSaveDataGames>().UpdateFromLoadedData();
                this.GetComponent<CntrSaveDataScreen>().UpdateFromLoadedData();
                Debug.Log("SaveData Found");
            }
            else
            {///�o�[�W�������オ�����炷�ׂă��Z�b�g
             ///��U�Z�[�u�f�[�^���N���A
             ///CGameSetting�̏������i������ƃ_���j
                resetAdminData();
                Debug.Log("Game Version Up to date.");
            }
        }
        else
        {///�f�[�^���Ȃ��̂ŏ����l�ŃZ�b�g
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
    /// �Ǘ���ʏ��̃Z�[�u
    /// </summary>
    public void SaveAdminData()
    {
        ///CKinectSetting�̏�����

        ///CSettingScreen�̏�����

        ///�A�v���P�[�V�����o�[�W�����̍X�V
        SaveDataTotal.Version = Application.version;
        ///JSON�����ăZ�[�u
        var savedata = JsonUtility.ToJson(SaveDataTotal);
        PlayerPrefs.SetString("GamePlaySettings", savedata);
        PlayerPrefs.Save();
        ///�������񂾂��̂ŃL���b�V�����Đݒ�
        Debug.Log("SaveData Updated");
        SaveDataTotal = JsonUtility.FromJson<CSaveDataTotal>(PlayerPrefs.GetString("GamePlaySettings"));
        Debug.Log(SaveDataTotal.ToString());
    }

    private void resetAdminData()
    {
        ///CGameSetting�̏������i������ƃ_���j
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
        {///���b�N�A�b�v�ł̏ꍇ��PlayerPrefs���X�V���Ȃ�
         ///��U�Z�[�u�f�[�^���N���A
            PlayerPrefs.DeleteAll();
            SaveAdminData();
        }
    }
    */
}
