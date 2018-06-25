using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x06003773 RID: 14195 RVA: 0x001D9B4C File Offset: 0x001D7F4C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x001D9B70 File Offset: 0x001D7F70
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

		// Token: 0x06003775 RID: 14197 RVA: 0x001D9BB4 File Offset: 0x001D7FB4
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
