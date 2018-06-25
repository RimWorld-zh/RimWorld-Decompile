using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C7 RID: 2503
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x0600381C RID: 14364 RVA: 0x001DEC38 File Offset: 0x001DD038
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x001DEC54 File Offset: 0x001DD054
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return string.Concat(new string[]
			{
				"Wildness".Translate(),
				" ",
				wildness.ToStringPercent(),
				": ",
				this.ValueFromReq(req).ToString("F0")
			});
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x001DECC8 File Offset: 0x001DD0C8
		private float ValueFromReq(StatRequest req)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return Mathf.Clamp(GenMath.LerpDouble(0.3f, 1f, 0f, 9f, wildness), 0f, 20f);
		}
	}
}
