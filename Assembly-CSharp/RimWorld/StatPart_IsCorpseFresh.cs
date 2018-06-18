using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AF RID: 2479
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x06003787 RID: 14215 RVA: 0x001D99D8 File Offset: 0x001D7DD8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x001D99FC File Offset: 0x001D7DFC
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			string result;
			if (this.TryGetIsFreshFactor(req, out num) && num != 1f)
			{
				result = "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x001D9A4C File Offset: 0x001D7E4C
		private bool TryGetIsFreshFactor(StatRequest req, out float factor)
		{
			bool result;
			if (!req.HasThing)
			{
				factor = 1f;
				result = false;
			}
			else
			{
				Corpse corpse = req.Thing as Corpse;
				if (corpse == null)
				{
					factor = 1f;
					result = false;
				}
				else
				{
					factor = ((corpse.GetRotStage() != RotStage.Fresh) ? 0f : 1f);
					result = true;
				}
			}
			return result;
		}
	}
}
