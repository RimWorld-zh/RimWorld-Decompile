using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AC RID: 2476
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x06003776 RID: 14198 RVA: 0x001D9574 File Offset: 0x001D7974
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x001D9598 File Offset: 0x001D7998
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			string result;
			if (this.TryGetValue(req, out mass))
			{
				result = "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x001D95DC File Offset: 0x001D79DC
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
