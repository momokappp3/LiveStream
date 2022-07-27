using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.BodyTracking;
//using TMPro;


public class KinectMain : MonoBehaviour
{
	/*

	// Handler for SkeletalTracking thread.
	public int DeviceCount;

	[Header("---------------------")]
	[Header("スケルトンの親")]
	[SerializeField] Transform AxisZero;
	[Header("スケルトンのPrefab")]
	[SerializeField] GameObject Kinect4AzureBodyTracker;
	[Header("スケルトンの実体")]
	public List<GameObject> m_tracker;
	[Header("管理画面用")]
	[SerializeField] Text between;
	[SerializeField] Text foot;
	[SerializeField] GameObject kinectKill;
	[Header("---------------------")]
	public List<GameObject> MTrackerHierarchy;
	private BackgroundDataProvider[] m_backgroundDataProvider;
	public BackgroundData[] m_lastFrameData;
	SkeletalTrackingProvider[] m_skeletalTrackingProvider;

	string[] deviceIDtext = { "", "" };
	//public bool[] DeviceEffectiveF={ true,true};

	Device[] device = new Device[2];

	int sampling = 0;
	public Vector3[] IMU = new Vector3[] { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) };


	//public int EffectiveDeviceId=-1;

	public enum ESeq
	{
		instanceDevice,
		checkDevice,
		modeSelectDevice,
		openDevice,
		settingDevice,
		startDevice,
		playDevice,
		closeDevice,
		idleDevice,
	}
	public ESeq[] Seq = { ESeq.instanceDevice, ESeq.instanceDevice };

	int SeqOnKinectQuit = 0;
	void Start()
	{
		SeqOnKinectQuit = 0;
		//tracker ids needed for when there are two trackers
		// 接続されているKinectの数をチェック
		DeviceCount = Device.GetInstalledCount();
		m_backgroundDataProvider = new BackgroundDataProvider[DeviceCount];
		m_lastFrameData = new BackgroundData[DeviceCount];
		m_skeletalTrackingProvider = new SkeletalTrackingProvider[DeviceCount];
		for (int num = 0; num < 8; num++)
		{
			m_tracker.Add(Instantiate(Kinect4AzureBodyTracker, Vector3.zero, Quaternion.identity, AxisZero.Find("Humans")));
			var handler = m_tracker[m_tracker.Count - 1].GetComponent<TrackerHandler>();
			handler.between = between;
			handler.foot = foot;
			handler.savedataKinect = this.GetComponent<CntrSaveDataKinect>();

		}
	}

	public void SeqIncrement()
	{
		for (int devId = 0; devId < DeviceCount; devId++)
		{
			Seq[devId]++;
		}
	}

	void LateUpdate()
	{
		//DeviceCount = Device.GetInstalledCount();
		//var devId = (int)this.transform.GetComponent<CntrSaveDataKinect>().kinectDevice;
		for (int devId = 0; devId < DeviceCount; devId++)
		{
			//if(!DeviceEffectiveF[devId])
			//	continue;
			switch (Seq[devId])
			{
				case ESeq.instanceDevice:
					foreach (var tracker in m_tracker)
					{
						tracker.SetActive(false);
					}
					m_lastFrameData[devId] = new BackgroundData();
					//tracker ids needed for when there are two trackers
					//const int TRACKER_ID = 0;
#if false
					m_skeletalTrackingProvider[devId] = new SkeletalTrackingProvider();
#else
					m_skeletalTrackingProvider[devId] = new SkeletalTrackingProvider(devId);
#endif
					//device[devId] = Device.Open(devId);
					Seq[devId]++;
					break;
				case ESeq.checkDevice:
					Seq[devId]++;
#if false
					device[devId].Dispose();
#else
					//m_skeletalTrackingProvider[devId].Dispose();
#endif
					break;
				case ESeq.modeSelectDevice:
					Seq[devId]++;
					break;
				case ESeq.openDevice:
					{
						deviceIDtext[devId] = m_skeletalTrackingProvider[devId].SerialNum;
						this.GetComponent<SaveDataAdmin>().SaveDataTotal.cKinectSetting[devId].mName = deviceIDtext[devId];
						m_tracker[devId * DeviceCount].GetComponent<TrackerHandler>().DeviceID = deviceIDtext[devId];
						m_tracker[devId * DeviceCount + 1].GetComponent<TrackerHandler>().DeviceID = deviceIDtext[devId];
						var cGameListSettings = this.transform.GetComponent<SaveDataAdmin>().SaveDataTotal.cGameSetting.cGameListSetting;
						for (int num = 0; num < cGameListSettings.Count; num++)
						{
							cGameListSettings[num].mIsActiveKinects[devId].Name = deviceIDtext[devId];
						}
					}
					foreach (var tracker in m_tracker)
					{
						tracker.SetActive(false);
					}
#if false
					var depthMode = this.transform.GetComponent<SaveDataAdmin>().SaveDataTotal.cKinectSetting[devId].mDepthMode;
					m_skeletalTrackingProvider[devId].StartClientThread(devId, depthMode);
#else
					//m_skeletalTrackingProvider[devId].Dispose();
#endif
					m_backgroundDataProvider[devId] = m_skeletalTrackingProvider[devId];
					Seq[devId]++;
					break;
				case ESeq.settingDevice:
#if false
					if(m_skeletalTrackingProvider[devId].device != null)
#endif
					{
						Seq[devId]++;
					}
					break;
				case ESeq.startDevice:
					Seq[devId]++;
					IMU[devId] = Vector3.zero;
					sampling = 0;
					break;
				case ESeq.playDevice:
					var effectiveDevice = deviceIDtext[devId];
					if (devId != (int)this.transform.GetComponent<CntrSaveDataKinect>().kinectDevice)
					{
						break;
					}
					if (m_backgroundDataProvider[devId].IsRunning)
					{
						if (m_backgroundDataProvider[devId].GetCurrentFrameData(ref m_lastFrameData[devId]))
						{///フレームデータが拾えた
						 ///キネクトデバイス本体のIMU
							if (sampling < 1000000)
							{
								sampling++;
								IMU[devId] += m_lastFrameData[devId].IMU;
								IMU[devId].x = 0f;///ロールを無視
								//IMU[devId] = m_lastFrameData[devId].IMU;
								var axis = Quaternion.FromToRotation(Vector3.down, IMU[devId].normalized);
								///Kinectの姿勢
								AxisZero.Find("Axis").rotation = axis;
							}
							///Skeleton表示をリセット
							foreach (var tracker in m_tracker)
							{
								tracker.SetActive(false);
							}
							var numOfBodies = (int)m_lastFrameData[devId].NumOfBodies;
							if (numOfBodies != 0)
							{///該当デバイスがBodyを検出
								for (int num = 0; num < numOfBodies; num++)
								{///キネクトのSkeleton情報をUnityに転写
									m_tracker[num].GetComponent<TrackerHandler>().IMU = IMU[devId];
									m_tracker[num].SetActive(true);
									m_tracker[num].GetComponent<TrackerHandler>().updateTracker(m_lastFrameData[devId], num);
								}

							}
						}
					}
					break;
				case ESeq.closeDevice:
#if false
					m_backgroundDataProvider[devId].StopClientThread();
#else
					m_backgroundDataProvider[devId].Dispose();
#endif
					device[devId].Dispose();
					Seq[devId] = ESeq.idleDevice;
					break;
				case ESeq.idleDevice:
					break;
			}
		}
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Escape)) SeqOnKinectQuit++;

		switch (SeqOnKinectQuit)
		{
			case 1:
				//CallOnKinectQuit();
				SeqOnKinectQuit++;
				break;
			case 2:
				//CallOnApplicationQuit();
				break;

		}
	}

#if true
	public void CallOnKinectQuit()
	{
		///Skeleton表示をリセット
		kinectKill.SetActive(false);
		foreach (var tracker in m_tracker)
		{
			tracker.SetActive(false);
		}
		for (int devId = 0; devId < DeviceCount; devId++)
		{
			if (m_skeletalTrackingProvider[devId] != null)
			{
#if false
				m_skeletalTrackingProvider[devId].StopClientThread();
#else
				m_skeletalTrackingProvider[devId].Dispose();
#endif
				if (device[devId] != null)
				{
					device[devId].StopCameras();
					device[devId].Dispose();
				}

				// Stop background threads.
				if (m_backgroundDataProvider[devId] != null)
				{
#if false
					m_backgroundDataProvider[devId].StopClientThread();
#else
					m_backgroundDataProvider[devId].Dispose();
#endif
				}

			}
		}
	}
	public void CallOnApplicationQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
#if WINDOWS_UWP
		Windows.ApplicationModel.Core.CoreApplication.Exit();
#else
		Application.Quit();
#endif
#endif
	}
#endif
	public void SetFloorHeight(int deviceId)
	{
		this.m_tracker[0].GetComponent<TrackerHandler>().FlagSetHeight = true;
	}
	*/
}