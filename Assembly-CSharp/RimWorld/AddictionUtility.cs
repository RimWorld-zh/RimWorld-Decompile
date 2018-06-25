using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000463 RID: 1123
	public static class AddictionUtility
	{
		// Token: 0x060013B8 RID: 5048 RVA: 0x000AB7B0 File Offset: 0x000A9BB0
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000AB7D4 File Offset: 0x000A9BD4
		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000AB7F8 File Offset: 0x000A9BF8
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
				if (!compDrug.Props.Addictive)
				{
					result = null;
				}
				else
				{
					result = AddictionUtility.FindAddictionHediff(pawn, compDrug.Props.chemical);
				}
			}
			return result;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000AB854 File Offset: 0x000A9C54
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000AB89C File Offset: 0x000A9C9C
		public static Hediff FindToleranceHediff(Pawn pawn, ChemicalDef chemical)
		{
			Hediff result;
			if (chemical.toleranceHediff == null)
			{
				result = null;
			}
			else
			{
				result = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.toleranceHediff);
			}
			return result;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000AB8F8 File Offset: 0x000A9CF8
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

		// Token: 0x060013BE RID: 5054 RVA: 0x000AB954 File Offset: 0x000A9D54
		public static void CheckDrugAddictionTeachOpportunity(Pawn pawn)
		{
			if (pawn.RaceProps.IsFlesh && pawn.Spawned)
			{
				if (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer)
				{
					if (AddictionUtility.AddictedToAnything(pawn))
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugAddiction, pawn, OpportunityType.Important);
					}
				}
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x000AB9C4 File Offset: 0x000A9DC4
		public static bool AddictedToAnything(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_Addiction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x000ABA1C File Offset: 0x000A9E1C
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
					if (!list[i].Position.Fogged(list[i].Map))
					{
						if (drugCategory == DrugCategory.Any || list[i].def.ingestible.drugCategory == drugCategory)
						{
							CompDrug compDrug = list[i].TryGetComp<CompDrug>();
							if (compDrug.Props.chemical == chemical)
							{
								if (list[i].Position.Roofed(list[i].Map) || list[i].Position.InHorDistOf(pawn.Position, 45f))
								{
									if (pawn.CanReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
									{
										return true;
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
