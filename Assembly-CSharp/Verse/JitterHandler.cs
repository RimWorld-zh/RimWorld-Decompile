using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE3 RID: 3555
	public class JitterHandler
	{
		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004FB0 RID: 20400 RVA: 0x00297014 File Offset: 0x00295414
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x00297030 File Offset: 0x00295430
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

		// Token: 0x06004FB2 RID: 20402 RVA: 0x002970A1 File Offset: 0x002954A1
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x002970C8 File Offset: 0x002954C8
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x002970F0 File Offset: 0x002954F0
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

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
	}
}
