using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.BodyTracking;

namespace Assets.Scripts._System._Kinect.Skeleton.Stamps

{
    class CntrStampPelvis : MonoBehaviour
    {
        /// ------------------------------------
        [SerializeField] TrackerHandler trackerHandler;
        [Header("PelvisStampPrefab")]
        [SerializeField] GameObject pelvisStampPrefab;
        [SerializeField] public Material[] Colors;
        /// ------------------------------------
        public GameObject pelvisStamp { get; private set; }
        public enum EColors
        {
            Black,
            Red,
            Blue,
        }

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
            pelvisStamp.transform.Find("Locator").Find("Back").Find("Image").GetComponent<MeshRenderer>().material = Colors[(int)_color];
        }

        // Start is called before the first frame update
        void Start()
        {
            pelvisStamp = Instantiate<GameObject>(pelvisStampPrefab, this.transform);
        }
        private void LateUpdate()
        {
            /// ------------------------------------
            pelvisStamp.transform.position = trackerHandler.cAbsoluteJoint[(int)JointId.Pelvis].Node.position;
        }

        private void OnDestroy()
        {
            if (pelvisStamp != null)
                Destroy(pelvisStamp);
        }
    }
}
