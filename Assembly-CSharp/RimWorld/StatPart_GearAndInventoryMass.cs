using System;
using Verse;

namespace RimWorld
{
	public class StatPart_GearAndInventoryMass : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = default(float);
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float mass = default(float);
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return (string)null;
		}

		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Func<Pawn, float>)((Pawn x) => MassUtility.GearAndInventoryMass(x)), (Func<ThingDef, float>)((ThingDef x) => 0f), out value);
		}
	}
}
