using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF0 RID: 2800
	public class CameraShaker
	{
		// Token: 0x0400273A RID: 10042
		private float curShakeMag = 0f;

		// Token: 0x0400273B RID: 10043
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x0400273C RID: 10044
		private const float ShakeFrequency = 24f;

		// Token: 0x0400273D RID: 10045
		private const float MaxShakeMag = 0.2f;

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0020B718 File Offset: 0x00209B18
		// (set) Token: 0x06003DFC RID: 15868 RVA: 0x0020B733 File Offset: 0x00209B33
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

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x0020B74C File Offset: 0x00209B4C
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

		// Token: 0x06003DFE RID: 15870 RVA: 0x0020B7BD File Offset: 0x00209BBD
		public void DoShake(float mag)
		{
			if (mag > 0f)
			{
				this.CurShakeMag += mag;
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0020B7DE File Offset: 0x00209BDE
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0020B7F3 File Offset: 0x00209BF3
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}
	}
}
