using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.BodyTracking;
using System.Threading.Tasks;

public class TrackerHandler : MonoBehaviour
{
    [SerializeField] public string DeviceID;
    [SerializeField] public Vector3 IMU;
    [SerializeField] public float Height;
    [SerializeField] public float RoomScale = 1f;
    //[SerializeField] public Vector3 SavedataGlobalPositionOffset;
    //[SerializeField] bool setBoneDisplay=false;
    [SerializeField] public int closestBody = 0;
    [SerializeField] Transform root;
    [SerializeField] public Text between;
    [SerializeField] public Text foot;
    [SerializeField] public CntrSaveDataKinect savedataKinect;
    public Dictionary<JointId, JointId> parentJointMap;
    Dictionary<JointId, Quaternion> basisJointMap;
    [System.Serializable]
    public class CAbsoluteJoint
    {
        public string Name;
        public Transform Node;
        public Quaternion absoluteJointRotations;
        public JointConfidenceLevel precition;
    }
    public CAbsoluteJoint[] cAbsoluteJoint = new CAbsoluteJoint[(int)JointId.Count];
    public bool FlagSetHeight = false;

    ///--------------------------------
    int sampling = 0;
    float heightSum = 0f;
    int skeletonNum = 0;


    //public Quaternion[] absoluteJointRotations = new Quaternion[(int)JointId.Count];
    //public bool drawSkeletons = true;
    Quaternion Y_180_FLIP = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Awake()
    {
        parentJointMap = new Dictionary<JointId, JointId>();

        // pelvis has no parent so set to count
        parentJointMap[JointId.Pelvis] = JointId.Pelvis;
        parentJointMap[JointId.SpineNavel] = JointId.Pelvis;
        parentJointMap[JointId.SpineChest] = JointId.SpineNavel;
        parentJointMap[JointId.Neck] = JointId.SpineChest;
        parentJointMap[JointId.ClavicleLeft] = JointId.SpineChest;
        parentJointMap[JointId.ShoulderLeft] = JointId.ClavicleLeft;
        parentJointMap[JointId.ElbowLeft] = JointId.ShoulderLeft;
        parentJointMap[JointId.WristLeft] = JointId.ElbowLeft;
        parentJointMap[JointId.HandLeft] = JointId.WristLeft;
        parentJointMap[JointId.HandTipLeft] = JointId.HandLeft;
        parentJointMap[JointId.ThumbLeft] = JointId.WristLeft;
        parentJointMap[JointId.ClavicleRight] = JointId.SpineChest;
        parentJointMap[JointId.ShoulderRight] = JointId.ClavicleRight;
        parentJointMap[JointId.ElbowRight] = JointId.ShoulderRight;
        parentJointMap[JointId.WristRight] = JointId.ElbowRight;
        parentJointMap[JointId.HandRight] = JointId.WristRight;
        parentJointMap[JointId.HandTipRight] = JointId.HandRight;
        parentJointMap[JointId.ThumbRight] = JointId.WristRight;
        parentJointMap[JointId.HipLeft] = JointId.Pelvis;
        parentJointMap[JointId.KneeLeft] = JointId.HipLeft;
        parentJointMap[JointId.AnkleLeft] = JointId.KneeLeft;
        parentJointMap[JointId.FootLeft] = JointId.AnkleLeft;
        parentJointMap[JointId.HipRight] = JointId.Pelvis;
        parentJointMap[JointId.KneeRight] = JointId.HipRight;
        parentJointMap[JointId.AnkleRight] = JointId.KneeRight;
        parentJointMap[JointId.FootRight] = JointId.AnkleRight;
        parentJointMap[JointId.Head] = JointId.Neck;
        parentJointMap[JointId.Nose] = JointId.Head;
        parentJointMap[JointId.EyeLeft] = JointId.Head;
        parentJointMap[JointId.EarLeft] = JointId.Head;
        parentJointMap[JointId.EyeRight] = JointId.Head;
        parentJointMap[JointId.EarRight] = JointId.Head;

        Vector3 zpositive = Vector3.forward;
        Vector3 xpositive = Vector3.right;
        Vector3 ypositive = Vector3.up;
        // spine and left hip are the same
        Quaternion leftHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
        Quaternion spineHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
        Quaternion rightHipBasis = Quaternion.LookRotation(xpositive, zpositive);
        // arms and thumbs share the same basis
        Quaternion leftArmBasis = Quaternion.LookRotation(ypositive, -zpositive);
        Quaternion rightArmBasis = Quaternion.LookRotation(-ypositive, zpositive);
        Quaternion leftHandBasis = Quaternion.LookRotation(-zpositive, -ypositive);
        Quaternion rightHandBasis = Quaternion.identity;
        Quaternion leftFootBasis = Quaternion.LookRotation(xpositive, ypositive);
        Quaternion rightFootBasis = Quaternion.LookRotation(xpositive, -ypositive);

        basisJointMap = new Dictionary<JointId, Quaternion>();

        // pelvis has no parent so set to count
        basisJointMap[JointId.Pelvis] = spineHipBasis;
        basisJointMap[JointId.SpineNavel] = spineHipBasis;
        basisJointMap[JointId.SpineChest] = spineHipBasis;
        basisJointMap[JointId.Neck] = spineHipBasis;
        basisJointMap[JointId.ClavicleLeft] = leftArmBasis;
        basisJointMap[JointId.ShoulderLeft] = leftArmBasis;
        basisJointMap[JointId.ElbowLeft] = leftArmBasis;
        basisJointMap[JointId.WristLeft] = leftHandBasis;
        basisJointMap[JointId.HandLeft] = leftHandBasis;
        basisJointMap[JointId.HandTipLeft] = leftHandBasis;
        basisJointMap[JointId.ThumbLeft] = leftArmBasis;
        basisJointMap[JointId.ClavicleRight] = rightArmBasis;
        basisJointMap[JointId.ShoulderRight] = rightArmBasis;
        basisJointMap[JointId.ElbowRight] = rightArmBasis;
        basisJointMap[JointId.WristRight] = rightHandBasis;
        basisJointMap[JointId.HandRight] = rightHandBasis;
        basisJointMap[JointId.HandTipRight] = rightHandBasis;
        basisJointMap[JointId.ThumbRight] = rightArmBasis;
        basisJointMap[JointId.HipLeft] = leftHipBasis;
        basisJointMap[JointId.KneeLeft] = leftHipBasis;
        basisJointMap[JointId.AnkleLeft] = leftHipBasis;
        basisJointMap[JointId.FootLeft] = leftFootBasis;
        basisJointMap[JointId.HipRight] = rightHipBasis;
        basisJointMap[JointId.KneeRight] = rightHipBasis;
        basisJointMap[JointId.AnkleRight] = rightHipBasis;
        basisJointMap[JointId.FootRight] = rightFootBasis;
        basisJointMap[JointId.Head] = spineHipBasis;
        basisJointMap[JointId.Nose] = spineHipBasis;
        basisJointMap[JointId.EyeLeft] = spineHipBasis;
        basisJointMap[JointId.EarLeft] = spineHipBasis;
        basisJointMap[JointId.EyeRight] = spineHipBasis;
        basisJointMap[JointId.EarRight] = spineHipBasis;

        for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
        {
            cAbsoluteJoint[jointNum].Name = ((JointId)jointNum).ToString();
        }

        FlagSetHeight = false;
    }

