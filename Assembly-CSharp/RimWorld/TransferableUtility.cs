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
			if (a == b)
			{
				return true;
			}
			if (!a.def.tradeNeverStack && !b.def.tradeNeverStack)
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
					return false;
				}
				if (a is Corpse && b is Corpse)
				{
					Pawn innerPawn = ((Corpse)a).InnerPawn;
					Pawn innerPawn2 = ((Corpse)b).InnerPawn;
					if (innerPawn.def != innerPawn2.def)
					{
						return false;
					}
					if (innerPawn.kindDef != innerPawn2.kindDef)
					{
						return false;
					}
					if (!innerPawn.RaceProps.Humanlike && !innerPawn2.RaceProps.Humanlike)
					{
						if (innerPawn.Name != null && !innerPawn.Name.Numerical)
						{
							goto IL_012b;
						}
						if (innerPawn2.Name != null && !innerPawn2.Name.Numerical)
							goto IL_012b;
						return true;
					}
					return false;
				}
				if (a.def.category == ThingCategory.Pawn)
				{
					if (b.def != a.def)
					{
						return false;
					}
					if (!a.def.race.Humanlike && !b.def.race.Humanlike)
					{
						Pawn pawn = (Pawn)a;
						Pawn pawn2 = (Pawn)b;
						if (pawn.kindDef != pawn2.kindDef)
						{
							return false;
						}
						if (!(pawn.health.summaryHealth.SummaryHealthPercent < 0.99989998340606689) && !(pawn2.health.summaryHealth.SummaryHealthPercent < 0.99989998340606689))
						{
							if (pawn.gender != pawn2.gender)
							{
								return false;
							}
							if (pawn.Name != null && !pawn.Name.Numerical)
							{
								goto IL_022b;
							}
							if (pawn2.Name != null && !pawn2.Name.Numerical)
								goto IL_022b;
							if (pawn.ageTracker.CurLifeStageIndex != pawn2.ageTracker.CurLifeStageIndex)
							{
								return false;
							}
							if (Mathf.Abs(pawn.ageTracker.AgeBiologicalYearsFloat - pawn2.ageTracker.AgeBiologicalYearsFloat) > 1.0)
							{
								return false;
							}
							return true;
						}
						return false;
					}
					return false;
				}
				Apparel apparel = a as Apparel;
				Apparel apparel2 = b as Apparel;
				if (apparel != null && apparel2 != null && apparel.WornByCorpse != apparel2.WornByCorpse)
				{
					return false;
				}
				if (a.def.useHitPoints && Mathf.Abs(a.HitPoints - b.HitPoints) >= 10)
				{
					return false;
				}
				QualityCategory qualityCategory = default(QualityCategory);
				QualityCategory qualityCategory2 = default(QualityCategory);
				if (a.TryGetQuality(out qualityCategory) && b.TryGetQuality(out qualityCategory2) && qualityCategory != qualityCategory2)
				{
					return false;
				}
				if (a.def.category == ThingCategory.Item)
				{
					return a.CanStackWith(b);
				}
				Log.Error("Unknown TransferAsOne pair: " + a + ", " + b);
				return false;
			}
			return false;
			IL_022b:
			return false;
			IL_012b:
			return false;
		}

		public static T TransferableMatching<T>(Thing thing, List<T> transferables) where T : Transferable
		{
			if (thing != null && transferables != null)
			{
				for (int i = 0; i < transferables.Count; i++)
				{
					T result = transferables[i];
					if (result.HasAnyThing && TransferableUtility.TransferAsOne(thing, result.AnyThing))
					{
						return result;
					}
				}
				return (T)null;
			}
			return (T)null;
		}

		public static TransferableOneWay TransferableMatchingDesperate(Thing thing, List<TransferableOneWay> transferables)
		{
			if (thing != null && transferables != null)
			{
				for (int i = 0; i < transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.things.Contains(thing))
					{
						return transferableOneWay;
					}
				}
				for (int j = 0; j < transferables.Count; j++)
				{
					TransferableOneWay transferableOneWay2 = transferables[j];
					if (transferableOneWay2.HasAnyThing && TransferableUtility.TransferAsOne(thing, transferableOneWay2.AnyThing))
					{
						return transferableOneWay2;
					}
				}
				for (int k = 0; k < transferables.Count; k++)
				{
					TransferableOneWay transferableOneWay3 = transferables[k];
					if (transferableOneWay3.HasAnyThing && transferableOneWay3.ThingDef == thing.def)
					{
						return transferableOneWay3;
					}
				}
				return null;
			}
			return null;
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
