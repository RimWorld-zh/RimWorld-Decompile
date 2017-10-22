using UnityEngine;

namespace Verse
{
	public class CameraShaker
	{
		private const float ShakeDecayRate = 0.5f;

		private const float ShakeFrequency = 24f;

		private const float MaxShakeMag = 0.1f;

		private float curShakeMag;

		public Vector3 ShakeOffset
		{
			get
			{
				float x = Mathf.Sin((float)(Time.realtimeSinceStartup * 24.0)) * this.curShakeMag;
				float y = Mathf.Sin((float)(Time.realtimeSinceStartup * 24.0 * 1.0499999523162842)) * this.curShakeMag;
				float z = Mathf.Sin((float)(Time.realtimeSinceStartup * 24.0 * 1.1000000238418579)) * this.curShakeMag;
				return new Vector3(x, y, z);
			}
		}

		public void DoShake(float mag)
		{
			this.curShakeMag += mag;
			if (this.curShakeMag > 0.10000000149011612)
			{
				this.curShakeMag = 0.1f;
			}
		}

		public void Update()
		{
			this.curShakeMag -= (float)(0.5 * RealTime.realDeltaTime);
			if (this.curShakeMag < 0.0)
			{
				this.curShakeMag = 0f;
			}
		}
	}
}
