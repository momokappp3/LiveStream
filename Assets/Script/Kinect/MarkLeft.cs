using System.Collections;
using UnityEngine;

namespace Assets.Scripts._System._Kinect.Skeleton.Stamps
{
	public class MarkLeft : MonoBehaviour
	{
		public bool IsLeft = false;

		Vector3[] Velocity = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
		Vector3 oldPosition = Vector3.zero;
		Vector3 newPosition = Vector3.zero;
		public Vector3 effectiveVelocity { get; private set; }
		int counter = 0;

		private void Start()
		{
			oldPosition = newPosition = this.transform.parent.parent.parent.position;
			counter = 0;
		}
		private void FixedUpdate()
		{
			counter++;
			if (counter < 4) return;
			oldPosition = newPosition;
			newPosition = this.transform.parent.parent.parent.position;
			Velocity[counter & 3] = (newPosition - oldPosition) / Time.deltaTime;
			Vector3 ave = Vector3.zero;
			{
				foreach (var v in Velocity)
				{
					ave += v;
				}
				ave /= Velocity.Length;
			}
			///‚µ‚«‚¢’l(Žž‘¬100km/s’´‚¦‚é‘¬“x‚Í–³Œø)
			effectiveVelocity = (Velocity[(counter + 3) & 3].sqrMagnitude > ave.sqrMagnitude * 1.5f) ? ave : Velocity[(counter + 3) & 3];
		}
	}

}