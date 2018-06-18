using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C9 RID: 2505
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x0600381E RID: 14366 RVA: 0x001DE648 File Offset: 0x001DCA48
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x001DE664 File Offset: 0x001DCA64
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

		// Token: 0x06003820 RID: 14368 RVA: 0x001DE6D8 File Offset: 0x001DCAD8
		private float ValueFromReq(StatRequest req)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return Mathf.Clamp(GenMath.LerpDouble(0.3f, 1f, 0f, 9f, wildness), 0f, 20f);
		}
	}
}
