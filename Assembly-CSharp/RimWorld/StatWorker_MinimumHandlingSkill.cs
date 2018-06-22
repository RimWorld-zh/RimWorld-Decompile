using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C5 RID: 2501
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x06003818 RID: 14360 RVA: 0x001DE820 File Offset: 0x001DCC20
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x001DE83C File Offset: 0x001DCC3C
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

		// Token: 0x0600381A RID: 14362 RVA: 0x001DE8B0 File Offset: 0x001DCCB0
		private float ValueFromReq(StatRequest req)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return Mathf.Clamp(GenMath.LerpDouble(0.3f, 1f, 0f, 9f, wildness), 0f, 20f);
		}
	}
}
