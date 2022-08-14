using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.BodyTracking;

namespace Assets.Scripts._System._Kinect.Skeleton.Stamps
{
    class CntrStampHand : MonoBehaviour
    {
        /// ------------------------------------
        [SerializeField] TrackerHandler trackerHandler;
        [Header("HandStampPrefab")]
        [SerializeField] GameObject handStampPrefab;
        [SerializeField] public Material[] Colors;
        /// ------------------------------------
		JointId[][] stampKeys =
        {
            new JointId[]{ JointId.WristLeft, JointId.HandLeft },
            new JointId[]{ JointId.WristRight, JointId.HandRight },
        };
        public Dictionary<JointId, GameObject> handStamps = new Dictionary<JointId, GameObject>();
        public Vector3[] HandSpeed { get; private set; }
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

        public void ChangeColor(EColors _color)
        {
            foreach (var hand in stampKeys)
            {
                handStamps[hand[0]].transform.Find("Locator").Find("Back").Find("Image").GetComponent<MeshRenderer>().material = Colors[(int)_color];
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            handStamps.Add(JointId.WristLeft, Instantiate<GameObject>(handStampPrefab, this.transform));
            handStamps.Add(JointId.WristRight, Instantiate<GameObject>(handStampPrefab, this.transform));
            ///‰E‘«”½“]
            handStamps[JointId.WristRight].transform.Find("Locator").Find("Back").Find("Image").Rotate(0f, 180f, 0f);
            handStamps[JointId.WristLeft].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<Stamps.MarkLeft>().IsLeft = true;
            handStamps[JointId.WristRight].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<Stamps.MarkLeft>().IsLeft = false;
            ChangeColor(EColors.Black);
            HandSpeed = new Vector3[2];
            HandSpeed[0] = HandSpeed[1] = Vector3.zero;
        }
        private void LateUpdate()
        {
            /// ------------------------------------
            foreach (var hand in stampKeys)
            {
                //null
                handStamps[hand[0]].transform.position = trackerHandler.cAbsoluteJoint[(int)hand[0]].Node.position;
                var vecAncle = new Vector3(
                    trackerHandler.cAbsoluteJoint[(int)hand[1]].Node.position.x - trackerHandler.cAbsoluteJoint[(int)hand[0]].Node.position.x,
                    0f,
                    trackerHandler.cAbsoluteJoint[(int)hand[1]].Node.position.z - trackerHandler.cAbsoluteJoint[(int)hand[0]].Node.position.z
                );
                //Debug.DrawLine(handStamps[hand[0]].transform.position, handStamps[hand[0]].transform.position + vecAncle * 10f, Color.red);
                var xyPlane = trackerHandler.cAbsoluteJoint[(int)hand[0]].Node.position - trackerHandler.cAbsoluteJoint[(int)hand[1]].Node.position;
                xyPlane = new Vector3(xyPlane.x, xyPlane.y, xyPlane.z * 0f);
                handStamps[hand[0]].transform.Find("Locator").rotation = Quaternion.FromToRotation(Vector3.down, xyPlane);
            }
            HandSpeed[0] = handStamps[JointId.WristLeft].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<Stamps.MarkLeft>().effectiveVelocity;
            HandSpeed[1] = handStamps[JointId.WristRight].transform.Find("Locator").Find("Back").Find("Touch").GetComponent<Stamps.MarkLeft>().effectiveVelocity;
            /// ------------------------------------

        }

        private void OnDestroy()
        {
            foreach (var hand in stampKeys)
            {
                if (handStamps[hand[0]] != null)
                    Destroy(handStamps[hand[0]]);
            }
            handStamps.Clear();
        }
    }
}
