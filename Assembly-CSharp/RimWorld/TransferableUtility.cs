using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class TransferableUtility
	{
		private static List<Thing> tmpThings = new List<Thing>();

		public static void Transfer(List<Thing> things, int count, Action<Thing, IThingHolder> transferred)
		{
			if (count > 0)
			{
				TransferableUtility.tmpThings.Clear();
				TransferableUtility.tmpThings.AddRange(things);
				int num = count;
				for (int i = 0; i < TransferableUtility.tmpThings.Count; i++)
				{
					Thing thing = TransferableUtility.tmpThings[i];
					int num2 = Mathf.Min(num, thing.stackCount);
					if (num2 > 0)
					{
						IThingHolder parentHolder = thing.ParentHolder;
						Thing thing2 = thing.SplitOff(num2);
						num -= num2;
						if (thing2 == thing)
						{
							things.Remove(thing);
						}
						transferred(thing2, parentHolder);
						if (num <= 0)
							break;
					}
				}
				TransferableUtility.tmpThings.Clear();
				if (num > 0)
				{
					Log.Error("Can't transfer things because there is nothing left.");
				}
			}
		}

		public static void TransferNoSplit(List<Thing> things, int count, Action<Thing, int> transfer, bool removeIfTakingEntireThing = true, bool errorIfNotEnoughThings = true)
		{
			if (count > 0)
			{
				TransferableUtility.tmpThings.Clear();
				TransferableUtility.tmpThings.AddRange(things);
				int num = count;
				for (int i = 0; i < TransferableUtility.tmpThings.Count; i++)
				{
					Thing thing = TransferableUtility.tmpThings[i];
					int num2 = Mathf.Min(num, thing.stackCount);
					if (num2 > 0)
					{
						num -= num2;
						if (removeIfTakingEntireThing && num2 >= thing.stackCount)
						{
							things.Remove(thing);
						}
						transfer(thing, num2);
						if (num <= 0)
							break;
					}
				}
				TransferableUtility.tmpThings.Clear();
				if (num > 0 && errorIfNotEnoughThings)
				{
					Log.Error("Can't transfer things because there is nothing left.");
				}
			}
		}

		public static bool TransferAsOne(Thing a, Thing b)
		{
			bool result;
			if (a == b)
			{
				result = true;
			}
			else if (a.def.tradeNeverStack || b.def.tradeNeverStack)
			{
				result = false;
			}
			else
			{
				float num = -1f;
				CompRottable compRottable = a.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					num = compRottable.RotProgressPct;
				}
				float num2 = -1f;
				CompRottable compRottable2 = b.TryGetComp<CompRottable>();
				if (compRottable2 != null)
				{
					num2 = compRottable2.RotProgressPct;
				}
				if (Mathf.Abs(num - num2) > 0.10000000149011612)
				{
					result = false;
				}
				else if (a is Corpse && b is Corpse)
				{
					Pawn innerPawn = ((Corpse)a).InnerPawn;
					Pawn innerPawn2 = ((Corpse)b).InnerPawn;
					if (innerPawn.def != innerPawn2.def)
					{
						result = false;
					}
					else if (innerPawn.kindDef != innerPawn2.kindDef)
					{
						result = false;
					}
					else if (innerPawn.RaceProps.Humanlike || innerPawn2.RaceProps.Humanlike)
					{
						result = false;
					}
					else
					{
						if (innerPawn.Name != null && !innerPawn.Name.Numerical)
						{
							goto IL_0150;
						}
						if (innerPawn2.Name != null && !innerPawn2.Name.Numerical)
							goto IL_0150;
						result = true;
					}
				}
				else if (a.def.category == ThingCategory.Pawn)
				{
					if (b.def != a.def)
					{
						result = false;
					}
					else if (a.def.race.Humanlike || b.def.race.Humanlike)
					{
						result = false;
					}
					else
					{
						Pawn pawn = (Pawn)a;
						Pawn pawn2 = (Pawn)b;
						if (pawn.kindDef != pawn2.kindDef)
						{
							result = false;
						}
						else if (pawn.health.summaryHealth.SummaryHealthPercent < 0.99989998340606689 || pawn2.health.summaryHealth.SummaryHealthPercent < 0.99989998340606689)
						{
							result = false;
						}
						else if (pawn.gender != pawn2.gender)
						{
							result = false;
						}
						else
						{
							if (pawn.Name != null && !pawn.Name.Numerical)
							{
								goto IL_0274;
							}
							if (pawn2.Name != null && !pawn2.Name.Numerical)
								goto IL_0274;
							result = ((byte)((pawn.ageTracker.CurLifeStageIndex == pawn2.ageTracker.CurLifeStageIndex) ? ((!(Mathf.Abs(pawn.ageTracker.AgeBiologicalYearsFloat - pawn2.ageTracker.AgeBiologicalYearsFloat) > 1.0)) ? 1 : 0) : 0) != 0);
						}
					}
				}
				else
				{
					Apparel apparel = a as Apparel;
					Apparel apparel2 = b as Apparel;
					QualityCategory qualityCategory = default(QualityCategory);
					QualityCategory qualityCategory2 = default(QualityCategory);
					if (apparel != null && apparel2 != null && apparel.WornByCorpse != apparel2.WornByCorpse)
					{
						result = false;
					}
					else if (a.def.useHitPoints && Mathf.Abs(a.HitPoints - b.HitPoints) >= 10)
					{
						result = false;
					}
					else if (a.TryGetQuality(out qualityCategory) && b.TryGetQuality(out qualityCategory2) && qualityCategory != qualityCategory2)
					{
						result = false;
					}
					else if (a.def.category == ThingCategory.Item)
					{
						result = a.CanStackWith(b);
					}
					else
					{
						Log.Error("Unknown TransferAsOne pair: " + a + ", " + b);
						result = false;
					}
				}
			}
			goto IL_03b8;
			IL_0274:
			result = false;
			goto IL_03b8;
			IL_0150:
			result = false;
			goto IL_03b8;
			IL_03b8:
			return result;
		}

		public static T TransferableMatching<T>(Thing thing, List<T> transferables) where T : Transferable
		{
			T result;
			T val;
			if (thing == null || transferables == null)
			{
				result = (T)null;
			}
			else
			{
				for (int i = 0; i < transferables.Count; i++)
				{
					val = transferables[i];
					if (val.HasAnyThing && TransferableUtility.TransferAsOne(thing, val.AnyThing))
						goto IL_0058;
				}
				result = (T)null;
			}
			goto IL_007c;
			IL_0058:
			result = val;
			goto IL_007c;
			IL_007c:
			return result;
		}

		public static TransferableOneWay TransferableMatchingDesperate(Thing thing, List<TransferableOneWay> transferables)
		{
			TransferableOneWay result;
			TransferableOneWay transferableOneWay;
			TransferableOneWay transferableOneWay2;
			TransferableOneWay transferableOneWay3;
			if (thing == null || transferables == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < transferables.Count; i++)
				{
					transferableOneWay = transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.things.Contains(thing))
						goto IL_0045;
				}
				for (int j = 0; j < transferables.Count; j++)
				{
					transferableOneWay2 = transferables[j];
					if (transferableOneWay2.HasAnyThing && TransferableUtility.TransferAsOne(thing, transferableOneWay2.AnyThing))
						goto IL_0091;
				}
				for (int k = 0; k < transferables.Count; k++)
				{
					transferableOneWay3 = transferables[k];
					if (transferableOneWay3.HasAnyThing && transferableOneWay3.ThingDef == thing.def)
						goto IL_00e0;
				}
				result = null;
			}
			goto IL_0103;
			IL_00e0:
			result = transferableOneWay3;
			goto IL_0103;
			IL_0091:
			result = transferableOneWay2;
			goto IL_0103;
			IL_0045:
			result = transferableOneWay;
			goto IL_0103;
			IL_0103:
			return result;
		}

		public static List<Pawn> GetPawnsFromTransferables(List<TransferableOneWay> transferables)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].CountToTransfer > 0 && transferables[i].AnyThing is Pawn)
				{
					for (int j = 0; j < transferables[i].CountToTransfer; j++)
					{
						Pawn item = (Pawn)transferables[i].things[j];
						list.Add(item);
					}
				}
			}
			return list;
		}

		public static void SimulateTradeableTransfer(List<Thing> all, List<Tradeable> tradeables, List<ThingStackPart> outThingsAfterTransfer)
		{
			outThingsAfterTransfer.Clear();
			for (int i = 0; i < all.Count; i++)
			{
				outThingsAfterTransfer.Add(new ThingStackPart(all[i], all[i].stackCount));
			}
			for (int j = 0; j < tradeables.Count; j++)
			{
				int countToTransfer = tradeables[j].CountToTransfer;
				int num = -countToTransfer;
				if (countToTransfer > 0)
				{
					TransferableUtility.TransferNoSplit(tradeables[j].thingsTrader, countToTransfer, (Action<Thing, int>)delegate(Thing originalThing, int toTake)
					{
						outThingsAfterTransfer.Add(new ThingStackPart(originalThing, toTake));
					}, false, false);
				}
				else if (num > 0)
				{
					TransferableUtility.TransferNoSplit(tradeables[j].thingsColony, num, (Action<Thing, int>)delegate(Thing originalThing, int toTake)
					{
						int num2 = 0;
						ThingStackPart thingStackPart;
						while (true)
						{
							if (num2 < outThingsAfterTransfer.Count)
							{
								thingStackPart = outThingsAfterTransfer[num2];
								if (thingStackPart.Thing != originalThing)
								{
									num2++;
									continue;
								}
								break;
							}
							return;
						}
						outThingsAfterTransfer[num2] = thingStackPart.WithCount(thingStackPart.Count - toTake);
					}, false, false);
				}
			}
		}
	}
}
