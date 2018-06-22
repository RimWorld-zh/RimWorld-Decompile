using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AB RID: 2475
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x001D9B9C File Offset: 0x001D7F9C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x001D9BC0 File Offset: 0x001D7FC0
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

		// Token: 0x06003782 RID: 14210 RVA: 0x001D9C10 File Offset: 0x001D8010
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
