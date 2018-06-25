using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE5 RID: 3557
	public class JitterHandler
	{
		// Token: 0x040034CF RID: 13519
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x040034D0 RID: 13520
		private float DamageJitterDistance = 0.17f;

		// Token: 0x040034D1 RID: 13521
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x040034D2 RID: 13522
		private float JitterDropPerTick = 0.018f;

		// Token: 0x040034D3 RID: 13523
		private float JitterMax = 0.35f;

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x00297140 File Offset: 0x00295540
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x0029715C File Offset: 0x0029555C
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

		// Token: 0x06004FB6 RID: 20406 RVA: 0x002971CD File Offset: 0x002955CD
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x002971F4 File Offset: 0x002955F4
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x0029721C File Offset: 0x0029561C
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
