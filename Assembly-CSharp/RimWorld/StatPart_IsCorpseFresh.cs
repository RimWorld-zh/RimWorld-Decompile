using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AD RID: 2477
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x06003784 RID: 14212 RVA: 0x001D9FB0 File Offset: 0x001D83B0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x001D9FD4 File Offset: 0x001D83D4
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

		// Token: 0x06003786 RID: 14214 RVA: 0x001DA024 File Offset: 0x001D8424
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
