
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.BodyTracking;

namespace Assets.Scripts._System._Kinect.Skeleton.Stamps
{
    class CntrStampFoot : MonoBehaviour
    {
        /// ------------------------------------
        [SerializeField] TrackerHandler trackerHandler;
        [Header("FootStampPrefab")]
        [SerializeField] GameObject footStampPrefab;
        [SerializeField] public Material[] Colors;
        /// ------------------------------------
        JointId[][] stampKeys =
        {
            new JointId[]{ JointId.AnkleLeft, JointId.FootLeft },
            new JointId[]{ JointId.AnkleRight, JointId.FootRight },
        };
        public Dictionary<JointId, GameObject> footStamps = new Dictionary<JointId, GameObject>();
        public Vector3[] FootSpeed { get; private set; }
        public enum EColors
        {
            Black,
            Red,
            Blue,
        }
        /// ------------------------------------

        public void ColorBlack()
        {
            ChangeColor(EColors.Black);
        }
        public void ColorBlue()
        {
            ChangeColor(EColors.Blue);
        }
        public void ColorRed()
        {
            ChangeColor(EColors.Red);
        }

        void ChangeColor(EColors _color)
        {
            foreach (var foot in stampKeys)
            {
                footStamps[foot[0]].transform.Find("Locator").Find("Back").Find("Image").GetComponent<MeshRenderer>().material = Colors[(int)_color];
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            footStamps.Add(JointId.AnkleLeft, Instantiate<GameObject>(footStampPrefab, this.transform));
            footStamps.Add(JointId.AnkleRight, Instantiate<GameObject>(footStampPrefab, this.transform));
            ///‰E‘«”½“]
            footStamps[JointId.AnkleRight].transform.Find("Locator").Find("Back").Find("Image").rotation = Quaternion.Euler(-90f, 90f, 90f);
            footStamps[JointId.AnkleLeft].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<MarkLeft>().IsLeft = true;
            footStamps[JointId.AnkleRight].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<MarkLeft>().IsLeft = false;
            ChangeColor(EColors.Black);
            FootSpeed = new Vector3[2];
            FootSpeed[0] = FootSpeed[1] = Vector3.zero;
        }
        private void LateUpdate()
        {
            /// ------------------------------------
            foreach (var foot in stampKeys)
            {
                var pos = (trackerHandler.cAbsoluteJoint[(int)foot[0]].Node.position + trackerHandler.cAbsoluteJoint[(int)foot[1]].Node.position) / 2f;
                //footStamps[foot[0]].transform.position = new Vector3(pos.x, 0.02f, pos.z);
                footStamps[foot[0]].transform.position = pos;
                //footStamps[foot[0]].transform.position = trackerHandler.cAbsoluteJoint[(int)foot[0]].Node.position;
                var vecAncle = new Vector3(
                    trackerHandler.cAbsoluteJoint[(int)foot[1]].Node.position.x - trackerHandler.cAbsoluteJoint[(int)foot[0]].Node.position.x,
                    0f,
                    trackerHandler.cAbsoluteJoint[(int)foot[1]].Node.position.z - trackerHandler.cAbsoluteJoint[(int)foot[0]].Node.position.z
                );
                //Debug.DrawLine(handStamps[hand[0]].transform.position, handStamps[hand[0]].transform.position + vecAncle * 10f, Color.red);
                var xyPlane = trackerHandler.cAbsoluteJoint[(int)foot[0]].Node.position - trackerHandler.cAbsoluteJoint[(int)foot[1]].Node.position;
                xyPlane = new Vector3(xyPlane.x, xyPlane.y, xyPlane.z * 0f);
                //footStamps[foot[0]].transform.Find("Locator").rotation = Quaternion.FromToRotation(Vector3.down, xyPlane);
                footStamps[foot[0]].transform.Find("Locator").rotation = Quaternion.FromToRotation(Vector3.forward, vecAncle.normalized);
            }
            //FootSpeed[0] = footStamps[JointId.AnkleLeft].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<Kinect.Skeleton.Stamps.MarkLeft>().effectiveVelocity;
            //FootSpeed[1] = footStamps[JointId.AnkleRight].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<Kinect.Skeleton.Stamps.MarkLeft>().effectiveVelocity;
            /// ------------------------------------

        }

        private void OnDestroy()
        {
            foreach (var foot in stampKeys)
            {
                if (footStamps[foot[0]] != null)
                    Destroy(footStamps[foot[0]]);
            }
            footStamps.Clear();
        }
    }
}
