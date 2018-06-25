using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AEF RID: 2799
	public class CameraShaker
	{
		// Token: 0x04002733 RID: 10035
		private float curShakeMag = 0f;

		// Token: 0x04002734 RID: 10036
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x04002735 RID: 10037
		private const float ShakeFrequency = 24f;

		// Token: 0x04002736 RID: 10038
		private const float MaxShakeMag = 0.2f;

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0020B438 File Offset: 0x00209838
		// (set) Token: 0x06003DFC RID: 15868 RVA: 0x0020B453 File Offset: 0x00209853
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
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x0020B46C File Offset: 0x0020986C
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

		// Token: 0x06003DFE RID: 15870 RVA: 0x0020B4DD File Offset: 0x002098DD
		public void DoShake(float mag)
		{
			if (mag > 0f)
			{
				this.CurShakeMag += mag;
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0020B4FE File Offset: 0x002098FE
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0020B513 File Offset: 0x00209913
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
