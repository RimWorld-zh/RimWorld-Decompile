using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF1 RID: 2801
	public class CameraShaker
	{
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0020AFE8 File Offset: 0x002093E8
		// (set) Token: 0x06003DFD RID: 15869 RVA: 0x0020B003 File Offset: 0x00209403
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
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0020B01C File Offset: 0x0020941C
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

		// Token: 0x06003DFF RID: 15871 RVA: 0x0020B08D File Offset: 0x0020948D
		public void DoShake(float mag)
		{
			if (mag > 0f)
			{
				this.CurShakeMag += mag;
			}
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0020B0AE File Offset: 0x002094AE
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0020B0C3 File Offset: 0x002094C3
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
