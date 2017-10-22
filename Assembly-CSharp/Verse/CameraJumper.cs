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
			if (Current.ProgramState == ProgramState.Playing && thing.Spawned && thing.def.selectable)
			{
				bool flag = CameraJumper.TryHideWorld();
				bool flag2 = false;
				if (thing.Map != Current.Game.VisibleMap)
				{
					Current.Game.VisibleMap = thing.Map;
					flag2 = true;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				if (flag || flag2)
				{
					Find.CameraDriver.JumpToVisibleMapLoc(thing.Position);
				}
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
		}

		private static void TrySelectInternal(WorldObject worldObject)
		{
			if (Find.World != null && worldObject.Spawned && worldObject.SelectableNow)
			{
				CameraJumper.TryShowWorld();
				Find.WorldSelector.ClearSelection();
				Find.WorldSelector.Select(worldObject, true);
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
				if (mapHeld != null && thing.PositionHeld.IsValid && thing.PositionHeld.InBounds(mapHeld))
				{
					bool flag = CameraJumper.TryHideWorld();
					if (Current.Game.VisibleMap != mapHeld)
					{
						Current.Game.VisibleMap = mapHeld;
						if (!flag)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
					}
					Find.CameraDriver.JumpToVisibleMapLoc(thing.PositionHeld);
				}
			}
		}

		private static void TryJumpInternal(IntVec3 cell, Map map)
		{
			if (Current.ProgramState == ProgramState.Playing && cell.IsValid && map != null && Find.Maps.Contains(map))
			{
				bool flag = CameraJumper.TryHideWorld();
				if (Current.Game.VisibleMap != map)
				{
					Current.Game.VisibleMap = map;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				Find.CameraDriver.JumpToVisibleMapLoc(cell);
			}
		}

		private static void TryJumpInternal(WorldObject worldObject)
		{
			if (Find.World != null && worldObject.Tile >= 0)
			{
				CameraJumper.TryShowWorld();
				Find.WorldCameraDriver.JumpTo(worldObject.Tile);
			}
		}

		private static void TryJumpInternal(int tile)
		{
			if (Find.World != null && tile >= 0)
			{
				CameraJumper.TryShowWorld();
				Find.WorldCameraDriver.JumpTo(tile);
			}
		}

		public static GlobalTargetInfo GetAdjustedTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo result;
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					result = thing;
					goto IL_0205;
				}
				GlobalTargetInfo globalTargetInfo = GlobalTargetInfo.Invalid;
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					Thing thing2 = parentHolder as Thing;
					if (thing2 != null && thing2.Spawned)
					{
						globalTargetInfo = thing2;
						break;
					}
					ThingComp thingComp = parentHolder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						globalTargetInfo = (Thing)thingComp.parent;
						break;
					}
					WorldObject worldObject = parentHolder as WorldObject;
					if (worldObject != null && worldObject.Spawned)
					{
						globalTargetInfo = worldObject;
						break;
					}
				}
				if (globalTargetInfo.IsValid)
				{
					result = globalTargetInfo;
					goto IL_0205;
				}
				if (thing.Tile >= 0)
				{
					result = new GlobalTargetInfo(thing.Tile);
					goto IL_0205;
				}
			}
			else
			{
				if (target.Cell.IsValid && target.Tile >= 0 && target.Map != null && !Find.Maps.Contains(target.Map))
				{
					MapParent parent = target.Map.info.parent;
					result = ((parent == null || !parent.Spawned) ? ((parent == null || parent.Tile < 0) ? GlobalTargetInfo.Invalid : new GlobalTargetInfo(target.Map.Tile)) : ((WorldObject)parent));
					goto IL_0205;
				}
				if (target.HasWorldObject && !target.WorldObject.Spawned && target.WorldObject.Tile >= 0)
				{
					result = new GlobalTargetInfo(target.WorldObject.Tile);
					goto IL_0205;
				}
			}
			result = target;
			goto IL_0205;
			IL_0205:
			return result;
		}

		public static GlobalTargetInfo GetWorldTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
			return (!adjustedTarget.IsValid) ? GlobalTargetInfo.Invalid : ((!adjustedTarget.IsWorldTarget) ? CameraJumper.GetWorldTargetOfMap(adjustedTarget.Map) : adjustedTarget);
		}

		public static GlobalTargetInfo GetWorldTargetOfMap(Map map)
		{
			return (map != null) ? ((map.info.parent == null || !map.info.parent.Spawned) ? ((map.info.parent == null || map.info.parent.Tile < 0) ? GlobalTargetInfo.Invalid : new GlobalTargetInfo(map.Tile)) : ((WorldObject)map.info.parent)) : GlobalTargetInfo.Invalid;
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
			else if (Find.World.renderer.wantedMode != 0)
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
