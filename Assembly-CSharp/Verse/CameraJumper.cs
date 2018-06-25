using System;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000AE4 RID: 2788
	public static class CameraJumper
	{
		// Token: 0x06003DC4 RID: 15812 RVA: 0x00209837 File Offset: 0x00207C37
		public static void TryJumpAndSelect(GlobalTargetInfo target)
		{
			if (target.IsValid)
			{
				CameraJumper.TryJump(target);
				CameraJumper.TrySelect(target);
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00209858 File Offset: 0x00207C58
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

		// Token: 0x06003DC6 RID: 15814 RVA: 0x002098B4 File Offset: 0x00207CB4
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

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00209964 File Offset: 0x00207D64
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

		// Token: 0x06003DC8 RID: 15816 RVA: 0x002099B8 File Offset: 0x00207DB8
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

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00209A51 File Offset: 0x00207E51
		public static void TryJump(IntVec3 cell, Map map)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false));
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00209A61 File Offset: 0x00207E61
		public static void TryJump(int tile)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile));
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00209A70 File Offset: 0x00207E70
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

		// Token: 0x06003DCC RID: 15820 RVA: 0x00209B10 File Offset: 0x00207F10
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

		// Token: 0x06003DCD RID: 15821 RVA: 0x00209BA5 File Offset: 0x00207FA5
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

		// Token: 0x06003DCE RID: 15822 RVA: 0x00209BDE File Offset: 0x00207FDE
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

		// Token: 0x06003DCF RID: 15823 RVA: 0x00209C10 File Offset: 0x00208010
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

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00209D50 File Offset: 0x00208150
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

		// Token: 0x06003DD1 RID: 15825 RVA: 0x00209F60 File Offset: 0x00208360
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

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00209FB4 File Offset: 0x002083B4
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

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0020A038 File Offset: 0x00208438
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

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0020A0A8 File Offset: 0x002084A8
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
