using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE6 RID: 3558
	public class JitterHandler
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004F9B RID: 20379 RVA: 0x00295A38 File Offset: 0x00293E38
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x00295A54 File Offset: 0x00293E54
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

		// Token: 0x06004F9D RID: 20381 RVA: 0x00295AC5 File Offset: 0x00293EC5
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x00295AEC File Offset: 0x00293EEC
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00295B14 File Offset: 0x00293F14
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x040034C4 RID: 13508
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x040034C5 RID: 13509
		private float DamageJitterDistance = 0.17f;

		// Token: 0x040034C6 RID: 13510
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x040034C7 RID: 13511
		private float JitterDropPerTick = 0.018f;

		// Token: 0x040034C8 RID: 13512
		private float JitterMax = 0.35f;
	}
}
