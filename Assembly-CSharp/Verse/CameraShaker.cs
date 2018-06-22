using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AED RID: 2797
	public class CameraShaker
	{
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0020B30C File Offset: 0x0020970C
		// (set) Token: 0x06003DF8 RID: 15864 RVA: 0x0020B327 File Offset: 0x00209727
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
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x0020B340 File Offset: 0x00209740
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

		// Token: 0x06003DFA RID: 15866 RVA: 0x0020B3B1 File Offset: 0x002097B1
		public void DoShake(float mag)
		{
			if (mag > 0f)
			{
				this.CurShakeMag += mag;
			}
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0020B3D2 File Offset: 0x002097D2
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x0020B3E7 File Offset: 0x002097E7
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x04002732 RID: 10034
		private float curShakeMag = 0f;

		// Token: 0x04002733 RID: 10035
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x04002734 RID: 10036
		private const float ShakeFrequency = 24f;

		// Token: 0x04002735 RID: 10037
		private const float MaxShakeMag = 0.2f;
	}
}
