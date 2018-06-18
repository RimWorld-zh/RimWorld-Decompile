using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000DFB RID: 3579
	public static class ThingOwnerUtility
	{
		// Token: 0x060050C0 RID: 20672 RVA: 0x00298A18 File Offset: 0x00296E18
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

		// Token: 0x060050C1 RID: 20673 RVA: 0x00298A9C File Offset: 0x00296E9C
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

		// Token: 0x060050C2 RID: 20674 RVA: 0x00298C1C File Offset: 0x0029701C
		public static bool SpawnedOrAnyParentSpawned(IThingHolder holder)
		{
			return ThingOwnerUtility.SpawnedParentOrMe(holder) != null;
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x00298C40 File Offset: 0x00297040
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

		// Token: 0x060050C4 RID: 20676 RVA: 0x00298CB4 File Offset: 0x002970B4
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

		// Token: 0x060050C5 RID: 20677 RVA: 0x00298D48 File Offset: 0x00297148
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

		// Token: 0x060050C6 RID: 20678 RVA: 0x00298D88 File Offset: 0x00297188
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

		// Token: 0x060050C7 RID: 20679 RVA: 0x00298DD8 File Offset: 0x002971D8
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

		// Token: 0x060050C8 RID: 20680 RVA: 0x00298E24 File Offset: 0x00297224
		public static bool IsEnclosingContainer(this IThingHolder holder)
		{
			return holder != null && !(holder is Pawn_CarryTracker) && !(holder is Corpse) && !(holder is Map) && !(holder is Caravan) && !(holder is Settlement_TraderTracker) && !(holder is TradeShip);
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x00298E88 File Offset: 0x00297288
		public static bool ShouldAutoRemoveDestroyedThings(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is Caravan);
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x00298EB8 File Offset: 0x002972B8
		public static bool ShouldAutoExtinguishInnerThings(IThingHolder holder)
		{
			return !(holder is Map);
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x00298EDC File Offset: 0x002972DC
		public static bool ShouldRemoveDesignationsOnAddedThings(IThingHolder holder)
		{
			return holder.IsEnclosingContainer();
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x00298EF8 File Offset: 0x002972F8
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

		// Token: 0x060050CD RID: 20685 RVA: 0x00298FA0 File Offset: 0x002973A0
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

		// Token: 0x060050CE RID: 20686 RVA: 0x00298FF8 File Offset: 0x002973F8
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

		// Token: 0x060050CF RID: 20687 RVA: 0x002990FC File Offset: 0x002974FC
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

		// Token: 0x060050D0 RID: 20688 RVA: 0x00299230 File Offset: 0x00297630
		public static List<Thing> GetAllThingsRecursively(IThingHolder holder, bool allowUnreal = true)
		{
			List<Thing> list = new List<Thing>();
			ThingOwnerUtility.GetAllThingsRecursively(holder, list, allowUnreal, null);
			return list;
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x00299258 File Offset: 0x00297658
		public static bool AreImmediateContentsReal(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is MinifiedThing);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x00299288 File Offset: 0x00297688
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

		// Token: 0x04003531 RID: 13617
		private static Stack<IThingHolder> tmpStack = new Stack<IThingHolder>();

		// Token: 0x04003532 RID: 13618
		private static List<IThingHolder> tmpHolders = new List<IThingHolder>();

		// Token: 0x04003533 RID: 13619
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04003534 RID: 13620
		private static List<IThingHolder> tmpMapChildHolders = new List<IThingHolder>();
	}
}
