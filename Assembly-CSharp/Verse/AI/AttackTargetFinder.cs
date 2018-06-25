using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
	public static class AttackTargetFinder
	{
		private const float FriendlyFireScoreOffsetPerHumanlikeOrMechanoid = 18f;

		private const float FriendlyFireScoreOffsetPerAnimal = 7f;

		private const float FriendlyFireScoreOffsetPerNonPawn = 10f;

		private const float FriendlyFireScoreOffsetSelf = 40f;

		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		private static List<Pair<IAttackTarget, float>> availableShootingTargets = new List<Pair<IAttackTarget, float>>();

		private static List<float> tmpTargetScores = new List<float>();

		private static List<bool> tmpCanShootAtTarget = new List<bool>();

		private static List<IntVec3> tempDestList = new List<IntVec3>();

		private static List<IntVec3> tempSourceList = new List<IntVec3>();

		[CompilerGenerated]
		private static Func<Pair<IAttackTarget, float>, float> <>f__am$cache0;

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

		private static bool HasRangedAttack(IAttackTargetSearcher t)
		{
			Verb currentEffectiveVerb = t.CurrentEffectiveVerb;
			return currentEffectiveVerb != null && !currentEffectiveVerb.verbProps.IsMeleeAttack;
		}

		private static bool CanShootAtFromCurrentPosition(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			return verb != null && verb.CanHitTargetFrom(searcher.Thing.Position, target.Thing);
		}

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

		public static IAttackTarget BestShootTargetFromCurrentPosition(IAttackTargetSearcher searcher, Predicate<Thing> validator, float maxDistance, float minDistance, TargetScanFlags flags)
		{
			return AttackTargetFinder.BestAttackTarget(searcher, flags, validator, minDistance, maxDistance, default(IntVec3), float.MaxValue, false);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static AttackTargetFinder()
		{
		}

		[CompilerGenerated]
		private static float <GetRandomShootingTargetByScore>m__0(Pair<IAttackTarget, float> x)
		{
			return x.Second;
		}

		[CompilerGenerated]
		private sealed class <BestAttackTarget>c__AnonStorey0
		{
			internal Thing searcherThing;

			internal IAttackTargetSearcher searcher;

			internal float minDistanceSquared;

			internal float maxTravelRadiusFromLocus;

			internal IntVec3 locus;

			internal float maxLocusDistSquared;

			internal Predicate<Thing> validator;

			internal Pawn searcherPawn;

			internal TargetScanFlags flags;

			internal Func<IntVec3, bool> losValidator;

			internal bool onlyTargetMachines;

			internal bool canBash;

			internal float maxDist;

			internal Predicate<IAttackTarget> innerValidator;

			internal Verb verb;

			public <BestAttackTarget>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 vec3)
			{
				Gas gas = vec3.GetGas(this.searcherThing.Map);
				return gas == null || !gas.def.gas.blockTurretTracking;
			}

			internal bool <>m__1(IAttackTarget t)
			{
				Thing thing = t.Thing;
				bool result;
				if (t == this.searcher)
				{
					result = false;
				}
				else if (this.minDistanceSquared > 0f && (float)(this.searcherThing.Position - thing.Position).LengthHorizontalSquared < this.minDistanceSquared)
				{
					result = false;
				}
				else
				{
					if (this.maxTravelRadiusFromLocus < 9999f)
					{
						if ((float)(thing.Position - this.locus).LengthHorizontalSquared > this.maxLocusDistSquared)
						{
							return false;
						}
					}
					if (!this.searcherThing.HostileTo(thing))
					{
						result = false;
					}
					else if (this.validator != null && !this.validator(thing))
					{
						result = false;
					}
					else
					{
						if (this.searcherPawn != null)
						{
							Lord lord = this.searcherPawn.GetLord();
							if (lord != null && !lord.LordJob.ValidateAttackTarget(this.searcherPawn, thing))
							{
								return false;
							}
						}
						if ((byte)(this.flags & TargetScanFlags.NeedLOSToAll) != 0)
						{
							if (!this.searcherThing.CanSee(thing, this.losValidator))
							{
								if (t is Pawn)
								{
									if ((byte)(this.flags & TargetScanFlags.NeedLOSToPawns) != 0)
									{
										return false;
									}
								}
								else if ((byte)(this.flags & TargetScanFlags.NeedLOSToNonPawns) != 0)
								{
									return false;
								}
							}
						}
						if ((byte)(this.flags & TargetScanFlags.NeedThreat) != 0)
						{
							if (t.ThreatDisabled(this.searcher))
							{
								return false;
							}
						}
						Pawn pawn = t as Pawn;
						if (this.onlyTargetMachines && pawn != null && pawn.RaceProps.IsFlesh)
						{
							result = false;
						}
						else if ((byte)(this.flags & TargetScanFlags.NeedNonBurning) != 0 && thing.IsBurning())
						{
							result = false;
						}
						else
						{
							if (this.searcherThing.def.race != null && this.searcherThing.def.race.intelligence >= Intelligence.Humanlike)
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
								bool flag = false;
								CellRect.CellRectIterator iterator = thing.OccupiedRect().GetIterator();
								while (!iterator.Done())
								{
									if (!iterator.Current.Fogged(thing.Map))
									{
										flag = true;
										break;
									}
									iterator.MoveNext();
								}
								if (!flag)
								{
									return false;
								}
							}
							result = true;
						}
					}
				}
				return result;
			}

			internal bool <>m__2(IAttackTarget x)
			{
				return !x.Thing.Position.InHorDistOf(this.searcherThing.Position, this.maxDist) || !this.innerValidator(x);
			}

			internal bool <>m__3(Thing t)
			{
				return this.innerValidator((IAttackTarget)t) && (AttackTargetFinder.CanReach(this.searcherThing, t, this.canBash) || AttackTargetFinder.CanShootAtFromCurrentPosition((IAttackTarget)t, this.searcher, this.verb));
			}

			internal bool <>m__4(Thing t)
			{
				return this.innerValidator((IAttackTarget)t);
			}

			internal bool <>m__5(Thing x)
			{
				return this.innerValidator((IAttackTarget)x);
			}
		}

		[CompilerGenerated]
		private sealed class <BestAttackTarget>c__AnonStorey1
		{
			internal Predicate<IAttackTarget> oldValidator;

			internal AttackTargetFinder.<BestAttackTarget>c__AnonStorey0 <>f__ref$0;

			public <BestAttackTarget>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IAttackTarget t)
			{
				return this.oldValidator(t) && AttackTargetFinder.CanReach(this.<>f__ref$0.searcherThing, t.Thing, this.<>f__ref$0.canBash);
			}
		}

		[CompilerGenerated]
		private sealed class <BestAttackTarget>c__AnonStorey2
		{
			internal Predicate<IAttackTarget> oldValidator;

			internal AttackTargetFinder.<BestAttackTarget>c__AnonStorey0 <>f__ref$0;

			public <BestAttackTarget>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IAttackTarget t)
			{
				return this.oldValidator(t) && t.Thing.Position.InHorDistOf(this.<>f__ref$0.searcherPawn.mindState.duty.focus.Cell, this.<>f__ref$0.searcherPawn.mindState.duty.radius);
			}
		}

		[CompilerGenerated]
		private sealed class <FindBestReachableMeleeTarget>c__AnonStorey3
		{
			internal Pawn searcherPawn;

			internal Predicate<IAttackTarget> validator;

			internal float maxTargDist;

			internal bool canBash;

			internal Func<IntVec3, IAttackTarget> bestTargetOnCell;

			internal IAttackTarget reachableTarget;

			public <FindBestReachableMeleeTarget>c__AnonStorey3()
			{
			}

			internal IAttackTarget <>m__0(IntVec3 x)
			{
				List<Thing> thingList = x.GetThingList(this.searcherPawn.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					IAttackTarget attackTarget = thing as IAttackTarget;
					if (attackTarget != null)
					{
						if (this.validator(attackTarget))
						{
							if (ReachabilityImmediate.CanReachImmediate(x, thing, this.searcherPawn.Map, PathEndMode.Touch, this.searcherPawn))
							{
								if (this.searcherPawn.CanReachImmediate(thing, PathEndMode.Touch) || this.searcherPawn.Map.attackTargetReservationManager.CanReserve(this.searcherPawn, attackTarget))
								{
									return attackTarget;
								}
							}
						}
					}
				}
				return null;
			}

			internal bool <>m__1(IntVec3 x)
			{
				bool result;
				if (!x.Walkable(this.searcherPawn.Map))
				{
					result = false;
				}
				else if ((float)x.DistanceToSquared(this.searcherPawn.Position) > this.maxTargDist * this.maxTargDist)
				{
					result = false;
				}
				else
				{
					if (!this.canBash)
					{
						Building_Door building_Door = x.GetEdifice(this.searcherPawn.Map) as Building_Door;
						if (building_Door != null && !building_Door.CanPhysicallyPass(this.searcherPawn))
						{
							return false;
						}
					}
					result = !PawnUtility.AnyPawnBlockingPathAt(x, this.searcherPawn, true, false, false);
				}
				return result;
			}

			internal bool <>m__2(IntVec3 x)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = x + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(this.searcherPawn.Map))
					{
						IAttackTarget attackTarget = this.bestTargetOnCell(intVec);
						if (attackTarget != null)
						{
							this.reachableTarget = attackTarget;
							break;
						}
					}
				}
				return this.reachableTarget != null;
			}
		}

		[CompilerGenerated]
		private sealed class <FriendlyFireConeTargetScoreOffset>c__AnonStorey4
		{
			internal Map map;

			internal ShotReport report;

			public <FriendlyFireConeTargetScoreOffset>c__AnonStorey4()
			{
			}

			internal bool <>m__0(IntVec3 dest)
			{
				return dest.InBounds(this.map);
			}

			internal ShootLine <>m__1(IntVec3 dest)
			{
				return new ShootLine(this.report.ShootLine.Source, dest);
			}

			internal IEnumerable<IntVec3> <>m__2(ShootLine line)
			{
				return line.Points().Concat(line.Dest).TakeWhile((IntVec3 pos) => pos.CanBeSeenOverFast(this.map));
			}

			internal bool <>m__3(IntVec3 pos)
			{
				return pos.CanBeSeenOverFast(this.map);
			}
		}
	}
}
