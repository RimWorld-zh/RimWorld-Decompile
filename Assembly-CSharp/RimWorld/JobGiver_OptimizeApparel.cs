using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E6 RID: 230
	public class JobGiver_OptimizeApparel : ThinkNode_JobGiver
	{
		// Token: 0x040002BB RID: 699
		private static NeededWarmth neededWarmth;

		// Token: 0x040002BC RID: 700
		private static StringBuilder debugSb;

		// Token: 0x040002BD RID: 701
		private const int ApparelOptimizeCheckIntervalMin = 6000;

		// Token: 0x040002BE RID: 702
		private const int ApparelOptimizeCheckIntervalMax = 9000;

		// Token: 0x040002BF RID: 703
		private const float MinScoreGainToCare = 0.05f;

		// Token: 0x040002C0 RID: 704
		private const float ScoreFactorIfNotReplacing = 10f;

		// Token: 0x040002C1 RID: 705
		private static readonly SimpleCurve InsulationColdScoreFactorCurve_NeedWarm = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 8f),
				true
			}
		};

		// Token: 0x040002C2 RID: 706
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

		// Token: 0x060004F4 RID: 1268 RVA: 0x0003705E File Offset: 0x0003545E
		private void SetNextOptimizeTick(Pawn pawn)
		{
			pawn.mindState.nextApparelOptimizeTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00037088 File Offset: 0x00035488
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.outfits == null)
			{
				Log.ErrorOnce(pawn + " tried to run JobGiver_OptimizeApparel without an OutfitTracker", 5643897, false);
				result = null;
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				Log.ErrorOnce("Non-colonist " + pawn + " tried to optimize apparel.", 764323, false);
				result = null;
			}
			else
			{
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
					JobGiver_OptimizeApparel.debugSb.AppendLine(string.Concat(new object[]
					{
						"Scanning for ",
						pawn,
						" at ",
						pawn.Position
					}));
				}
				Outfit currentOutfit = pawn.outfits.CurrentOutfit;
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				for (int i = wornApparel.Count - 1; i >= 0; i--)
				{
					if (!currentOutfit.filter.Allows(wornApparel[i]) && pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]))
					{
						return new Job(JobDefOf.RemoveApparel, wornApparel[i])
						{
							haulDroppedApparel = true
						};
					}
				}
				Thing thing = null;
				float num = 0f;
				List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
				if (list.Count == 0)
				{
					this.SetNextOptimizeTick(pawn);
					result = null;
				}
				else
				{
					JobGiver_OptimizeApparel.neededWarmth = PawnApparelGenerator.CalculateNeededWarmth(pawn, pawn.Map.Tile, GenLocalDate.Twelfth(pawn));
					for (int j = 0; j < list.Count; j++)
					{
						Apparel apparel = (Apparel)list[j];
						if (currentOutfit.filter.Allows(apparel))
						{
							if (apparel.IsInAnyStorage())
							{
								if (!apparel.IsForbidden(pawn))
								{
									float num2 = JobGiver_OptimizeApparel.ApparelScoreGain(pawn, apparel);
									if (DebugViewSettings.debugApparelOptimize)
									{
										JobGiver_OptimizeApparel.debugSb.AppendLine(apparel.LabelCap + ": " + num2.ToString("F2"));
									}
									if (num2 >= 0.05f && num2 >= num)
									{
										if (ApparelUtility.HasPartsToWear(pawn, apparel.def))
										{
											if (pawn.CanReserveAndReach(apparel, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
											{
												thing = apparel;
												num = num2;
											}
										}
									}
								}
							}
						}
					}
					if (DebugViewSettings.debugApparelOptimize)
					{
						JobGiver_OptimizeApparel.debugSb.AppendLine("BEST: " + thing);
						Log.Message(JobGiver_OptimizeApparel.debugSb.ToString(), false);
						JobGiver_OptimizeApparel.debugSb = null;
					}
					if (thing == null)
					{
						this.SetNextOptimizeTick(pawn);
						result = null;
					}
					else
					{
						result = new Job(JobDefOf.Wear, thing);
					}
				}
			}
			return result;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000373B4 File Offset: 0x000357B4
		public static float ApparelScoreGain(Pawn pawn, Apparel ap)
		{
			float result;
			if (ap is ShieldBelt && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				result = -1000f;
			}
			else
			{
				float num = JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, ap);
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				bool flag = false;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(wornApparel[i].def, ap.def, pawn.RaceProps.body))
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
					num *= 10f;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000374B0 File Offset: 0x000358B0
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
				num -= 0.5f;
				if (num > 0f)
				{
					num *= 0.1f;
				}
			}
			if (ap.Stuff == ThingDefOf.Human.race.leatherDef)
			{
				if (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelSad))
				{
					num -= 0.5f;
					if (num > 0f)
					{
						num *= 0.1f;
					}
				}
				if (pawn != null && ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelHappy))
				{
					num += 0.12f;
				}
			}
			return num;
		}
	}
}
