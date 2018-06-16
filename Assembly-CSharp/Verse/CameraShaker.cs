using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF1 RID: 2801
	public class CameraShaker
	{
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x0020AF14 File Offset: 0x00209314
		// (set) Token: 0x06003DFB RID: 15867 RVA: 0x0020AF2F File Offset: 0x0020932F
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

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0020AF48 File Offset: 0x00209348
		public Vector3 ShakeOffset
		{
			get
			{
				float x = Mathf.Sin(Time.realtimeSinceStartup * 24f) * this.curShakeMag;
				float y = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.05f) * this.curShakeMag;
				float z = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.1f) * this.curShakeMag;
				return new Vector3(x, y, z);
			}
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x0020AFB9 File Offset: 0x002093B9
		public void DoShake(float mag)
		{
			if (mag > 0f)
			{
				this.CurShakeMag += mag;
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0020AFDA File Offset: 0x002093DA
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0020AFEF File Offset: 0x002093EF
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x04002737 RID: 10039
		private float curShakeMag = 0f;

		// Token: 0x04002738 RID: 10040
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x04002739 RID: 10041
		private const float ShakeFrequency = 24f;

		// Token: 0x0400273A RID: 10042
		private const float MaxShakeMag = 0.2f;
	}
}
