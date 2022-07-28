using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;
using Microsoft.Azure.Kinect.Sensor;

public class CntrSaveDataKinect : MonoBehaviour
{
    /*
    [System.Serializable]
    public class CSettingKinect
    {
        [System.Serializable]
        public class Vector3Field
        {
            public InputField x, y, z;
        }
        public string Name;
        public Toggle[] FOV = new Toggle[4];
        public Text KinectSerialNumbersFOV;
        public Text[] KinectSerialNumbers;
        public Text KinectSerialNumbersGames;
        public Vector3Field Rotate;
        public Vector3Field Translate;
        public InputField RoomScale;
        public Text Height;
        public GameObject Menu;
    }
    [Header("キネクトセッティング画面")]
    public CSettingKinect[] cSettingKinect = new CSettingKinect[2];

    public enum EKinect
    {
        device0,
        device1,
    }
    public EKinect kinectDevice;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        var deviceCount = this.GetComponent<KinectMain>().DeviceCount;
        for (var devId = 0; devId < deviceCount; devId++)
        {
            var disp = cSettingKinect[devId];
            Vector3 deltaCamAxis = new Vector3(0f, 0f, 0f);///深度カメラのIMU差分補正値
            var savedata = savedataTotal.cKinectSetting[devId];
            {
                ///FOVのトグル
                ///https://docs.microsoft.com/ja-jp/azure/kinect-dk/coordinate-systems#depth-and-color-camera
                switch (savedata.mDepthMode)
                {///深度カメラのIMU差分補正
                    case DepthMode.Off:
                        savedata.mDepthMode = DepthMode.NFOV_2x2Binned;
                        deltaCamAxis = new Vector3(-6f, 0f, 0f);
                        break;
                    case DepthMode.PassiveIR:
                        savedata.mDepthMode = DepthMode.WFOV_Unbinned;
                        deltaCamAxis = new Vector3(-6f - 1.3f, 0f, 0f);
                        break;
                    case DepthMode.NFOV_2x2Binned:
                        deltaCamAxis = new Vector3(-6f, 0f, 0f);
                        break;
                    case DepthMode.NFOV_Unbinned:
                        deltaCamAxis = new Vector3(-6f, 0f, 0f);
                        break;
                    case DepthMode.WFOV_2x2Binned:
                        deltaCamAxis = new Vector3(-6f - 1.3f, 0f, 0f);
                        break;
                    case DepthMode.WFOV_Unbinned:
                        deltaCamAxis = new Vector3(-6f - 1.3f, 0f, 0f);
                        break;

                }
                int dispPos = (int)savedata.mDepthMode - (int)DepthMode.NFOV_2x2Binned;
                disp.FOV[dispPos].isOn = true;
            }
            disp.Name = savedata.mName;
            disp.KinectSerialNumbers[0].text = savedata.mName + "(" + savedata.mDepthMode.ToString() + ")";
            //disp.KinectSerialNumbers[1].text = savedata.mName + "(" + savedata.mDepthMode.ToString() + ")";
            disp.KinectSerialNumbersFOV.text = savedata.mName;
            //disp.KinectSerialNumbersGames.text = savedata.mName;

            {
                //savedata.mRoomScale = float.Parse(disp.RoomScale.text);
                savedata.mHeight = float.Parse(disp.Height.text);
                savedata.mRotate = new Vector3(
                    float.Parse(disp.Rotate.x.text),
                    float.Parse(disp.Rotate.y.text),
                    float.Parse(disp.Rotate.z.text)
                    );
                //savedata.mRotate += deltaCamAxis;
                savedata.mTranslate = new Vector3(
                    float.Parse(disp.Translate.x.text),
                    savedata.mHeight,
                    float.Parse(disp.Translate.z.text)
                    );

                var serialNum = this.cSettingKinect[devId].Name;
                var skeletonObj = this.transform.GetComponent<KinectMain>().m_tracker.ToArray();

                //int num = 0;
                if (devId == (int)kinectDevice && this.GetComponent<KinectMain>().m_lastFrameData[devId] != null)
                {
                    for (var num = 0; num < (int)this.GetComponent<KinectMain>().m_lastFrameData[devId].NumOfBodies; num++)
                    {
                        skeletonObj[num].GetComponent<TrackerHandler>().Height = savedata.mHeight;
                        //if(skeletonObj[num].GetComponent<TrackerHandler>().DeviceID == serialNum)
                        {
                            //skeletonObj[num].transform.Find("PositionBody").rotation = Quaternion.Euler(
                            skeletonObj[num].transform.parent.rotation = Quaternion.Euler(
                                savedata.mRotate.x + deltaCamAxis.x,
                                savedata.mRotate.y + deltaCamAxis.y,
                                savedata.mRotate.z + deltaCamAxis.z
                                );
                            //skeletonObj[num].transform.Find("PositionBody").position = new Vector3(
                            skeletonObj[num].transform.parent.position = new Vector3(
                                savedata.mTranslate.x,
                                savedata.mTranslate.y,
                                savedata.mTranslate.z
                            );
                            //skeletonObj[num].GetComponent<TrackerHandler>().SavedataGlobalPositionOffset = savedata.mTranslate;

                        }
                    }
                }
            }
        }
    }

    public void UpdateFromLoadedData()
    {
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        for (var num = 0; num < 1; num++)
        {
            var disp = cSettingKinect[num];
            SaveDataAdmin.CSaveDataTotal.CKinectSetting savedata = new SaveDataAdmin.CSaveDataTotal.CKinectSetting();
            savedata = savedataTotal.cKinectSetting[num];
            disp.Height.text = savedata.mHeight.ToString();
            //disp.RoomScale.text = savedata.mRoomScale.ToString("###0.0");
            //disp.RoomScale.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mRoomScale;
            disp.Rotate.x.text = savedata.mRotate.x.ToString("##0.0");
            disp.Rotate.x.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mRotate.x;
            disp.Rotate.y.text = savedata.mRotate.y.ToString("##0.0");
            disp.Rotate.y.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mRotate.y;
            disp.Rotate.z.text = savedata.mRotate.z.ToString("##0.0");
            disp.Rotate.z.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mRotate.z;
            disp.Translate.x.text = savedata.mTranslate.x.ToString("###0.0");
            disp.Translate.x.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mTranslate.x;
            //disp.Translate.y.text = savedata.mTranslate.y.ToString("###0.0");
            //disp.Translate.y.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mTranslate.y;
            disp.Translate.z.text = savedata.mTranslate.z.ToString("###0.0");
            disp.Translate.z.transform.parent.Find("Slider").GetComponent<Slider>().value = savedata.mTranslate.z;
        }
    }

#if false
    public void UpdateKinectFOV(Toggle toggle)
    {
        if(toggle.isOn)
        {
            int devId=0;
            var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
            switch(toggle.transform.parent.parent.name)
            {
                case "Kinect0":
                    devId = 0;
                    break;
                case "Kinect1":
                    devId = 1;
                    break;
            }
            DepthMode depthMode = DepthMode.NFOV_2x2Binned;
            switch(toggle.gameObject.name)
            {
                case "Toggle1":
                    depthMode = DepthMode.NFOV_2x2Binned;
                    break;
                case "Toggle2":
                    depthMode = DepthMode.NFOV_Unbinned;
                    break;
                case "Toggle3":
                    depthMode = DepthMode.WFOV_2x2Binned;
                    break;
                case "Toggle4":
                    depthMode = DepthMode.WFOV_Unbinned;
                    break;
            }
            savedataTotal.cKinectSetting[devId].mDepthMode = depthMode;
        }
    }
#endif

    public void ChangeKinect0()
    {
        kinectDevice = EKinect.device0;
        cSettingKinect[(int)EKinect.device0].Menu.SetActive(true);
        cSettingKinect[(int)EKinect.device1].Menu.SetActive(false);
        this.GetComponent<KinectMain>().Seq[0] = KinectMain.ESeq.openDevice;
        this.GetComponent<KinectMain>().Seq[1] = KinectMain.ESeq.closeDevice;
    }
    public void ChangeKinect1()
    {
        kinectDevice = EKinect.device1;
        cSettingKinect[(int)EKinect.device1].Menu.SetActive(true);
        cSettingKinect[(int)EKinect.device0].Menu.SetActive(false);
        this.GetComponent<KinectMain>().Seq[1] = KinectMain.ESeq.openDevice;
        this.GetComponent<KinectMain>().Seq[0] = KinectMain.ESeq.closeDevice;
    }
    */
}
