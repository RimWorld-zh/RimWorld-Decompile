using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class BuildingsDamageSectionLayerUtility
	{
		private static readonly Material[] DefaultScratchMats = new Material[3]
		{
			MaterialPool.MatFrom("Damage/Scratch1"),
			MaterialPool.MatFrom("Damage/Scratch2"),
			MaterialPool.MatFrom("Damage/Scratch3")
		};

		private static List<DamageOverlay> availableOverlays = new List<DamageOverlay>();

		private static List<DamageOverlay> overlaysWorkingList = new List<DamageOverlay>();

		private static List<DamageOverlay> overlays = new List<DamageOverlay>();

		public static void Notify_BuildingHitPointsChanged(Building b, int oldHitPoints)
		{
			if (b.Spawned && b.def.useHitPoints && b.HitPoints != oldHitPoints && b.def.drawDamagedOverlay && BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints) != BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, oldHitPoints))
			{
				b.Map.mapDrawer.MapMeshDirty(b.Position, MapMeshFlag.BuildingsDamage);
			}
		}

		public static bool UsesLinkableCornersAndEdges(Building b)
		{
			return b.def.size.x == 1 && b.def.size.z == 1 && b.def.Fillage == FillCategory.Full;
		}

		public static IList<Material> GetScratchMats(Building b)
		{
			IList<Material> result = BuildingsDamageSectionLayerUtility.DefaultScratchMats;
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && b.def.graphicData.damageData.scratchMats != null)
			{
				result = b.def.graphicData.damageData.scratchMats;
			}
			return result;
		}

		public static List<DamageOverlay> GetAvailableOverlays(Building b)
		{
			BuildingsDamageSectionLayerUtility.availableOverlays.Clear();
			if (BuildingsDamageSectionLayerUtility.GetScratchMats(b).Any())
			{
				int num = 3;
				Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
				float num2 = damageRect.width * damageRect.height;
				if (num2 > 4.0)
				{
					num += Mathf.RoundToInt((float)((num2 - 4.0) * 0.54000002145767212));
				}
				for (int num3 = 0; num3 < num; num3++)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.Scratch);
				}
			}
			if (BuildingsDamageSectionLayerUtility.UsesLinkableCornersAndEdges(b))
			{
				if (b.def.graphicData != null && b.def.graphicData.damageData != null)
				{
					IntVec3 position = b.Position;
					DamageGraphicData damageData = b.def.graphicData.damageData;
					if ((Object)damageData.edgeTopMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopEdge);
					}
					if ((Object)damageData.edgeRightMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.RightEdge);
					}
					if ((Object)damageData.edgeBotMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotEdge);
					}
					if ((Object)damageData.edgeLeftMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.LeftEdge);
					}
					if ((Object)damageData.cornerTLMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopLeftCorner);
					}
					if ((Object)damageData.cornerTRMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopRightCorner);
					}
					if ((Object)damageData.cornerBRMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotRightCorner);
					}
					if ((Object)damageData.cornerBLMat != (Object)null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotLeftCorner);
					}
				}
			}
			else
			{
				Material x = default(Material);
				Material x2 = default(Material);
				Material x3 = default(Material);
				Material x4 = default(Material);
				BuildingsDamageSectionLayerUtility.GetCornerMats(out x, out x2, out x3, out x4, b);
				if ((Object)x != (Object)null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopLeftCorner);
				}
				if ((Object)x2 != (Object)null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopRightCorner);
				}
				if ((Object)x4 != (Object)null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotLeftCorner);
				}
				if ((Object)x3 != (Object)null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotRightCorner);
				}
			}
			return BuildingsDamageSectionLayerUtility.availableOverlays;
		}

		public static void GetCornerMats(out Material topLeft, out Material topRight, out Material botRight, out Material botLeft, Building b)
		{
			if (b.def.graphicData == null || b.def.graphicData.damageData == null)
			{
				topLeft = null;
				topRight = null;
				botRight = null;
				botLeft = null;
			}
			else
			{
				DamageGraphicData damageData = b.def.graphicData.damageData;
				if (b.Rotation == Rot4.North)
				{
					topLeft = damageData.cornerTLMat;
					topRight = damageData.cornerTRMat;
					botRight = damageData.cornerBRMat;
					botLeft = damageData.cornerBLMat;
				}
				else if (b.Rotation == Rot4.East)
				{
					topLeft = damageData.cornerBLMat;
					topRight = damageData.cornerTLMat;
					botRight = damageData.cornerTRMat;
					botLeft = damageData.cornerBRMat;
				}
				else if (b.Rotation == Rot4.South)
				{
					topLeft = damageData.cornerBRMat;
					topRight = damageData.cornerBLMat;
					botRight = damageData.cornerTLMat;
					botLeft = damageData.cornerTRMat;
				}
				else
				{
					topLeft = damageData.cornerTRMat;
					topRight = damageData.cornerBRMat;
					botRight = damageData.cornerBLMat;
					botLeft = damageData.cornerTLMat;
				}
			}
		}

		public static List<DamageOverlay> GetOverlays(Building b)
		{
			BuildingsDamageSectionLayerUtility.overlays.Clear();
			BuildingsDamageSectionLayerUtility.overlaysWorkingList.Clear();
			BuildingsDamageSectionLayerUtility.overlaysWorkingList.AddRange(BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b));
			if (!BuildingsDamageSectionLayerUtility.overlaysWorkingList.Any())
			{
				return BuildingsDamageSectionLayerUtility.overlays;
			}
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(b.thingIDNumber, 1958376471);
			int damageOverlaysCount = BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints);
			int num = 0;
			while (num < damageOverlaysCount && BuildingsDamageSectionLayerUtility.overlaysWorkingList.Any())
			{
				DamageOverlay item = BuildingsDamageSectionLayerUtility.overlaysWorkingList.RandomElement();
				BuildingsDamageSectionLayerUtility.overlaysWorkingList.Remove(item);
				BuildingsDamageSectionLayerUtility.overlays.Add(item);
				num++;
			}
			Rand.PopState();
			return BuildingsDamageSectionLayerUtility.overlays;
		}

		public static Rect GetDamageRect(Building b)
		{
			DamageGraphicData damageGraphicData = null;
			if (b.def.graphicData != null)
			{
				damageGraphicData = b.def.graphicData.damageData;
			}
			CellRect cellRect = b.OccupiedRect();
			Rect result = new Rect((float)cellRect.minX, (float)cellRect.minZ, (float)cellRect.Width, (float)cellRect.Height);
			if (damageGraphicData != null)
			{
				if (b.Rotation == Rot4.North && damageGraphicData.rectN != default(Rect))
				{
					result.position += damageGraphicData.rectN.position;
					result.size = damageGraphicData.rectN.size;
				}
				else if (b.Rotation == Rot4.East && damageGraphicData.rectE != default(Rect))
				{
					result.position += damageGraphicData.rectE.position;
					result.size = damageGraphicData.rectE.size;
				}
				else if (b.Rotation == Rot4.South && damageGraphicData.rectS != default(Rect))
				{
					result.position += damageGraphicData.rectS.position;
					result.size = damageGraphicData.rectS.size;
				}
				else if (b.Rotation == Rot4.West && damageGraphicData.rectW != default(Rect))
				{
					result.position += damageGraphicData.rectW.position;
					result.size = damageGraphicData.rectW.size;
				}
				else if (damageGraphicData.rect != default(Rect))
				{
					Rect rect = damageGraphicData.rect;
					if (b.Rotation == Rot4.North)
					{
						result.x += rect.x;
						result.y += rect.y;
						result.width = rect.width;
						result.height = rect.height;
					}
					else if (b.Rotation == Rot4.South)
					{
						result.x += (float)cellRect.Width - rect.x - rect.width;
						result.y += (float)cellRect.Height - rect.y - rect.height;
						result.width = rect.width;
						result.height = rect.height;
					}
					else if (b.Rotation == Rot4.West)
					{
						result.x += (float)cellRect.Width - rect.y - rect.height;
						result.y += rect.x;
						result.width = rect.height;
						result.height = rect.width;
					}
					else if (b.Rotation == Rot4.East)
					{
						result.x += rect.y;
						result.y += (float)cellRect.Height - rect.x - rect.width;
						result.width = rect.height;
						result.height = rect.width;
					}
				}
			}
			return result;
		}

		private static int GetDamageOverlaysCount(Building b, int hp)
		{
			float num = (float)hp / (float)b.MaxHitPoints;
			int count = BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b).Count;
			return count - Mathf.FloorToInt((float)count * num);
		}

		private static bool DifferentAt(Building b, int x, int z)
		{
			IntVec3 c = new IntVec3(x, 0, z);
			if (!c.InBounds(b.Map))
			{
				return true;
			}
			List<Thing> thingList = c.GetThingList(b.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == b.def)
				{
					return false;
				}
			}
			return true;
		}

		private static bool SameAndDamagedAt(Building b, int x, int z)
		{
			IntVec3 c = new IntVec3(x, 0, z);
			if (!c.InBounds(b.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(b.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == b.def && thingList[i].HitPoints < thingList[i].MaxHitPoints)
				{
					return true;
				}
			}
			return false;
		}
	}
}
