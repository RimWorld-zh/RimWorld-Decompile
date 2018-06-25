using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class StatPart_GearAndInventoryMass : StatPart
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		public StatPart_GearAndInventoryMass()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

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

		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}

		[CompilerGenerated]
		private static float <TryGetValue>m__0(Pawn x)
		{
			return MassUtility.GearAndInventoryMass(x);
		}

		[CompilerGenerated]
		private static float <TryGetValue>m__1(ThingDef x)
		{
			return 0f;
		}
	}
}
