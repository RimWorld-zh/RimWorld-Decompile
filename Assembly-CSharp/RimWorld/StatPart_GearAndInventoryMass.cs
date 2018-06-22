using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A8 RID: 2472
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x0600376F RID: 14191 RVA: 0x001D9738 File Offset: 0x001D7B38
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x001D975C File Offset: 0x001D7B5C
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

		// Token: 0x06003771 RID: 14193 RVA: 0x001D97A0 File Offset: 0x001D7BA0
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
