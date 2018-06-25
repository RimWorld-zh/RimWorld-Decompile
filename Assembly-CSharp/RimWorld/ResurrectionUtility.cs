using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004AF RID: 1199
	public static class ResurrectionUtility
	{
		// Token: 0x04000CA0 RID: 3232
		private static SimpleCurve DementiaChancePerRotDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.8f),
				true
			}
		};

		// Token: 0x04000CA1 RID: 3233
		private static SimpleCurve BlindnessChancePerRotDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.8f),
				true
			}
		};

		// Token: 0x04000CA2 RID: 3234
		private static SimpleCurve ResurrectionPsychosisChancePerRotDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.8f),
				true
			}
		};

		// Token: 0x06001560 RID: 5472 RVA: 0x000BDD18 File Offset: 0x000BC118
		public static void Resurrect(Pawn pawn)
		{
			if (!pawn.Dead)
			{
				Log.Error("Tried to resurrect a pawn who is not dead: " + pawn.ToStringSafe<Pawn>(), false);
			}
			else if (pawn.Discarded)
			{
				Log.Error("Tried to resurrect a discarded pawn: " + pawn.ToStringSafe<Pawn>(), false);
			}
			else
			{
				Corpse corpse = pawn.Corpse;
				bool flag = false;
				IntVec3 loc = IntVec3.Invalid;
				Map map = null;
				if (corpse != null)
				{
					flag = corpse.Spawned;
					loc = corpse.Position;
					map = corpse.Map;
					corpse.InnerPawn = null;
					corpse.Destroy(DestroyMode.Vanish);
				}
				if (flag && pawn.IsWorldPawn())
				{
					Find.WorldPawns.RemovePawn(pawn);
				}
				pawn.ForceSetStateToUnspawned();
				PawnComponentsUtility.CreateInitialComponents(pawn);
				pawn.health.Notify_Resurrected();
				if (pawn.Faction != null && pawn.Faction.IsPlayer)
				{
					if (pawn.workSettings != null)
					{
						pawn.workSettings.EnableAndInitialize();
					}
					Find.Storyteller.intenderPopulation.Notify_PopulationGained();
				}
				if (flag)
				{
					GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
					for (int i = 0; i < 10; i++)
					{
						MoteMaker.ThrowAirPuffUp(pawn.DrawPos, map);
					}
					if (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.HostileTo(Faction.OfPlayer))
					{
						LordMaker.MakeNewLord(pawn.Faction, new LordJob_AssaultColony(pawn.Faction, true, true, false, false, true), pawn.Map, Gen.YieldSingle<Pawn>(pawn));
					}
					if (pawn.apparel != null)
					{
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int j = 0; j < wornApparel.Count; j++)
						{
							wornApparel[j].Notify_PawnResurrected();
						}
					}
				}
				PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(pawn);
			}
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x000BDEFC File Offset: 0x000BC2FC
		public static void ResurrectWithSideEffects(Pawn pawn)
		{
			Corpse corpse = pawn.Corpse;
			float x2;
			if (corpse != null)
			{
				CompRottable comp = corpse.GetComp<CompRottable>();
				x2 = comp.RotProgress / 60000f;
			}
			else
			{
				x2 = 0f;
			}
			ResurrectionUtility.Resurrect(pawn);
			BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ResurrectionSickness, pawn, null);
			if (!pawn.health.WouldDieAfterAddingHediff(hediff))
			{
				pawn.health.AddHediff(hediff, null, null, null);
			}
			float chance = ResurrectionUtility.DementiaChancePerRotDaysCurve.Evaluate(x2);
			if (Rand.Chance(chance) && brain != null)
			{
				Hediff hediff2 = HediffMaker.MakeHediff(HediffDefOf.Dementia, pawn, brain);
				if (!pawn.health.WouldDieAfterAddingHediff(hediff2))
				{
					pawn.health.AddHediff(hediff2, null, null, null);
				}
			}
			float chance2 = ResurrectionUtility.BlindnessChancePerRotDaysCurve.Evaluate(x2);
			if (Rand.Chance(chance2))
			{
				IEnumerable<BodyPartRecord> enumerable = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where x.def == BodyPartDefOf.Eye
				select x;
				foreach (BodyPartRecord partRecord in enumerable)
				{
					Hediff hediff3 = HediffMaker.MakeHediff(HediffDefOf.Blindness, pawn, partRecord);
					pawn.health.AddHediff(hediff3, null, null, null);
				}
			}
			if (brain != null)
			{
				float chance3 = ResurrectionUtility.ResurrectionPsychosisChancePerRotDaysCurve.Evaluate(x2);
				if (Rand.Chance(chance3))
				{
					Hediff hediff4 = HediffMaker.MakeHediff(HediffDefOf.ResurrectionPsychosis, pawn, brain);
					if (!pawn.health.WouldDieAfterAddingHediff(hediff4))
					{
						pawn.health.AddHediff(hediff4, null, null, null);
					}
				}
			}
			if (pawn.Dead)
			{
				Log.Error("The pawn has died while being resurrected.", false);
				ResurrectionUtility.Resurrect(pawn);
			}
		}
	}
}
