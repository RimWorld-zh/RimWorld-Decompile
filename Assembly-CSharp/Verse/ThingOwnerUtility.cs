using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;

namespace Verse
{
	public static class ThingOwnerUtility
	{
		private static Stack<IThingHolder> tmpStack = new Stack<IThingHolder>();

		private static List<IThingHolder> tmpHolders = new List<IThingHolder>();

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
						goto IL_004f;
				}
				result = false;
			}
			goto IL_0073;
			IL_004f:
			result = true;
			goto IL_0073;
			IL_0073:
			return result;
		}

		public static ThingOwner TryGetInnerInteractableThingOwner(this Thing thing)
		{
			IThingHolder thingHolder = thing as IThingHolder;
			ThingWithComps thingWithComps = thing as ThingWithComps;
			ThingOwner result;
			if (thingHolder != null)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				if (directlyHeldThings != null)
				{
					result = directlyHeldThings;
					goto IL_016f;
				}
			}
			ThingOwner directlyHeldThings2;
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					IThingHolder thingHolder2 = allComps[i] as IThingHolder;
					if (thingHolder2 != null)
					{
						directlyHeldThings2 = thingHolder2.GetDirectlyHeldThings();
						if (directlyHeldThings2 != null)
							goto IL_006b;
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			if (thingHolder != null)
			{
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				if (ThingOwnerUtility.tmpHolders.Any())
				{
					ThingOwner directlyHeldThings3 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
					if (directlyHeldThings3 != null)
					{
						result = directlyHeldThings3;
						goto IL_016f;
					}
				}
			}
			ThingOwner directlyHeldThings4;
			if (thingWithComps != null)
			{
				List<ThingComp> allComps2 = thingWithComps.AllComps;
				for (int j = 0; j < allComps2.Count; j++)
				{
					IThingHolder thingHolder3 = allComps2[j] as IThingHolder;
					if (thingHolder3 != null)
					{
						thingHolder3.GetChildHolders(ThingOwnerUtility.tmpHolders);
						if (ThingOwnerUtility.tmpHolders.Any())
						{
							directlyHeldThings4 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
							if (directlyHeldThings4 != null)
								goto IL_013e;
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			result = null;
			goto IL_016f;
			IL_016f:
			return result;
			IL_013e:
			result = directlyHeldThings4;
			goto IL_016f;
			IL_006b:
			result = directlyHeldThings2;
			goto IL_016f;
		}

		public static bool SpawnedOrAnyParentSpawned(IThingHolder holder)
		{
			bool result;
			while (true)
			{
				if (holder != null)
				{
					Thing thing = holder as Thing;
					if (thing != null && thing.Spawned)
					{
						result = true;
						break;
					}
					ThingComp thingComp = holder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						result = true;
						break;
					}
					holder = holder.ParentHolder;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

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

		public static Map GetRootMap(IThingHolder holder)
		{
			Map result;
			while (true)
			{
				if (holder != null)
				{
					Map map = holder as Map;
					if (map != null)
					{
						result = map;
						break;
					}
					holder = holder.ParentHolder;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static int GetRootTile(IThingHolder holder)
		{
			int result;
			while (true)
			{
				if (holder != null)
				{
					WorldObject worldObject = holder as WorldObject;
					if (worldObject != null && worldObject.Tile >= 0)
					{
						result = worldObject.Tile;
						break;
					}
					holder = holder.ParentHolder;
					continue;
				}
				result = -1;
				break;
			}
			return result;
		}

		public static bool IsEnclosingContainer(this IThingHolder holder)
		{
			return holder != null && !(holder is Pawn_CarryTracker) && !(holder is Corpse) && !(holder is Map) && !(holder is Caravan) && !(holder is Settlement_TraderTracker) && !(holder is TradeShip);
		}

		public static bool ShouldAutoRemoveDestroyedThings(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is Caravan);
		}

		public static bool ShouldAutoExtinguishInnerThings(IThingHolder holder)
		{
			return !(holder is Map);
		}

		public static bool ShouldRemoveDesignationsOnAddedThings(IThingHolder holder)
		{
			return holder.IsEnclosingContainer();
		}

		public static void AppendThingHoldersFromThings(List<IThingHolder> outThingsHolders, ThingOwner container)
		{
			if (container != null)
			{
				int num = 0;
				int count = container.Count;
				while (num < count)
				{
					IThingHolder thingHolder = container[num] as IThingHolder;
					if (thingHolder != null)
					{
						outThingsHolders.Add(thingHolder);
					}
					ThingWithComps thingWithComps = container[num] as ThingWithComps;
					if (thingWithComps != null)
					{
						List<ThingComp> allComps = thingWithComps.AllComps;
						for (int i = 0; i < allComps.Count; i++)
						{
							IThingHolder thingHolder2 = allComps[i] as IThingHolder;
							if (thingHolder2 != null)
							{
								outThingsHolders.Add(thingHolder2);
							}
						}
					}
					num++;
				}
			}
		}

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
						goto IL_002b;
				}
				result = false;
			}
			goto IL_0047;
			IL_002b:
			result = true;
			goto IL_0047;
			IL_0047:
			return result;
		}

		public static void GetAllThingsRecursively(IThingHolder holder, List<Thing> outThings, bool allowUnreal = true)
		{
			outThings.Clear();
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
					ThingOwnerUtility.tmpStack.Push(ThingOwnerUtility.tmpHolders[i]);
				}
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpHolders.Clear();
		}

		public static List<Thing> GetAllThingsRecursively(IThingHolder holder, bool allowUnreal = true)
		{
			List<Thing> list = new List<Thing>();
			ThingOwnerUtility.GetAllThingsRecursively(holder, list, allowUnreal);
			return list;
		}

		public static bool AreImmediateContentsReal(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is MinifiedThing);
		}

		public static bool ContentsFrozen(IThingHolder holder)
		{
			return holder is Building_CryptosleepCasket;
		}

		public static bool TryGetFixedTemperature(IThingHolder holder, out float temperature)
		{
			bool result;
			if (holder is Pawn_InventoryTracker)
			{
				temperature = 14f;
				result = true;
			}
			else if (holder is CompLaunchable || holder is ActiveDropPodInfo || holder is TravelingTransportPods)
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
	}
}
