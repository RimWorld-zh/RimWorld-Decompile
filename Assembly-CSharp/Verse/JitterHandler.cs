using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE6 RID: 3558
	public class JitterHandler
	{
		// Token: 0x040034D6 RID: 13526
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x040034D7 RID: 13527
		private float DamageJitterDistance = 0.17f;

		// Token: 0x040034D8 RID: 13528
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x040034D9 RID: 13529
		private float JitterDropPerTick = 0.018f;

		// Token: 0x040034DA RID: 13530
		private float JitterMax = 0.35f;

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x00297420 File Offset: 0x00295820
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x0029743C File Offset: 0x0029583C
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

		// Token: 0x06004FB6 RID: 20406 RVA: 0x002974AD File Offset: 0x002958AD
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x002974D4 File Offset: 0x002958D4
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x002974FC File Offset: 0x002958FC
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}
	}
}
