using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_DiseaseAnimal : IncidentWorker_Disease
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, ThingDef> <>f__am$cache2;

		public IncidentWorker_DiseaseAnimal()
		{
		}

		protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map != null)
			{
				return from p in map.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.HostFaction == null && !p.RaceProps.Humanlike
				select p;
			}
			return from p in ((Caravan)target).PawnsListForReading
			where !p.RaceProps.Humanlike
			select p;
		}

		protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
		{
			Pawn[] potentialVictims = base.PotentialVictims(parms.target).ToArray<Pawn>();
			IEnumerable<ThingDef> source = (from v in potentialVictims
			select v.def).Distinct<ThingDef>();
			ThingDef targetRace = source.RandomElementByWeightWithFallback((ThingDef race) => (from v in potentialVictims
			where v.def == race
			select v.BodySize).Sum(), null);
			IEnumerable<Pawn> source2 = from v in potentialVictims
			where v.def == targetRace
			select v;
			int num = source2.Count<Pawn>();
			IntRange intRange = new IntRange(Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.max));
			int num2 = intRange.RandomInRange;
			num2 = Mathf.Clamp(num2, 1, this.def.diseaseMaxVictims);
			return source2.InRandomOrder(null).Take(num2);
		}

		[CompilerGenerated]
		private static bool <PotentialVictimCandidates>m__0(Pawn p)
		{
			return p.HostFaction == null && !p.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <PotentialVictimCandidates>m__1(Pawn p)
		{
			return !p.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static ThingDef <ActualVictims>m__2(Pawn v)
		{
			return v.def;
		}

		[CompilerGenerated]
		private sealed class <ActualVictims>c__AnonStorey0
		{
			internal Pawn[] potentialVictims;

			internal ThingDef targetRace;

			private static Func<Pawn, float> <>f__am$cache0;

			public <ActualVictims>c__AnonStorey0()
			{
			}

			internal float <>m__0(ThingDef race)
			{
				return (from v in this.potentialVictims
				where v.def == race
				select v.BodySize).Sum();
			}

			internal bool <>m__1(Pawn v)
			{
				return v.def == this.targetRace;
			}

			private static float <>m__2(Pawn v)
			{
				return v.BodySize;
			}

			private sealed class <ActualVictims>c__AnonStorey1
			{
				internal ThingDef race;

				internal IncidentWorker_DiseaseAnimal.<ActualVictims>c__AnonStorey0 <>f__ref$0;

				public <ActualVictims>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Pawn v)
				{
					return v.def == this.race;
				}
			}
		}
	}
}
