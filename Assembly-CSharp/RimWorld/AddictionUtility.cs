using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000461 RID: 1121
	public static class AddictionUtility
	{
		// Token: 0x060013B4 RID: 5044 RVA: 0x000AB660 File Offset: 0x000A9A60
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x000AB684 File Offset: 0x000A9A84
		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000AB6A8 File Offset: 0x000A9AA8
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

		// Token: 0x060013B7 RID: 5047 RVA: 0x000AB704 File Offset: 0x000A9B04
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000AB74C File Offset: 0x000A9B4C
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

		// Token: 0x060013B9 RID: 5049 RVA: 0x000AB7A8 File Offset: 0x000A9BA8
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

		// Token: 0x060013BA RID: 5050 RVA: 0x000AB804 File Offset: 0x000A9C04
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

		// Token: 0x060013BB RID: 5051 RVA: 0x000AB874 File Offset: 0x000A9C74
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

		// Token: 0x060013BC RID: 5052 RVA: 0x000AB8CC File Offset: 0x000A9CCC
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
