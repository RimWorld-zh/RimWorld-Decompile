using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE7 RID: 3559
	public class JitterHandler
	{
		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004F9D RID: 20381 RVA: 0x00295A58 File Offset: 0x00293E58
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x00295A74 File Offset: 0x00293E74
		public void JitterHandlerTick()
		{
			if (this.curOffset.sqrMagnitude < this.JitterDropPerTick * this.JitterDropPerTick)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
			}
			else
			{
				this.curOffset -= this.curOffset.normalized * this.JitterDropPerTick;
			}
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00295AE5 File Offset: 0x00293EE5
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x00295B0C File Offset: 0x00293F0C
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00295B34 File Offset: 0x00293F34
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x040034C6 RID: 13510
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x040034C7 RID: 13511
		private float DamageJitterDistance = 0.17f;

		// Token: 0x040034C8 RID: 13512
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x040034C9 RID: 13513
		private float JitterDropPerTick = 0.018f;

		// Token: 0x040034CA RID: 13514
		private float JitterMax = 0.35f;
	}
}
