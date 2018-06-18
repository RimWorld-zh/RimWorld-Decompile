using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000465 RID: 1125
	public static class AddictionUtility
	{
		// Token: 0x060013BD RID: 5053 RVA: 0x000AB650 File Offset: 0x000A9A50
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000AB674 File Offset: 0x000A9A74
		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x000AB698 File Offset: 0x000A9A98
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

		// Token: 0x060013C0 RID: 5056 RVA: 0x000AB6F4 File Offset: 0x000A9AF4
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000AB73C File Offset: 0x000A9B3C
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

		// Token: 0x060013C2 RID: 5058 RVA: 0x000AB798 File Offset: 0x000A9B98
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

		// Token: 0x060013C3 RID: 5059 RVA: 0x000AB7F4 File Offset: 0x000A9BF4
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

		// Token: 0x060013C4 RID: 5060 RVA: 0x000AB864 File Offset: 0x000A9C64
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

		// Token: 0x060013C5 RID: 5061 RVA: 0x000AB8BC File Offset: 0x000A9CBC
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