    public void updateTracker(BackgroundData _trackerFrameData, int _skeletonNumber)
    {
        //this is an array in case you want to get the n closest bodies
        //int closestBody = findClosestTrackedBody(trackerFrameData);

        // render the closest body
        skeletonNum = _skeletonNumber;
        Body skeleton = _trackerFrameData.Bodies[_skeletonNumber];
        renderSkeleton(skeleton);
    }

    int findIndexFromId(BackgroundData frameData, int id)
    {
        int retIndex = -1;
        for (int i = 0; i < (int)frameData.NumOfBodies; i++)
        {
            if ((int)frameData.Bodies[i].Id == id)
            {
                retIndex = i;
                break;
            }
        }
        return retIndex;
    }

    private int findClosestTrackedBody(BackgroundData trackerFrameData)
    {
        //int closestBody = -1;
        const float MAX_DISTANCE = 5000.0f;
        float minDistanceFromKinect = MAX_DISTANCE;
        //for (int i = 0; 0 < (int)trackerFrameData.NumOfBodies; i++)
        {
            var pelvisPosition = trackerFrameData.Bodies[closestBody].JointPositions3D[(int)JointId.Pelvis];
            Vector3 pelvisPos = new Vector3((float)pelvisPosition.X, (float)pelvisPosition.Y, (float)pelvisPosition.Z);
            if (pelvisPos.magnitude < minDistanceFromKinect)
            {
                //closestBody = 0;
                minDistanceFromKinect = pelvisPos.magnitude;
            }
        }
        return closestBody;
    }

