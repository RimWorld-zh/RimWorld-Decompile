using System;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	public static class CameraJumper
	{
		public static void TryJumpAndSelect(GlobalTargetInfo target)
		{
			if (target.IsValid)
			{
				CameraJumper.TryJump(target);
				CameraJumper.TrySelect(target);
			}
		}

		public static void TrySelect(GlobalTargetInfo target)
		{
			if (target.IsValid)
			{
				target = CameraJumper.GetAdjustedTarget(target);
				if (target.HasThing)
				{
					CameraJumper.TrySelectInternal(target.Thing);
				}
				else if (target.HasWorldObject)
				{
					CameraJumper.TrySelectInternal(target.WorldObject);
				}
			}
		}

		private static void TrySelectInternal(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (thing.Spawned && thing.def.selectable)
				{
					bool flag = CameraJumper.TryHideWorld();
					bool flag2 = false;
					if (thing.Map != Find.CurrentMap)
					{
						Current.Game.CurrentMap = thing.Map;
						flag2 = true;
						if (!flag)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
					}
					if (flag || flag2)
					{
						Find.CameraDriver.JumpToCurrentMapLoc(thing.Position);
					}
					Find.Selector.ClearSelection();
					Find.Selector.Select(thing, true, true);
				}
			}
		}

		private static void TrySelectInternal(WorldObject worldObject)
		{
			if (Find.World != null)
			{
				if (worldObject.Spawned && worldObject.SelectableNow)
				{
					CameraJumper.TryShowWorld();
					Find.WorldSelector.ClearSelection();
					Find.WorldSelector.Select(worldObject, true);
				}
			}
		}

		public static void TryJump(GlobalTargetInfo target)
		{
			if (target.IsValid)
			{
				target = CameraJumper.GetAdjustedTarget(target);
				if (target.HasThing)
				{
					CameraJumper.TryJumpInternal(target.Thing);
				}
				else if (target.HasWorldObject)
				{
					CameraJumper.TryJumpInternal(target.WorldObject);
				}
				else if (target.Cell.IsValid)
				{
					CameraJumper.TryJumpInternal(target.Cell, target.Map);
				}
				else
				{
					CameraJumper.TryJumpInternal(target.Tile);
				}
			}
		}

		public static void TryJump(IntVec3 cell, Map map)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false));
		}

		public static void TryJump(int tile)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile));
		}

		private static void TryJumpInternal(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Map mapHeld = thing.MapHeld;
				if (mapHeld != null && Find.Maps.Contains(mapHeld) && thing.PositionHeld.IsValid && thing.PositionHeld.InBounds(mapHeld))
				{
					bool flag = CameraJumper.TryHideWorld();
					if (Find.CurrentMap != mapHeld)
					{
						Current.Game.CurrentMap = mapHeld;
						if (!flag)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
					}
					Find.CameraDriver.JumpToCurrentMapLoc(thing.PositionHeld);
				}
			}
		}

		private static void TryJumpInternal(IntVec3 cell, Map map)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (cell.IsValid)
				{
					if (map != null && Find.Maps.Contains(map))
					{
						if (cell.InBounds(map))
						{
							bool flag = CameraJumper.TryHideWorld();
							if (Find.CurrentMap != map)
							{
								Current.Game.CurrentMap = map;
								if (!flag)
								{
									SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
								}
							}
							Find.CameraDriver.JumpToCurrentMapLoc(cell);
						}
					}
				}
			}
		}

		private static void TryJumpInternal(WorldObject worldObject)
		{
			if (Find.World != null)
			{
				if (worldObject.Tile >= 0)
				{
					CameraJumper.TryShowWorld();
					Find.WorldCameraDriver.JumpTo(worldObject.Tile);
				}
			}
		}

		private static void TryJumpInternal(int tile)
		{
			if (Find.World != null)
			{
				if (tile >= 0)
				{
					CameraJumper.TryShowWorld();
					Find.WorldCameraDriver.JumpTo(tile);
				}
			}
		}

		public static bool CanJump(GlobalTargetInfo target)
		{
			bool result;
			if (!target.IsValid)
			{
				result = false;
			}
			else
			{
				target = CameraJumper.GetAdjustedTarget(target);
				if (target.HasThing)
				{
					result = (target.Thing.MapHeld != null && Find.Maps.Contains(target.Thing.MapHeld) && target.Thing.PositionHeld.IsValid && target.Thing.PositionHeld.InBounds(target.Thing.MapHeld));
				}
				else if (target.HasWorldObject)
				{
					result = (target.WorldObject.Tile >= 0);
				}
				else if (target.Cell.IsValid)
				{
					result = (target.Map != null && Find.Maps.Contains(target.Map) && target.Cell.IsValid && target.Cell.InBounds(target.Map));
				}
				else
				{
					result = (target.Tile >= 0);
				}
			}
			return result;
		}

		public static GlobalTargetInfo GetAdjustedTarget(GlobalTargetInfo target)
		{
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					return thing;
				}
				GlobalTargetInfo result = GlobalTargetInfo.Invalid;
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					Thing thing2 = parentHolder as Thing;
					if (thing2 != null && thing2.Spawned)
					{
						result = thing2;
						break;
					}
					ThingComp thingComp = parentHolder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						result = thingComp.parent;
						break;
					}
					WorldObject worldObject = parentHolder as WorldObject;
					if (worldObject != null && worldObject.Spawned)
					{
						result = worldObject;
						break;
					}
				}
				if (result.IsValid)
				{
					return result;
				}
				if (thing.Tile >= 0)
				{
					return new GlobalTargetInfo(thing.Tile);
				}
			}
			else if (target.Cell.IsValid && target.Tile >= 0 && target.Map != null && !Find.Maps.Contains(target.Map))
			{
				MapParent parent = target.Map.Parent;
				if (parent != null && parent.Spawned)
				{
					return parent;
				}
				if (parent != null && parent.Tile >= 0)
				{
					return new GlobalTargetInfo(target.Map.Tile);
				}
				return GlobalTargetInfo.Invalid;
			}
			else if (target.HasWorldObject && !target.WorldObject.Spawned && target.WorldObject.Tile >= 0)
			{
				return new GlobalTargetInfo(target.WorldObject.Tile);
			}
			return target;
		}

		public static GlobalTargetInfo GetWorldTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
			GlobalTargetInfo result;
			if (adjustedTarget.IsValid)
			{
				if (adjustedTarget.IsWorldTarget)
				{
					result = adjustedTarget;
				}
				else
				{
					result = CameraJumper.GetWorldTargetOfMap(adjustedTarget.Map);
				}
			}
			else
			{
				result = GlobalTargetInfo.Invalid;
			}
			return result;
		}

		public static GlobalTargetInfo GetWorldTargetOfMap(Map map)
		{
			GlobalTargetInfo result;
			if (map == null)
			{
				result = GlobalTargetInfo.Invalid;
			}
			else if (map.Parent != null && map.Parent.Spawned)
			{
				result = map.Parent;
			}
			else if (map.Parent != null && map.Parent.Tile >= 0)
			{
				result = new GlobalTargetInfo(map.Tile);
			}
			else
			{
				result = GlobalTargetInfo.Invalid;
			}
			return result;
		}

		public static bool TryHideWorld()
		{
			bool result;
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				result = true;
			}
			else if (Current.ProgramState != ProgramState.Playing)
			{
				result = false;
			}
			else if (Find.World.renderer.wantedMode != WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool TryShowWorld()
		{
			bool result;
			if (WorldRendererUtility.WorldRenderedNow)
			{
				result = true;
			}
			else if (Current.ProgramState != ProgramState.Playing)
			{
				result = false;
			}
			else if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
