using UnityEngine;

namespace Verse
{
	public class MoteProperties
	{
		public bool realTime = false;

		public float fadeInTime = 0f;

		public float solidTime = 1f;

		public float fadeOutTime = 0f;

		public Vector3 acceleration = Vector3.zero;

		public float speedPerTime;

		public float growthRate = 0f;

		public bool collide = false;

		public SoundDef landSound;

		public Vector3 attachedDrawOffset;

		public bool needsMaintenance = false;

		public bool rotateTowardsTarget;
	}
}
