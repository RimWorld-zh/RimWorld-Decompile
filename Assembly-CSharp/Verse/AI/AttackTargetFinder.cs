using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000ADE RID: 2782
	public static class AttackTargetFinder
	{
		// Token: 0x040026D7 RID: 9943
		private const float FriendlyFireScoreOffsetPerHumanlikeOrMechanoid = 18f;

		// Token: 0x040026D8 RID: 9944
		private const float FriendlyFireScoreOffsetPerAnimal = 7f;

		// Token: 0x040026D9 RID: 9945
		private const float FriendlyFireScoreOffsetPerNonPawn = 10f;

		// Token: 0x040026DA RID: 9946
		private const float FriendlyFireScoreOffsetSelf = 40f;

		// Token: 0x040026DB RID: 9947
		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x040026DC RID: 9948
		private static List<Pair<IAttackTarget, float>> availableShootingTargets = new List<Pair<IAttackTarget, float>>();

		// Token: 0x040026DD RID: 9949
		private static List<float> tmpTargetScores = new List<float>();

		// Token: 0x040026DE RID: 9950
		private static List<bool> tmpCanShootAtTarget = new List<bool>();

		// Token: 0x040026DF RID: 9951
		private static List<IntVec3> tempDestList = new List<IntVec3>();

		// Token: 0x040026E0 RID: 9952
		private static List<IntVec3> tempSourceList = new List<IntVec3>();

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00206A0C File Offset: 0x00204E0C
		public static IAttackTarget BestAttackTarget(IAttackTargetSearcher searcher, TargetScanFlags flags, Predicate<Thing> validator = null, float minDist = 0f, float maxDist = 9999f, IntVec3 locus = default(IntVec3), float maxTravelRadiusFromLocus = 3.40282347E+38f, bool canBash = false)
		{
			Thing searcherThing = searcher.Thing;
			Pawn searcherPawn = searcher as Pawn;
			Verb verb = searcher.CurrentEffectiveVerb;
			IAttackTarget result;
			if (verb == null)
			{
				Log.Error("BestAttackTarget with " + searcher + " who has no attack verb.", false);
				result = null;
			}
			else
			{
				bool onlyTargetMachines = verb != null && verb.IsEMP();
				float minDistanceSquared = minDist * minDist;
				float num = maxTravelRadiusFromLocus + verb.verbProps.range;
				float maxLocusDistSquared = num * num;
				Func<IntVec3, bool> losValidator = null;
				if ((byte)(flags & TargetScanFlags.LOSBlockableByGas) != 0)
				{
					losValidator = delegate(IntVec3 vec3)
					{
						Gas gas = vec3.GetGas(searcherThing.Map);
						return gas == null || !gas.def.gas.blockTurretTracking;
					};
				}
				Predicate<IAttackTarget> innerValidator = delegate(IAttackTarget t)
				{
					Thing thing = t.Thing;
					bool result2;
					if (t == searcher)
					{
						result2 = false;
					}
					else if (minDistanceSquared > 0f && (float)(searcherThing.Position - thing.Position).LengthHorizontalSquared < minDistanceSquared)
					{
						result2 = false;
					}
					else
					{
						if (maxTravelRadiusFromLocus < 9999f)
						{
							if ((float)(thing.Position - locus).LengthHorizontalSquared > maxLocusDistSquared)
							{
								return false;
							}
						}
						if (!searcherThing.HostileTo(thing))
						{
							result2 = false;
						}
						else if (validator != null && !validator(thing))
						{
							result2 = false;
						}
						else
						{
							if (searcherPawn != null)
							{
								Lord lord = searcherPawn.GetLord();
								if (lord != null && !lord.LordJob.ValidateAttackTarget(searcherPawn, thing))
								{
									return false;
								}
							}
							if ((byte)(flags & TargetScanFlags.NeedLOSToAll) != 0)
							{
								if (!searcherThing.CanSee(thing, losValidator))
								{
									if (t is Pawn)
									{
										if ((byte)(flags & TargetScanFlags.NeedLOSToPawns) != 0)
										{
											return false;
										}
									}
									else if ((byte)(flags & TargetScanFlags.NeedLOSToNonPawns) != 0)
									{
										return false;
									}
								}
							}
							if ((byte)(flags & TargetScanFlags.NeedThreat) != 0)
							{
								if (t.ThreatDisabled(searcher))
								{
									return false;
								}
							}
							Pawn pawn = t as Pawn;
							if (onlyTargetMachines && pawn != null && pawn.RaceProps.IsFlesh)
							{
								result2 = false;
							}
							else if ((byte)(flags & TargetScanFlags.NeedNonBurning) != 0 && thing.IsBurning())
							{
								result2 = false;
							}
							else
							{
								if (searcherThing.def.race != null && searcherThing.def.race.intelligence >= Intelligence.Humanlike)
								{
									CompExplosive compExplosive = thing.TryGetComp<CompExplosive>();
									if (compExplosive != null && compExplosive.wickStarted)
									{
										return false;
									}
								}
								if (thing.def.size.x == 1 && thing.def.size.z == 1)
								{
									if (thing.Position.Fogged(thing.Map))
									{
										return false;
									}
								}
								else
								{
									bool flag2 = false;
									CellRect.CellRectIterator iterator = thing.OccupiedRect().GetIterator();
									while (!iterator.Done())
									{
										if (!iterator.Current.Fogged(thing.Map))
										{
											flag2 = true;
											break;
										}
										iterator.MoveNext();
									}
									if (!flag2)
									{
										return false;
									}
								}
								result2 = true;
							}
						}
					}
					return result2;
				};
				if (AttackTargetFinder.HasRangedAttack(searcher))
				{
					AttackTargetFinder.tmpTargets.Clear();
					AttackTargetFinder.tmpTargets.AddRange(searcherThing.Map.attackTargetsCache.GetPotentialTargetsFor(searcher));
					if ((byte)(flags & TargetScanFlags.NeedReachable) != 0)
					{
						Predicate<IAttackTarget> oldValidator = innerValidator;
						innerValidator = ((IAttackTarget t) => oldValidator(t) && AttackTargetFinder.CanReach(searcherThing, t.Thing, canBash));
					}
					bool flag = false;
					for (int i = 0; i < AttackTargetFinder.tmpTargets.Count; i++)
					{
						IAttackTarget attackTarget = AttackTargetFinder.tmpTargets[i];
						if (attackTarget.Thing.Position.InHorDistOf(searcherThing.Position, maxDist) && innerValidator(attackTarget) && AttackTargetFinder.CanShootAtFromCurrentPosition(attackTarget, searcher, verb))
						{
							flag = true;
							break;
						}
					}
					IAttackTarget attackTarget2;
					if (flag)
					{
						AttackTargetFinder.tmpTargets.RemoveAll((IAttackTarget x) => !x.Thing.Position.InHorDistOf(searcherThing.Position, maxDist) || !innerValidator(x));
						attackTarget2 = AttackTargetFinder.GetRandomShootingTargetByScore(AttackTargetFinder.tmpTargets, searcher, verb);
					}
					else
					{
						Predicate<Thing> validator2;
						if ((byte)(flags & TargetScanFlags.NeedReachableIfCantHitFromMyPos) != 0 && (byte)(flags & TargetScanFlags.NeedReachable) == 0)
						{
							validator2 = ((Thing t) => innerValidator((IAttackTarget)t) && (AttackTargetFinder.CanReach(searcherThing, t, canBash) || AttackTargetFinder.CanShootAtFromCurrentPosition((IAttackTarget)t, searcher, verb)));
						}
						else
						{
							validator2 = ((Thing t) => innerValidator((IAttackTarget)t));
						}
						attackTarget2 = (IAttackTarget)GenClosest.ClosestThing_Global(searcherThing.Position, AttackTargetFinder.tmpTargets, maxDist, validator2, null);
					}
					AttackTargetFinder.tmpTargets.Clear();
					result = attackTarget2;
				}
				else
				{
					if (searcherPawn != null && searcherPawn.mindState.duty != null && searcherPawn.mindState.duty.radius > 0f && !searcherPawn.InMentalState)
					{
						Predicate<IAttackTarget> oldValidator = innerValidator;
						innerValidator = ((IAttackTarget t) => oldValidator(t) && t.Thing.Position.InHorDistOf(searcherPawn.mindState.duty.focus.Cell, searcherPawn.mindState.duty.radius));
					}
					IntVec3 position = searcherThing.Position;
					Map map = searcherThing.Map;
					ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.AttackTarget);
					PathEndMode peMode = PathEndMode.Touch;
					Pawn searcherPawn2 = searcherPawn;
					Danger maxDanger = Danger.Deadly;
					bool canBash2 = canBash;
					TraverseParms traverseParams = TraverseParms.For(searcherPawn2, maxDanger, TraverseMode.ByPawn, canBash2);
					float maxDist2 = maxDist;
					Predicate<Thing> validator3 = (Thing x) => innerValidator((IAttackTarget)x);
					int searchRegionsMax = (maxDist <= 800f) ? 40 : -1;
					IAttackTarget attackTarget3 = (IAttackTarget)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDist2, validator3, null, 0, searchRegionsMax, false, RegionType.Set_Passable, false);
					if (attackTarget3 != null && PawnUtility.ShouldCollideWithPawns(searcherPawn))
					{
						IAttackTarget attackTarget4 = AttackTargetFinder.FindBestReachableMeleeTarget(innerValidator, searcherPawn, maxDist, canBash);
						if (attackTarget4 != null)
						{
							float lengthHorizontal = (searcherPawn.Position - attackTarget3.Thing.Position).LengthHorizontal;
							float lengthHorizontal2 = (searcherPawn.Position - attackTarget4.Thing.Position).LengthHorizontal;
							if (Mathf.Abs(lengthHorizontal - lengthHorizontal2) < 50f)
							{
								attackTarget3 = attackTarget4;
							}
						}
					}
					result = attackTarget3;
				}
			}
			return result;
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00206EC4 File Offset: 0x002052C4
		private static bool CanReach(Thing searcher, Thing target, bool canBash)
		{
			Pawn pawn = searcher as Pawn;
			if (pawn != null)
			{
				if (!pawn.CanReach(target, PathEndMode.Touch, Danger.Some, canBash, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			else
			{
				TraverseMode mode = (!canBash) ? TraverseMode.NoPassClosedDoors : TraverseMode.PassDoors;
				if (!searcher.Map.reachability.CanReach(searcher.Position, target, PathEndMode.Touch, TraverseParms.For(mode, Danger.Deadly, false)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00206F4C File Offset: 0x0020534C
		private static IAttackTarget FindBestReachableMeleeTarget(Predicate<IAttackTarget> validator, Pawn searcherPawn, float maxTargDist, bool canBash)
		{
			maxTargDist = Mathf.Min(maxTargDist, 30f);
			IAttackTarget reachableTarget = null;
			Func<IntVec3, IAttackTarget> bestTargetOnCell = delegate(IntVec3 x)
			{
				List<Thing> thingList = x.GetThingList(searcherPawn.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					IAttackTarget attackTarget = thing as IAttackTarget;
					if (attackTarget != null)
					{
						if (validator(attackTarget))
						{
							if (ReachabilityImmediate.CanReachImmediate(x, thing, searcherPawn.Map, PathEndMode.Touch, searcherPawn))
							{
								if (searcherPawn.CanReachImmediate(thing, PathEndMode.Touch) || searcherPawn.Map.attackTargetReservationManager.CanReserve(searcherPawn, attackTarget))
								{
									return attackTarget;
								}
							}
						}
					}
				}
				return null;
			};
			searcherPawn.Map.floodFiller.FloodFill(searcherPawn.Position, delegate(IntVec3 x)
			{
				bool result;
				if (!x.Walkable(searcherPawn.Map))
				{
					result = false;
				}
				else if ((float)x.DistanceToSquared(searcherPawn.Position) > maxTargDist * maxTargDist)
				{
					result = false;
				}
				else
				{
					if (!canBash)
					{
						Building_Door building_Door = x.GetEdifice(searcherPawn.Map) as Building_Door;
						if (building_Door != null && !building_Door.CanPhysicallyPass(searcherPawn))
						{
							return false;
						}
					}
					result = !PawnUtility.AnyPawnBlockingPathAt(x, searcherPawn, true, false, false);
				}
				return result;
			}, delegate(IntVec3 x)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = x + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(searcherPawn.Map))
					{
						IAttackTarget attackTarget = bestTargetOnCell(intVec);
						if (attackTarget != null)
						{
							reachableTarget = attackTarget;
							break;
						}
					}
				}
				return reachableTarget != null;
			}, int.MaxValue, false, null);
			return reachableTarget;
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00206FF8 File Offset: 0x002053F8
		private static bool HasRangedAttack(IAttackTargetSearcher t)
		{
			Verb currentEffectiveVerb = t.CurrentEffectiveVerb;
			return currentEffectiveVerb != null && !currentEffectiveVerb.verbProps.IsMeleeAttack;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x0020702C File Offset: 0x0020542C
		private static bool CanShootAtFromCurrentPosition(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			return verb != null && verb.CanHitTargetFrom(searcher.Thing.Position, target.Thing);
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x0020706C File Offset: 0x0020546C
		private static IAttackTarget GetRandomShootingTargetByScore(List<IAttackTarget> targets, IAttackTargetSearcher searcher, Verb verb)
		{
			Pair<IAttackTarget, float> pair;
			IAttackTarget result;
			if (AttackTargetFinder.GetAvailableShootingTargetsByScore(targets, searcher, verb).TryRandomElementByWeight((Pair<IAttackTarget, float> x) => x.Second, out pair))
			{
				result = pair.First;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x002070C0 File Offset: 0x002054C0
		private static List<Pair<IAttackTarget, float>> GetAvailableShootingTargetsByScore(List<IAttackTarget> rawTargets, IAttackTargetSearcher searcher, Verb verb)
		{
			AttackTargetFinder.availableShootingTargets.Clear();
			List<Pair<IAttackTarget, float>> result;
			if (rawTargets.Count == 0)
			{
				result = AttackTargetFinder.availableShootingTargets;
			}
			else
			{
				AttackTargetFinder.tmpTargetScores.Clear();
				AttackTargetFinder.tmpCanShootAtTarget.Clear();
				float num = 0f;
				IAttackTarget attackTarget = null;
				for (int i = 0; i < rawTargets.Count; i++)
				{
					AttackTargetFinder.tmpTargetScores.Add(float.MinValue);
					AttackTargetFinder.tmpCanShootAtTarget.Add(false);
					if (rawTargets[i] != searcher)
					{
						bool flag = AttackTargetFinder.CanShootAtFromCurrentPosition(rawTargets[i], searcher, verb);
						AttackTargetFinder.tmpCanShootAtTarget[i] = flag;
						if (flag)
						{
							float shootingTargetScore = AttackTargetFinder.GetShootingTargetScore(rawTargets[i], searcher, verb);
							AttackTargetFinder.tmpTargetScores[i] = shootingTargetScore;
							if (attackTarget == null || shootingTargetScore > num)
							{
								attackTarget = rawTargets[i];
								num = shootingTargetScore;
							}
						}
					}
				}
				if (num < 1f)
				{
					if (attackTarget != null)
					{
						AttackTargetFinder.availableShootingTargets.Add(new Pair<IAttackTarget, float>(attackTarget, 1f));
					}
				}
				else
				{
					float num2 = num - 30f;
					for (int j = 0; j < rawTargets.Count; j++)
					{
						if (rawTargets[j] != searcher)
						{
							if (AttackTargetFinder.tmpCanShootAtTarget[j])
							{
								float num3 = AttackTargetFinder.tmpTargetScores[j];
								if (num3 >= num2)
								{
									float second = Mathf.InverseLerp(num - 30f, num, num3);
									AttackTargetFinder.availableShootingTargets.Add(new Pair<IAttackTarget, float>(rawTargets[j], second));
								}
							}
						}
					}
				}
				result = AttackTargetFinder.availableShootingTargets;
			}
			return result;
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00207280 File Offset: 0x00205680
		private static float GetShootingTargetScore(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			float num = 60f;
			num -= Mathf.Min((target.Thing.Position - searcher.Thing.Position).LengthHorizontal, 40f);
			if (target.TargetCurrentlyAimingAt == searcher.Thing)
			{
				num += 10f;
			}
			if (searcher.LastAttackedTarget == target.Thing && Find.TickManager.TicksGame - searcher.LastAttackTargetTick <= 300)
			{
				num += 40f;
			}
			num -= CoverUtility.CalculateOverallBlockChance(target.Thing.Position, searcher.Thing.Position, searcher.Thing.Map) * 10f;
			Pawn pawn = target as Pawn;
			if (pawn != null && pawn.RaceProps.Animal && pawn.Faction != null && !pawn.IsFighting())
			{
				num -= 50f;
			}
			num += AttackTargetFinder.FriendlyFireBlastRadiusTargetScoreOffset(target, searcher, verb);
			return num + AttackTargetFinder.FriendlyFireConeTargetScoreOffset(target, searcher, verb);
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x002073B8 File Offset: 0x002057B8
		private static float FriendlyFireBlastRadiusTargetScoreOffset(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			float result;
			if (verb.verbProps.ai_AvoidFriendlyFireRadius <= 0f)
			{
				result = 0f;
			}
			else
			{
				Map map = target.Thing.Map;
				IntVec3 position = target.Thing.Position;
				int num = GenRadial.NumCellsInRadius(verb.verbProps.ai_AvoidFriendlyFireRadius);
				float num2 = 0f;
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(map))
					{
						bool flag = true;
						List<Thing> thingList = intVec.GetThingList(map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j] is IAttackTarget && thingList[j] != target)
							{
								if (flag)
								{
									if (!GenSight.LineOfSight(position, intVec, map, true, null, 0, 0))
									{
										break;
									}
									flag = false;
								}
								float num3;
								if (thingList[j] == searcher)
								{
									num3 = 40f;
								}
								else if (thingList[j] is Pawn)
								{
									num3 = ((!thingList[j].def.race.Animal) ? 18f : 7f);
								}
								else
								{
									num3 = 10f;
								}
								if (searcher.Thing.HostileTo(thingList[j]))
								{
									num2 += num3 * 0.6f;
								}
								else
								{
									num2 -= num3;
								}
							}
						}
					}
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00207574 File Offset: 0x00205974
		private static float FriendlyFireConeTargetScoreOffset(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			Pawn pawn = searcher.Thing as Pawn;
			float result;
			if (pawn == null)
			{
				result = 0f;
			}
			else if (pawn.RaceProps.intelligence < Intelligence.ToolUser)
			{
				result = 0f;
			}
			else if (pawn.RaceProps.IsMechanoid)
			{
				result = 0f;
			}
			else
			{
				Verb_Shoot verb_Shoot = verb as Verb_Shoot;
				if (verb_Shoot == null)
				{
					result = 0f;
				}
				else
				{
					ThingDef defaultProjectile = verb_Shoot.verbProps.defaultProjectile;
					if (defaultProjectile == null)
					{
						result = 0f;
					}
					else if (defaultProjectile.projectile.flyOverhead)
					{
						result = 0f;
					}
					else
					{
						Map map = pawn.Map;
						ShotReport report = ShotReport.HitReportFor(pawn, verb, (Thing)target);
						float a = VerbUtility.CalculateAdjustedForcedMiss(verb.verbProps.forcedMissRadius, report.ShootLine.Dest - report.ShootLine.Source);
						float radius = Mathf.Max(a, 1.5f);
						IntVec3 dest2 = report.ShootLine.Dest;
						IEnumerable<IntVec3> source = from dest in GenRadial.RadialCellsAround(dest2, radius, true)
						where dest.InBounds(map)
						select dest;
						IEnumerable<ShootLine> source2 = from dest in source
						select new ShootLine(report.ShootLine.Source, dest);
						IEnumerable<IntVec3> source3 = source2.SelectMany((ShootLine line) => line.Points().Concat(line.Dest).TakeWhile((IntVec3 pos) => pos.CanBeSeenOverFast(map)));
						IEnumerable<IntVec3> enumerable = source3.Distinct<IntVec3>();
						float num = 0f;
						foreach (IntVec3 c in enumerable)
						{
							float num2 = VerbUtility.DistanceInterceptChance(report.ShootLine.Source.ToVector3Shifted(), c, ((Thing)target).Position);
							if (num2 > 0f)
							{
								List<Thing> thingList = c.GetThingList(map);
								for (int i = 0; i < thingList.Count; i++)
								{
									Thing thing = thingList[i];
									if (thing is IAttackTarget && thing != target)
									{
										float num3;
										if (thing == searcher)
										{
											num3 = 40f;
										}
										else if (thing is Pawn)
										{
											num3 = ((!thing.def.race.Animal) ? 18f : 7f);
										}
										else
										{
											num3 = 10f;
										}
										num3 *= num2;
										if (searcher.Thing.HostileTo(thing))
										{
											num3 *= 0.6f;
										}
										else
										{
											num3 *= -1f;
										}
										num += num3;
									}
								}
							}
						}
						result = num;
					}
				}
			}
			return result;
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00207890 File Offset: 0x00205C90
		public static IAttackTarget BestShootTargetFromCurrentPosition(IAttackTargetSearcher searcher, Predicate<Thing> validator, float maxDistance, float minDistance, TargetScanFlags flags)
		{
			return AttackTargetFinder.BestAttackTarget(searcher, flags, validator, minDistance, maxDistance, default(IntVec3), float.MaxValue, false);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x002078C0 File Offset: 0x00205CC0
		public static bool CanSee(this Thing seer, Thing target, Func<IntVec3, bool> validator = null)
		{
			ShootLeanUtility.CalcShootableCellsOf(AttackTargetFinder.tempDestList, target);
			for (int i = 0; i < AttackTargetFinder.tempDestList.Count; i++)
			{
				if (GenSight.LineOfSight(seer.Position, AttackTargetFinder.tempDestList[i], seer.Map, true, validator, 0, 0))
				{
					return true;
				}
			}
			ShootLeanUtility.LeanShootingSourcesFromTo(seer.Position, target.Position, seer.Map, AttackTargetFinder.tempSourceList);
			for (int j = 0; j < AttackTargetFinder.tempSourceList.Count; j++)
			{
				for (int k = 0; k < AttackTargetFinder.tempDestList.Count; k++)
				{
					if (GenSight.LineOfSight(AttackTargetFinder.tempSourceList[j], AttackTargetFinder.tempDestList[k], seer.Map, true, validator, 0, 0))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x002079B4 File Offset: 0x00205DB4
		public static void DebugDrawAttackTargetScores_Update()
		{
			IAttackTargetSearcher attackTargetSearcher = Find.Selector.SingleSelectedThing as IAttackTargetSearcher;
			if (attackTargetSearcher != null)
			{
				if (attackTargetSearcher.Thing.Map == Find.CurrentMap)
				{
					Verb currentEffectiveVerb = attackTargetSearcher.CurrentEffectiveVerb;
					if (currentEffectiveVerb != null)
					{
						AttackTargetFinder.tmpTargets.Clear();
						List<Thing> list = attackTargetSearcher.Thing.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
						for (int i = 0; i < list.Count; i++)
						{
							AttackTargetFinder.tmpTargets.Add((IAttackTarget)list[i]);
						}
						List<Pair<IAttackTarget, float>> availableShootingTargetsByScore = AttackTargetFinder.GetAvailableShootingTargetsByScore(AttackTargetFinder.tmpTargets, attackTargetSearcher, currentEffectiveVerb);
						for (int j = 0; j < availableShootingTargetsByScore.Count; j++)
						{
							GenDraw.DrawLineBetween(attackTargetSearcher.Thing.DrawPos, availableShootingTargetsByScore[j].First.Thing.DrawPos);
						}
					}
				}
			}
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x00207AB4 File Offset: 0x00205EB4
		public static void DebugDrawAttackTargetScores_OnGUI()
		{
			IAttackTargetSearcher attackTargetSearcher = Find.Selector.SingleSelectedThing as IAttackTargetSearcher;
			if (attackTargetSearcher != null)
			{
				if (attackTargetSearcher.Thing.Map == Find.CurrentMap)
				{
					Verb currentEffectiveVerb = attackTargetSearcher.CurrentEffectiveVerb;
					if (currentEffectiveVerb != null)
					{
						List<Thing> list = attackTargetSearcher.Thing.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
						Text.Anchor = TextAnchor.MiddleCenter;
						Text.Font = GameFont.Tiny;
						for (int i = 0; i < list.Count; i++)
						{
							Thing thing = list[i];
							if (thing != attackTargetSearcher)
							{
								string text;
								Color red;
								if (!AttackTargetFinder.CanShootAtFromCurrentPosition((IAttackTarget)thing, attackTargetSearcher, currentEffectiveVerb))
								{
									text = "out of range";
									red = Color.red;
								}
								else
								{
									text = AttackTargetFinder.GetShootingTargetScore((IAttackTarget)thing, attackTargetSearcher, currentEffectiveVerb).ToString("F0");
									red = new Color(0.25f, 1f, 0.25f);
								}
								Vector2 screenPos = thing.DrawPos.MapToUIPosition();
								GenMapUI.DrawThingLabel(screenPos, text, red);
							}
						}
						Text.Anchor = TextAnchor.UpperLeft;
						Text.Font = GameFont.Small;
					}
				}
			}
		}
	}
}
