using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AC RID: 2476
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x06003774 RID: 14196 RVA: 0x001D94A0 File Offset: 0x001D78A0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x001D94C4 File Offset: 0x001D78C4
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

		// Token: 0x06003776 RID: 14198 RVA: 0x001D9508 File Offset: 0x001D7908
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
