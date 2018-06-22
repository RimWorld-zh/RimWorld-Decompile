using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000DF8 RID: 3576
	public static class ThingOwnerUtility
	{
		// Token: 0x060050D4 RID: 20692 RVA: 0x00299FF4 File Offset: 0x002983F4
		public static bool ThisOrAnyCompIsThingHolder(this ThingDef thingDef)
		{
			bool result;
			if (typeof(IThingHolder).IsAssignableFrom(thingDef.thingClass))
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < thingDef.comps.Count; i++)
				{
					if (typeof(IThingHolder).IsAssignableFrom(thingDef.comps[i].compClass))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x0029A078 File Offset: 0x00298478
		public static ThingOwner TryGetInnerInteractableThingOwner(this Thing thing)
		{
			IThingHolder thingHolder = thing as IThingHolder;
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingHolder != null)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				if (directlyHeldThings != null)
				{
					return directlyHeldThings;
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					IThingHolder thingHolder2 = allComps[i] as IThingHolder;
					if (thingHolder2 != null)
					{
						ThingOwner directlyHeldThings2 = thingHolder2.GetDirectlyHeldThings();
						if (directlyHeldThings2 != null)
						{
							return directlyHeldThings2;
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			if (thingHolder != null)
			{
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
				{
					ThingOwner directlyHeldThings3 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
					if (directlyHeldThings3 != null)
					{
						return directlyHeldThings3;
					}
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps2 = thingWithComps.AllComps;
				for (int j = 0; j < allComps2.Count; j++)
				{
					IThingHolder thingHolder3 = allComps2[j] as IThingHolder;
					if (thingHolder3 != null)
					{
						thingHolder3.GetChildHolders(ThingOwnerUtility.tmpHolders);
						if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
						{
							ThingOwner directlyHeldThings4 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
							if (directlyHeldThings4 != null)
							{
								return directlyHeldThings4;
							}
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			return null;
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x0029A1F8 File Offset: 0x002985F8
		public static bool SpawnedOrAnyParentSpawned(IThingHolder holder)
		{
			return ThingOwnerUtility.SpawnedParentOrMe(holder) != null;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x0029A21C File Offset: 0x0029861C
		public static Thing SpawnedParentOrMe(IThingHolder holder)
		{
			while (holder != null)
			{
				Thing thing = holder as Thing;
				Thing result;
				if (thing != null && thing.Spawned)
				{
					result = thing;
				}
				else
				{
					ThingComp thingComp = holder as ThingComp;
					if (thingComp == null || !thingComp.parent.Spawned)
					{
						holder = holder.ParentHolder;
						continue;
					}
					result = thingComp.parent;
				}
				return result;
			}
			return null;
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x0029A290 File Offset: 0x00298690
		public static IntVec3 GetRootPosition(IThingHolder holder)
		{
			IntVec3 result = IntVec3.Invalid;
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Position.IsValid)
				{
					result = thing.Position;
				}
				else
				{
					ThingComp thingComp = holder as ThingComp;
					if (thingComp != null && thingComp.parent.Position.IsValid)
					{
						result = thingComp.parent.Position;
					}
				}
				holder = holder.ParentHolder;
			}
			return result;
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x0029A324 File Offset: 0x00298724
		public static Map GetRootMap(IThingHolder holder)
		{
			while (holder != null)
			{
				Map map = holder as Map;
				if (map != null)
				{
					return map;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x0029A364 File Offset: 0x00298764
		public static int GetRootTile(IThingHolder holder)
		{
			while (holder != null)
			{
				WorldObject worldObject = holder as WorldObject;
				if (worldObject != null && worldObject.Tile >= 0)
				{
					return worldObject.Tile;
				}
				holder = holder.ParentHolder;
			}
			return -1;
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x0029A3B4 File Offset: 0x002987B4
		public static bool ContentsSuspended(IThingHolder holder)
		{
			while (holder != null)
			{
				if (holder is Building_CryptosleepCasket || holder is ImportantPawnComp)
				{
					return true;
				}
				holder = holder.ParentHolder;
			}
			return false;
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x0029A400 File Offset: 0x00298800
		public static bool IsEnclosingContainer(this IThingHolder holder)
		{
			return holder != null && !(holder is Pawn_CarryTracker) && !(holder is Corpse) && !(holder is Map) && !(holder is Caravan) && !(holder is Settlement_TraderTracker) && !(holder is TradeShip);
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x0029A464 File Offset: 0x00298864
		public static bool ShouldAutoRemoveDestroyedThings(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is Caravan);
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x0029A494 File Offset: 0x00298894
		public static bool ShouldAutoExtinguishInnerThings(IThingHolder holder)
		{
			return !(holder is Map);
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x0029A4B8 File Offset: 0x002988B8
		public static bool ShouldRemoveDesignationsOnAddedThings(IThingHolder holder)
		{
			return holder.IsEnclosingContainer();
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x0029A4D4 File Offset: 0x002988D4
		public static void AppendThingHoldersFromThings(List<IThingHolder> outThingsHolders, IList<Thing> container)
		{
			if (container != null)
			{
				int i = 0;
				int count = container.Count;
				while (i < count)
				{
					IThingHolder thingHolder = container[i] as IThingHolder;
					if (thingHolder != null)
					{
						outThingsHolders.Add(thingHolder);
					}
					ThingWithComps thingWithComps = container[i] as ThingWithComps;
					if (thingWithComps != null)
					{
						List<ThingComp> allComps = thingWithComps.AllComps;
						for (int j = 0; j < allComps.Count; j++)
						{
							IThingHolder thingHolder2 = allComps[j] as IThingHolder;
							if (thingHolder2 != null)
							{
								outThingsHolders.Add(thingHolder2);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x0029A57C File Offset: 0x0029897C
		public static bool AnyParentIs<T>(Thing thing) where T : IThingHolder
		{
			bool result;
			if (thing is T)
			{
				result = true;
			}
			else
			{
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					if (parentHolder is T)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x0029A5D4 File Offset: 0x002989D4
		public static void GetAllThingsRecursively(IThingHolder holder, List<Thing> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null)
		{
			outThings.Clear();
			if (passCheck == null || passCheck(holder))
			{
				ThingOwnerUtility.tmpStack.Clear();
				ThingOwnerUtility.tmpStack.Push(holder);
				while (ThingOwnerUtility.tmpStack.Count != 0)
				{
					IThingHolder thingHolder = ThingOwnerUtility.tmpStack.Pop();
					if (allowUnreal || ThingOwnerUtility.AreImmediateContentsReal(thingHolder))
					{
						ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
						if (directlyHeldThings != null)
						{
							outThings.AddRange(directlyHeldThings);
						}
					}
					ThingOwnerUtility.tmpHolders.Clear();
					thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
					for (int i = 0; i < ThingOwnerUtility.tmpHolders.Count; i++)
					{
						if (passCheck == null || passCheck(ThingOwnerUtility.tmpHolders[i]))
						{
							ThingOwnerUtility.tmpStack.Push(ThingOwnerUtility.tmpHolders[i]);
						}
					}
				}
				ThingOwnerUtility.tmpStack.Clear();
				ThingOwnerUtility.tmpHolders.Clear();
			}
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x0029A6D8 File Offset: 0x00298AD8
		public static void GetAllThingsRecursively<T>(Map map, ThingRequest request, List<T> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null, bool alsoGetSpawnedThings = true) where T : Thing
		{
			outThings.Clear();
			if (alsoGetSpawnedThings)
			{
				List<Thing> list = map.listerThings.ThingsMatching(request);
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i] as T;
					if (t != null)
					{
						outThings.Add(t);
					}
				}
			}
			ThingOwnerUtility.tmpMapChildHolders.Clear();
			map.GetChildHolders(ThingOwnerUtility.tmpMapChildHolders);
			for (int j = 0; j < ThingOwnerUtility.tmpMapChildHolders.Count; j++)
			{
				ThingOwnerUtility.tmpThings.Clear();
				ThingOwnerUtility.GetAllThingsRecursively(ThingOwnerUtility.tmpMapChildHolders[j], ThingOwnerUtility.tmpThings, allowUnreal, passCheck);
				for (int k = 0; k < ThingOwnerUtility.tmpThings.Count; k++)
				{
					if (request.Accepts(ThingOwnerUtility.tmpThings[k]))
					{
						T t2 = ThingOwnerUtility.tmpThings[k] as T;
						if (t2 != null)
						{
							outThings.Add(t2);
						}
					}
				}
			}
			ThingOwnerUtility.tmpMapChildHolders.Clear();
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x0029A80C File Offset: 0x00298C0C
		public static List<Thing> GetAllThingsRecursively(IThingHolder holder, bool allowUnreal = true)
		{
			List<Thing> list = new List<Thing>();
			ThingOwnerUtility.GetAllThingsRecursively(holder, list, allowUnreal, null);
			return list;
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x0029A834 File Offset: 0x00298C34
		public static bool AreImmediateContentsReal(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is MinifiedThing);
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x0029A864 File Offset: 0x00298C64
		public static bool TryGetFixedTemperature(IThingHolder holder, Thing forThing, out float temperature)
		{
			if (holder is Pawn_InventoryTracker)
			{
				if (forThing.TryGetComp<CompHatcher>() != null)
				{
					temperature = 14f;
					return true;
				}
			}
			bool result;
			if (holder is CompLaunchable || holder is ActiveDropPodInfo || holder is TravelingTransportPods)
			{
				temperature = 14f;
				result = true;
			}
			else if (holder is Settlement_TraderTracker || holder is TradeShip)
			{
				temperature = 14f;
				result = true;
			}
			else
			{
				temperature = 21f;
				result = false;
			}
			return result;
		}

		// Token: 0x04003538 RID: 13624
		private static Stack<IThingHolder> tmpStack = new Stack<IThingHolder>();

		// Token: 0x04003539 RID: 13625
		private static List<IThingHolder> tmpHolders = new List<IThingHolder>();

		// Token: 0x0400353A RID: 13626
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400353B RID: 13627
		private static List<IThingHolder> tmpMapChildHolders = new List<IThingHolder>();
	}
}
