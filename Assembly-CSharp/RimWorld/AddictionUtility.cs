using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class AddictionUtility
	{
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, Thing drug)
		{
			Hediff_Addiction result;
			if (!drug.def.IsDrug)
			{
				result = null;
			}
			else
			{
				CompDrug compDrug = drug.TryGetComp<CompDrug>();
				result = (compDrug.Props.Addictive ? AddictionUtility.FindAddictionHediff(pawn, compDrug.Props.chemical) : null);
			}
			return result;
		}

		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Predicate<Hediff>)((Hediff x) => x.def == chemical.addictionHediff));
		}

		public static Hediff FindToleranceHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (chemical.toleranceHediff != null) ? pawn.health.hediffSet.hediffs.Find((Predicate<Hediff>)((Hediff x) => x.def == chemical.toleranceHediff)) : null;
		}

		public static void ModifyChemicalEffectForToleranceAndBodySize(Pawn pawn, ChemicalDef chemicalDef, ref float effect)
		{
			if (chemicalDef != null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					hediffs[i].ModifyChemicalEffect(chemicalDef, ref effect);
				}
			}
			effect /= pawn.BodySize;
		}

		public static void CheckDrugAddictionTeachOpportunity(Pawn pawn)
		{
			if (pawn.RaceProps.IsFlesh && pawn.Spawned && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer) && AddictionUtility.AddictedToAnything(pawn))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugAddiction, pawn, OpportunityType.Important);
			}
		}

		public static bool AddictedToAnything(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < hediffs.Count)
				{
					if (hediffs[num] is Hediff_Addiction)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool CanBingeOnNow(Pawn pawn, ChemicalDef chemical, DrugCategory drugCategory)
		{
			bool result;
			if (!chemical.canBinge)
			{
				result = false;
			}
			else if (!pawn.Spawned)
			{
				result = false;
			}
			else
			{
				List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug);
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].Position.Fogged(list[i].Map) && (drugCategory == DrugCategory.Any || list[i].def.ingestible.drugCategory == drugCategory))
					{
						CompDrug compDrug = list[i].TryGetComp<CompDrug>();
						if (compDrug.Props.chemical == chemical && (list[i].Position.Roofed(list[i].Map) || list[i].Position.InHorDistOf(pawn.Position, 45f)) && pawn.CanReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
							goto IL_011e;
					}
				}
				result = false;
			}
			goto IL_013c;
			IL_013c:
			return result;
			IL_011e:
			result = true;
			goto IL_013c;
		}
	}
}
