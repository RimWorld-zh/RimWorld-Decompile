using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_OptimizeApparel : ThinkNode_JobGiver
	{
		private const int ApparelOptimizeCheckIntervalMin = 6000;

		private const int ApparelOptimizeCheckIntervalMax = 9000;

		private const float MinScoreGainToCare = 0.05f;

		private const float ScoreFactorIfNotReplacing = 10f;

		private static NeededWarmth neededWarmth;

		private static StringBuilder debugSb;

		private static readonly SimpleCurve InsulationColdScoreFactorCurve_NeedWarm = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 8f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		private static readonly SimpleCurve HitPointsPercentScoreFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.2f, 0.15f),
				true
			},
			{
				new CurvePoint(0.25f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 0.4f),
				true
			},
			{
				new CurvePoint(0.55f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		private void SetNextOptimizeTick(Pawn pawn)
		{
			pawn.mindState.nextApparelOptimizeTick = Find.TickManager.TicksGame + Random.Range(6000, 9000);
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.outfits == null)
			{
				Log.ErrorOnce(pawn + " tried to run JobGiver_OptimizeApparel without an OutfitTracker", 5643897);
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				Log.ErrorOnce("Non-colonist " + pawn + " tried to optimize apparel.", 764323);
				return null;
			}
			if (!DebugViewSettings.debugApparelOptimize)
			{
				if (Find.TickManager.TicksGame < pawn.mindState.nextApparelOptimizeTick)
				{
					return null;
				}
			}
			else
			{
				JobGiver_OptimizeApparel.debugSb = new StringBuilder();
				JobGiver_OptimizeApparel.debugSb.AppendLine("Scanning for " + pawn + " at " + pawn.Position);
			}
			Outfit currentOutfit = pawn.outfits.CurrentOutfit;
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			for (int num = wornApparel.Count - 1; num >= 0; num--)
			{
				if (!currentOutfit.filter.Allows(wornApparel[num]) && pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[num]))
				{
					Job job = new Job(JobDefOf.RemoveApparel, (Thing)wornApparel[num]);
					job.haulDroppedApparel = true;
					return job;
				}
			}
			Thing thing = null;
			float num2 = 0f;
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
			if (list.Count == 0)
			{
				this.SetNextOptimizeTick(pawn);
				return null;
			}
			JobGiver_OptimizeApparel.neededWarmth = PawnApparelGenerator.CalculateNeededWarmth(pawn, pawn.Map.Tile, GenLocalDate.Twelfth(pawn));
			for (int i = 0; i < list.Count; i++)
			{
				Apparel apparel = (Apparel)list[i];
				if (currentOutfit.filter.Allows(apparel))
				{
					SlotGroup slotGroup = apparel.Map.slotGroupManager.SlotGroupAt(apparel.Position);
					if (slotGroup != null && !apparel.IsForbidden(pawn))
					{
						float num3 = JobGiver_OptimizeApparel.ApparelScoreGain(pawn, apparel);
						if (DebugViewSettings.debugApparelOptimize)
						{
							JobGiver_OptimizeApparel.debugSb.AppendLine(apparel.LabelCap + ": " + num3.ToString("F2"));
						}
						if (!(num3 < 0.05000000074505806) && !(num3 < num2) && ApparelUtility.HasPartsToWear(pawn, apparel.def) && pawn.CanReserveAndReach((Thing)apparel, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
						{
							thing = apparel;
							num2 = num3;
						}
					}
				}
			}
			if (DebugViewSettings.debugApparelOptimize)
			{
				JobGiver_OptimizeApparel.debugSb.AppendLine("BEST: " + thing);
				Log.Message(JobGiver_OptimizeApparel.debugSb.ToString());
				JobGiver_OptimizeApparel.debugSb = null;
			}
			if (thing == null)
			{
				this.SetNextOptimizeTick(pawn);
				return null;
			}
			return new Job(JobDefOf.Wear, thing);
		}

		public static float ApparelScoreGain(Pawn pawn, Apparel ap)
		{
			if (ap.def == ThingDefOf.Apparel_ShieldBelt && pawn.equipment.Primary != null && !pawn.equipment.Primary.def.Verbs[0].MeleeRange)
			{
				return -1000f;
			}
			float num = JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, ap);
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			bool flag = false;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(wornApparel[i].def, ap.def))
				{
					if (!pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]))
					{
						return -1000f;
					}
					num -= JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, wornApparel[i]);
					flag = true;
				}
			}
			if (!flag)
			{
				num = (float)(num * 10.0);
			}
			return num;
		}

		public static float ApparelScoreRaw(Pawn pawn, Apparel ap)
		{
			float num = 0.1f;
			float num2 = ap.GetStatValue(StatDefOf.ArmorRating_Sharp, true) + ap.GetStatValue(StatDefOf.ArmorRating_Blunt, true);
			num += num2;
			if (ap.def.useHitPoints)
			{
				float x = (float)ap.HitPoints / (float)ap.MaxHitPoints;
				num *= JobGiver_OptimizeApparel.HitPointsPercentScoreFactorCurve.Evaluate(x);
			}
			num += ap.GetSpecialApparelScoreOffset();
			float num3 = 1f;
			if (JobGiver_OptimizeApparel.neededWarmth == NeededWarmth.Warm)
			{
				float statValue = ap.GetStatValue(StatDefOf.Insulation_Cold, true);
				num3 *= JobGiver_OptimizeApparel.InsulationColdScoreFactorCurve_NeedWarm.Evaluate(statValue);
			}
			num *= num3;
			if (ap.WornByCorpse && (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.DeadMansApparel)))
			{
				num = (float)(num - 0.5);
				if (num > 0.0)
				{
					num = (float)(num * 0.10000000149011612);
				}
			}
			if (ap.Stuff == ThingDefOf.Human.race.leatherDef)
			{
				if (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelSad))
				{
					num = (float)(num - 0.5);
					if (num > 0.0)
					{
						num = (float)(num * 0.10000000149011612);
					}
				}
				if (pawn != null && ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelHappy))
				{
					num = (float)(num + 0.11999999731779099);
				}
			}
			return num;
		}
	}
}