    public void renderSkeleton(Body skeleton)
    {
        ///Kinectの姿勢
        var imu = new Vector3(IMU.x, IMU.y, IMU.z);
        var axis = Quaternion.FromToRotation(Vector3.down, imu.normalized);
        //this.transform.Find("Axis").localRotation = axis;
        for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
        {
            Vector3 jointLocationFromPervis
                = new Vector3(
                  skeleton.JointPositions3D[jointNum].X,
                  skeleton.JointPositions3D[jointNum].Y,
                -(skeleton.JointPositions3D[jointNum].Z)
                );
            Quaternion jointRot = Y_180_FLIP * new Quaternion(
                -skeleton.JointRotations[jointNum].X, skeleton.JointRotations[jointNum].Y,
                skeleton.JointRotations[jointNum].Z, -skeleton.JointRotations[jointNum].W)
                * Quaternion.Inverse(basisJointMap[(JointId)jointNum]);
            cAbsoluteJoint[jointNum].precition = skeleton.JointPrecisions[jointNum];
            //cAbsoluteJoint[jointNum].absoluteJointRotations = Quaternion.Inverse(axis) * jointRot;
            cAbsoluteJoint[jointNum].Node.localPosition = Quaternion.Inverse(axis) * jointLocationFromPervis;
            cAbsoluteJoint[jointNum].Node.localRotation = Quaternion.Inverse(axis) * jointRot;
            ///骨の長さ（スケール）合わせ
            var parentNode = cAbsoluteJoint[(int)parentJointMap[(JointId)jointNum]].Node;
            var ownNode = cAbsoluteJoint[jointNum].Node;
            var magnitude = (ownNode.position - parentNode.position).magnitude;
            var parentLocater = parentNode.Find("Locater");
            parentLocater.localScale
                = new Vector3(1f, magnitude / 2f, 1f);
            parentLocater.Find("Bone").localScale
                = new Vector3(0.02f, 1f, 0.02f);
            parentLocater.Find("Bone").localPosition
                = new Vector3(0f, 1f, 0f);

        }
        var menu = savedataKinect.cSettingKinect[(int)savedataKinect.kinectDevice].Menu.transform;
        if (menu != null && skeletonNum == 0)
        {
            ///現状の高さ調整
            if (FlagSetHeight)
            {
                if (sampling < 60)
                {
                    var precitionL = skeleton.JointPrecisions[(int)JointId.FootLeft];
                    var precitionR = skeleton.JointPrecisions[(int)JointId.FootRight];
                    if (precitionL >= JointConfidenceLevel.Medium
                        && precitionR >= JointConfidenceLevel.Medium)
                    {///信頼度の高いものだけ有効
                        sampling++;
                        Vector3 floor = new Vector3(0f, 0f, 0f);
                        floor += cAbsoluteJoint[(int)JointId.FootLeft].Node.position;
                        floor += cAbsoluteJoint[(int)JointId.FootRight].Node.position;
                        //var oldFloor = this.transform.Find("PositionBody").position;
                        var oldFloor = this.transform.parent.position;
                        heightSum += oldFloor.y - floor.y / 2f;
                        Height = heightSum / sampling;
                    }
                }
                else
                {
                    FlagSetHeight = false;
                    {
                        //var oldFloor = this.transform.Find("PositionBody").position;
                        var oldFloor = this.transform.parent.position;
                        //this.transform.Find("PositionBody").position = new Vector3(oldFloor.x, Height, oldFloor.z);
                        this.transform.parent.position = new Vector3(oldFloor.x, Height, oldFloor.z);
                    }
                    heightSum = 0f;
                    sampling = 0;
                }
                menu.Find("Height").Find("Foot").GetComponent<Text>().text = Height.ToString();
            }
            menu.Find("Wrist").Find("Between").GetComponent<Text>().text =
                (cAbsoluteJoint[(int)JointId.WristLeft].Node.position
                - cAbsoluteJoint[(int)JointId.WristRight].Node.position).magnitude + "m";
        }
    }

    public Quaternion GetRelativeJointRotation(JointId jointId)
    {
        JointId parent = parentJointMap[jointId];
        Quaternion parentJointRotationBodySpace = Quaternion.identity;
        if (parent == JointId.Count)
        {
            //parentJointRotationBodySpace = Y_180_FLIP;
        }
        else
        {
            parentJointRotationBodySpace = cAbsoluteJoint[(int)parent].absoluteJointRotations;
        }
        Quaternion jointRotationBodySpace = cAbsoluteJoint[(int)jointId].absoluteJointRotations;
        Quaternion relativeRotation = Quaternion.Inverse(parentJointRotationBodySpace) * jointRotationBodySpace;

        return relativeRotation;
    }


}
