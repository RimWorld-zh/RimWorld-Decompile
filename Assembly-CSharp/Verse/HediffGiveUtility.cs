using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class HediffGiveUtility
	{
		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache1;

		public static bool TryApply(Pawn pawn, HediffDef hediff, List<BodyPartDef> partsToAffect, bool canAffectAnyLivePart = false, int countToAffect = 1, List<Hediff> outAddedHediffs = null)
		{
			bool result;
			if (canAffectAnyLivePart || partsToAffect != null)
			{
				bool flag = false;
				for (int i = 0; i < countToAffect; i++)
				{
					IEnumerable<BodyPartRecord> source = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null);
					if (partsToAffect != null)
					{
						source = from p in source
						where partsToAffect.Contains(p.def)
						select p;
					}
					if (canAffectAnyLivePart)
					{
						source = from p in source
						where p.def.alive
						select p;
					}
					source = from p in source
					where !pawn.health.hediffSet.HasHediff(hediff, p, false) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)
					select p;
					if (!source.Any<BodyPartRecord>())
					{
						break;
					}
					BodyPartRecord partRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					Hediff hediff2 = HediffMaker.MakeHediff(hediff, pawn, partRecord);
					pawn.health.AddHediff(hediff2, null, null, null);
					if (outAddedHediffs != null)
					{
						outAddedHediffs.Add(hediff2);
					}
					flag = true;
				}
				result = flag;
			}
			else if (!pawn.health.hediffSet.HasHediff(hediff, false))
			{
				Hediff hediff3 = HediffMaker.MakeHediff(hediff, pawn, null);
				pawn.health.AddHediff(hediff3, null, null, null);
				if (outAddedHediffs != null)
				{
					outAddedHediffs.Add(hediff3);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <TryApply>m__0(BodyPartRecord p)
		{
			return p.def.alive;
		}

		[CompilerGenerated]
		private static float <TryApply>m__1(BodyPartRecord x)
		{
			return x.coverageAbs;
		}

		[CompilerGenerated]
		private sealed class <TryApply>c__AnonStorey0
		{
			internal List<BodyPartDef> partsToAffect;

			internal Pawn pawn;

			internal HediffDef hediff;

			public <TryApply>c__AnonStorey0()
			{
			}

			internal bool <>m__0(BodyPartRecord p)
			{
				return this.partsToAffect.Contains(p.def);
			}

			internal bool <>m__1(BodyPartRecord p)
			{
				return !this.pawn.health.hediffSet.HasHediff(this.hediff, p, false) && !this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p);
			}
		}
	}
}
