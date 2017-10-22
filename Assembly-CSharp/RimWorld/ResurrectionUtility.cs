using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class ResurrectionUtility
	{
		private const float BrainDamageChancePerDayRotting = 0.3f;

		private const float BlindnessChancePerDayRotting = 0.3f;

		private const float ResurrectionPsychosisChance = 0.3f;

		public static void Resurrect(Pawn pawn)
		{
			if (!pawn.Dead)
			{
				Log.Error("Tried to resurrect a pawn who is not dead: " + pawn.ToStringSafe());
			}
			else if (pawn.Discarded)
			{
				Log.Error("Tried to resurrect a discarded pawn: " + pawn.ToStringSafe());
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
					GenSpawn.Spawn(pawn, loc, map);
					for (int i = 0; i < 10; i++)
					{
						MoteMaker.ThrowAirPuffUp(pawn.DrawPos, map);
					}
					if (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.HostileTo(Faction.OfPlayer))
					{
						LordMaker.MakeNewLord(pawn.Faction, new LordJob_AssaultColony(pawn.Faction, true, true, false, false, true), pawn.Map, Gen.YieldSingle(pawn));
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
			}
		}

		public static void ResurrectWithSideEffects(Pawn pawn)
		{
			Corpse corpse = pawn.Corpse;
			float num;
			if (corpse != null)
			{
				CompRottable comp = corpse.GetComp<CompRottable>();
				num = Mathf.Max((float)((comp.RotProgress - (float)comp.PropsRot.TicksToRotStart) / 60000.0), 0f);
			}
			else
			{
				num = 0f;
			}
			ResurrectionUtility.Resurrect(pawn);
			BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
			if (Rand.Chance((float)(0.30000001192092896 * num)) && brain != null)
			{
				int a = Rand.RangeInclusive(1, 5);
				int b = Mathf.FloorToInt(pawn.health.hediffSet.GetPartHealth(brain)) - 1;
				a = Mathf.Min(a, b);
				if (a > 0 && !pawn.health.WouldDieAfterAddingHediff(HediffDefOf.Burn, brain, (float)a))
				{
					DamageDef burn = DamageDefOf.Burn;
					int amount = a;
					BodyPartRecord hitPart = brain;
					pawn.TakeDamage(new DamageInfo(burn, amount, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			}
			if (Rand.Chance((float)(0.30000001192092896 * num)))
			{
				IEnumerable<BodyPartRecord> enumerable = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where x.def == BodyPartDefOf.LeftEye || x.def == BodyPartDefOf.RightEye
				select x;
				foreach (BodyPartRecord item in enumerable)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Blindness, pawn, item);
					pawn.health.AddHediff(hediff, null, default(DamageInfo?));
				}
			}
			Hediff hediff2 = HediffMaker.MakeHediff(HediffDefOf.ResurrectionSickness, pawn, null);
			pawn.health.AddHediff(hediff2, null, default(DamageInfo?));
			if (Rand.Chance(0.3f) && brain != null)
			{
				Hediff hediff3 = HediffMaker.MakeHediff(HediffDefOf.ResurrectionPsychosis, pawn, brain);
				pawn.health.AddHediff(hediff3, null, default(DamageInfo?));
			}
			if (pawn.Dead)
			{
				Log.Error("The pawn has died while being resurrected.");
				ResurrectionUtility.Resurrect(pawn);
			}
		}
	}
}
