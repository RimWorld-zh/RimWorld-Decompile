using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082C RID: 2092
	public static class TransferableUtility
	{
		// Token: 0x0400195A RID: 6490
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06002F0B RID: 12043 RVA: 0x001923B0 File Offset: 0x001907B0
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
						{
							break;
						}
					}
				}
				TransferableUtility.tmpThings.Clear();
				if (num > 0)
				{
					Log.Error("Can't transfer things because there is nothing left.", false);
				}
			}
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x0019247C File Offset: 0x0019087C
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
						{
							break;
						}
					}
				}
				TransferableUtility.tmpThings.Clear();
				if (num > 0 && errorIfNotEnoughThings)
				{
					Log.Error("Can't transfer things because there is nothing left.", false);
				}
			}
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x00192544 File Offset: 0x00190944
		public static bool TransferAsOne(Thing a, Thing b, TransferAsOneMode mode)
		{
			bool result;
			if (a == b)
			{
				result = true;
			}
			else if (a.def != b.def)
			{
				result = false;
			}
			else
			{
				a = a.GetInnerIfMinified();
				b = b.GetInnerIfMinified();
				if (a.def.tradeNeverStack || b.def.tradeNeverStack)
				{
					result = false;
				}
				else if (!TransferableUtility.CanStack(a) || !TransferableUtility.CanStack(b))
				{
					result = false;
				}
				else if (a.def != b.def || a.Stuff != b.Stuff)
				{
					result = false;
				}
				else
				{
					if (mode == TransferAsOneMode.PodsOrCaravanPacking)
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
						if (Mathf.Abs(num - num2) > 0.1f)
						{
							return false;
						}
					}
					if (a is Corpse && b is Corpse)
					{
						Pawn innerPawn = ((Corpse)a).InnerPawn;
						Pawn innerPawn2 = ((Corpse)b).InnerPawn;
						result = (innerPawn.def == innerPawn2.def && innerPawn.kindDef == innerPawn2.kindDef && !innerPawn.RaceProps.Humanlike && !innerPawn2.RaceProps.Humanlike && (innerPawn.Name == null || innerPawn.Name.Numerical) && (innerPawn2.Name == null || innerPawn2.Name.Numerical));
					}
					else if (a.def.category == ThingCategory.Pawn)
					{
						if (b.def != a.def)
						{
							result = false;
						}
						else
						{
							Pawn pawn = (Pawn)a;
							Pawn pawn2 = (Pawn)b;
							result = (pawn.kindDef == pawn2.kindDef && pawn.gender == pawn2.gender && pawn.ageTracker.CurLifeStageIndex == pawn2.ageTracker.CurLifeStageIndex && Mathf.Abs(pawn.ageTracker.AgeBiologicalYearsFloat - pawn2.ageTracker.AgeBiologicalYearsFloat) <= 1f);
						}
					}
					else
					{
						Apparel apparel = a as Apparel;
						Apparel apparel2 = b as Apparel;
						if (apparel != null && apparel2 != null)
						{
							if (apparel.WornByCorpse != apparel2.WornByCorpse)
							{
								return false;
							}
						}
						if (mode != TransferAsOneMode.InactiveTradeable && a.def.useHitPoints && Mathf.Abs(a.HitPoints - b.HitPoints) >= 10)
						{
							result = false;
						}
						else
						{
							QualityCategory qualityCategory;
							QualityCategory qualityCategory2;
							if (a.TryGetQuality(out qualityCategory) && b.TryGetQuality(out qualityCategory2))
							{
								if (qualityCategory != qualityCategory2)
								{
									return false;
								}
							}
							if (a.def.category == ThingCategory.Item)
							{
								result = a.CanStackWith(b);
							}
							else if (a.def.category == ThingCategory.Building)
							{
								result = true;
							}
							else
							{
								Log.Error(string.Concat(new object[]
								{
									"Unknown TransferAsOne pair: ",
									a,
									", ",
									b
								}), false);
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x001928F4 File Offset: 0x00190CF4
		public static bool CanStack(Thing thing)
		{
			if (thing.def.category == ThingCategory.Pawn)
			{
				if (thing.def.race.Humanlike)
				{
					return false;
				}
				Pawn pawn = (Pawn)thing;
				if (pawn.health.summaryHealth.SummaryHealthPercent < 0.9999f)
				{
					return false;
				}
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					return false;
				}
				if (pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
				{
					return false;
				}
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x001929C4 File Offset: 0x00190DC4
		public static T TransferableMatching<T>(Thing thing, List<T> transferables, TransferAsOneMode mode) where T : Transferable
		{
			T result;
			if (thing == null || transferables == null)
			{
				result = (T)((object)null);
			}
			else
			{
				for (int i = 0; i < transferables.Count; i++)
				{
					T result2 = transferables[i];
					if (result2.HasAnyThing)
					{
						if (TransferableUtility.TransferAsOne(thing, result2.AnyThing, mode))
						{
							return result2;
						}
					}
				}
				result = (T)((object)null);
			}
			return result;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x00192A50 File Offset: 0x00190E50
		public static Tradeable TradeableMatching(Thing thing, List<Tradeable> tradeables)
		{
			Tradeable result;
			if (thing == null || tradeables == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < tradeables.Count; i++)
				{
					Tradeable tradeable = tradeables[i];
					if (tradeable.HasAnyThing)
					{
						TransferAsOneMode mode = (!tradeable.TraderWillTrade) ? TransferAsOneMode.InactiveTradeable : TransferAsOneMode.Normal;
						if (TransferableUtility.TransferAsOne(thing, tradeable.AnyThing, mode))
						{
							return tradeable;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00192AD8 File Offset: 0x00190ED8
		public static TransferableOneWay TransferableMatchingDesperate(Thing thing, List<TransferableOneWay> transferables, TransferAsOneMode mode)
		{
			TransferableOneWay result;
			if (thing == null || transferables == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = transferables[i];
					if (transferableOneWay.HasAnyThing)
					{
						if (transferableOneWay.things.Contains(thing))
						{
							return transferableOneWay;
						}
					}
				}
				for (int j = 0; j < transferables.Count; j++)
				{
					TransferableOneWay transferableOneWay2 = transferables[j];
					if (transferableOneWay2.HasAnyThing)
					{
						if (TransferableUtility.TransferAsOne(thing, transferableOneWay2.AnyThing, mode))
						{
							return transferableOneWay2;
						}
					}
				}
				for (int k = 0; k < transferables.Count; k++)
				{
					TransferableOneWay transferableOneWay3 = transferables[k];
					if (transferableOneWay3.HasAnyThing)
					{
						if (transferableOneWay3.ThingDef == thing.def)
						{
							return transferableOneWay3;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x00192BEC File Offset: 0x00190FEC
		public static List<Pawn> GetPawnsFromTransferables(List<TransferableOneWay> transferables)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].CountToTransfer > 0)
				{
					if (transferables[i].AnyThing is Pawn)
					{
						for (int j = 0; j < transferables[i].CountToTransfer; j++)
						{
							Pawn item = (Pawn)transferables[i].things[j];
							list.Add(item);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x00192C94 File Offset: 0x00191094
		public static void SimulateTradeableTransfer(List<Thing> all, List<Tradeable> tradeables, List<ThingCount> outThingsAfterTransfer)
		{
			outThingsAfterTransfer.Clear();
			for (int i = 0; i < all.Count; i++)
			{
				outThingsAfterTransfer.Add(new ThingCount(all[i], all[i].stackCount));
			}
			for (int j = 0; j < tradeables.Count; j++)
			{
				int countToTransferToSource = tradeables[j].CountToTransferToSource;
				int countToTransferToDestination = tradeables[j].CountToTransferToDestination;
				if (countToTransferToSource > 0)
				{
					TransferableUtility.TransferNoSplit(tradeables[j].thingsTrader, countToTransferToSource, delegate(Thing originalThing, int toTake)
					{
						outThingsAfterTransfer.Add(new ThingCount(originalThing, toTake));
					}, false, false);
				}
				else if (countToTransferToDestination > 0)
				{
					TransferableUtility.TransferNoSplit(tradeables[j].thingsColony, countToTransferToDestination, delegate(Thing originalThing, int toTake)
					{
						for (int k = 0; k < outThingsAfterTransfer.Count; k++)
						{
							ThingCount thingCount = outThingsAfterTransfer[k];
							if (thingCount.Thing == originalThing)
							{
								outThingsAfterTransfer[k] = thingCount.WithCount(thingCount.Count - toTake);
								break;
							}
						}
					}, false, false);
				}
			}
		}
	}
}
