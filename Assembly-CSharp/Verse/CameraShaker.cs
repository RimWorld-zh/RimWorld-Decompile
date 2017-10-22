using UnityEngine;

namespace Verse
{
	public class CameraShaker
	{
		private float curShakeMag = 0f;

		private const float ShakeDecayRate = 0.5f;

		private const float ShakeFrequency = 24f;

		private const float MaxShakeMag = 0.2f;

		public float CurShakeMag
		{
			get
			{
				return this.curShakeMag;
			}
			set
			{
				this.curShakeMag = Mathf.Clamp(value, 0f, 0.2f);
			}
		}

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
			if (!(mag <= 0.0))
			{
				this.CurShakeMag += mag;
			}
		}

		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
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
