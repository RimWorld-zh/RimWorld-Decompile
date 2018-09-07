using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class StatPart_GearAndInventoryMass : StatPart
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache0;

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
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		private bool TryGetValue(StatRequest req, out float value)
		{
			if (StatPart_GearAndInventoryMass.<>f__mg$cache0 == null)
			{
				StatPart_GearAndInventoryMass.<>f__mg$cache0 = new Func<Pawn, float>(MassUtility.GearAndInventoryMass);
			}
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, StatPart_GearAndInventoryMass.<>f__mg$cache0, (ThingDef x) => 0f, out value);
		}

		[CompilerGenerated]
		private static float <TryGetValue>m__0(ThingDef x)
		{
			return 0f;
		}
	}
}
