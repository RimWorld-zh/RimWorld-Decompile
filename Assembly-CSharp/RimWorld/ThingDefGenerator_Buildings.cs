using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000237 RID: 567
	public static class ThingDefGenerator_Buildings
	{
		// Token: 0x040003EB RID: 1003
		public static readonly string BlueprintDefNamePrefix = "Blueprint_";

		// Token: 0x040003EC RID: 1004
		public static readonly string InstallBlueprintDefNamePrefix = "Install_";

		// Token: 0x040003ED RID: 1005
		public static readonly string BuildingFrameDefNamePrefix = "Frame_";

		// Token: 0x040003EE RID: 1006
		private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

		// Token: 0x040003EF RID: 1007
		private static Color BlueprintColor = new Color(0.8235294f, 0.921568632f, 1f, 0.6f);

		// Token: 0x06000A3C RID: 2620 RVA: 0x0005BC00 File Offset: 0x0005A000
		public static IEnumerable<ThingDef> ImpliedBlueprintAndFrameDefs()
		{
			foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs.ToList<ThingDef>())
			{
				ThingDef blueprint = null;
				if (def.BuildableByPlayer)
				{
					blueprint = ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, false, null);
					yield return blueprint;
					yield return ThingDefGenerator_Buildings.NewFrameDef_Thing(def);
				}
				if (def.Minifiable)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, true, blueprint);
				}
			}
			foreach (TerrainDef terrDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (terrDef.BuildableByPlayer)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Terrain(terrDef);
					yield return ThingDefGenerator_Buildings.NewFrameDef_Terrain(terrDef);
				}
			}
			yield break;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0005BC24 File Offset: 0x0005A024
		private static ThingDef BaseBlueprintDef()
		{
			return new ThingDef
			{
				category = ThingCategory.Ethereal,
				label = "Unspecified blueprint",
				altitudeLayer = AltitudeLayer.Blueprint,
				useHitPoints = false,
				selectable = true,
				seeThroughFog = true,
				comps = 
				{
					new CompProperties_Forbiddable()
				},
				drawerType = DrawerType.MapMeshAndRealTime
			};
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0005BC88 File Offset: 0x0005A088
		private static ThingDef BaseFrameDef()
		{
			return new ThingDef
			{
				isFrame = true,
				category = ThingCategory.Building,
				label = "Unspecified building frame",
				thingClass = typeof(Frame),
				altitudeLayer = AltitudeLayer.Building,
				useHitPoints = true,
				selectable = true,
				building = new BuildingProperties(),
				comps = 
				{
					new CompProperties_Forbiddable()
				},
				scatterableOnMapGen = false,
				leaveResourcesWhenKilled = true
			};
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0005BD0C File Offset: 0x0005A10C
		private static ThingDef NewBlueprintDef_Thing(ThingDef def, bool isInstallBlueprint, ThingDef normalBlueprint = null)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + def.defName;
			thingDef.label = def.label + "BlueprintLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.modContentPack = def.modContentPack;
			if (!isInstallBlueprint)
			{
				thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
			}
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (isInstallBlueprint)
			{
				thingDef.defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + ThingDefGenerator_Buildings.InstallBlueprintDefNamePrefix + def.defName;
			}
			if (isInstallBlueprint && normalBlueprint != null)
			{
				thingDef.graphicData = normalBlueprint.graphicData;
			}
			else
			{
				thingDef.graphicData = new GraphicData();
				if (def.building.blueprintGraphicData != null)
				{
					thingDef.graphicData.CopyFrom(def.building.blueprintGraphicData);
					if (thingDef.graphicData.graphicClass == null)
					{
						thingDef.graphicData.graphicClass = typeof(Graphic_Single);
					}
					if (thingDef.graphicData.shaderType == null)
					{
						thingDef.graphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					thingDef.graphicData.drawSize = def.graphicData.drawSize;
					thingDef.graphicData.linkFlags = def.graphicData.linkFlags;
					thingDef.graphicData.linkType = def.graphicData.linkType;
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
				}
				else
				{
					thingDef.graphicData.CopyFrom(def.graphicData);
					thingDef.graphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					thingDef.graphicData.colorTwo = Color.white;
					thingDef.graphicData.shadowData = null;
				}
			}
			if (thingDef.graphicData.shadowData != null)
			{
				Log.Error("Blueprint has shadow: " + def, false);
			}
			if (isInstallBlueprint)
			{
				thingDef.thingClass = typeof(Blueprint_Install);
			}
			else
			{
				thingDef.thingClass = def.building.blueprintClass;
			}
			if (def.thingClass == typeof(Building_Door))
			{
				thingDef.drawerType = DrawerType.RealtimeOnly;
			}
			else
			{
				thingDef.drawerType = DrawerType.MapMeshAndRealTime;
			}
			thingDef.entityDefToBuild = def;
			if (isInstallBlueprint)
			{
				def.installBlueprintDef = thingDef;
			}
			else
			{
				def.blueprintDef = thingDef;
			}
			return thingDef;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0005BFAC File Offset: 0x0005A3AC
		private static ThingDef NewFrameDef_Thing(ThingDef def)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + def.defName;
			thingDef.label = def.label + "FrameLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)def.BaseMaxHitPoints * 0.25f);
			thingDef.SetStatBaseValue(StatDefOf.Beauty, -8f);
			thingDef.SetStatBaseValue(StatDefOf.Flammability, def.BaseFlammability);
			thingDef.fillPercent = 0.2f;
			thingDef.pathCost = 10;
			thingDef.description = def.description;
			thingDef.passability = def.passability;
			if (thingDef.passability > Traversability.PassThroughOnly)
			{
				thingDef.passability = Traversability.PassThroughOnly;
			}
			thingDef.selectable = def.selectable;
			thingDef.constructEffect = def.constructEffect;
			thingDef.building.isEdifice = def.building.isEdifice;
			thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.modContentPack = def.modContentPack;
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (def.BuildableByPlayer)
			{
				thingDef.stuffCategories = def.stuffCategories;
			}
			thingDef.entityDefToBuild = def;
			def.frameDef = thingDef;
			return thingDef;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0005C11C File Offset: 0x0005A51C
		private static ThingDef NewBlueprintDef_Terrain(TerrainDef terrDef)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.thingClass = typeof(Blueprint_Build);
			thingDef.defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + terrDef.defName;
			thingDef.label = terrDef.label + "BlueprintLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.graphicData = new GraphicData();
			thingDef.graphicData.shaderType = ShaderTypeDefOf.MetaOverlay;
			thingDef.graphicData.texPath = ThingDefGenerator_Buildings.TerrainBlueprintGraphicPath;
			thingDef.graphicData.graphicClass = typeof(Graphic_Single);
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.entityDefToBuild = terrDef;
			terrDef.blueprintDef = thingDef;
			return thingDef;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0005C1F0 File Offset: 0x0005A5F0
		private static ThingDef NewFrameDef_Terrain(TerrainDef terrDef)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + terrDef.defName;
			thingDef.label = terrDef.label + "FrameLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.useHitPoints = false;
			thingDef.fillPercent = 0f;
			thingDef.description = "Terrain building in progress.";
			thingDef.passability = Traversability.Standable;
			thingDef.selectable = true;
			thingDef.constructEffect = terrDef.constructEffect;
			thingDef.building.isEdifice = false;
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.category = ThingCategory.Ethereal;
			thingDef.entityDefToBuild = terrDef;
			terrDef.frameDef = thingDef;
			if (!thingDef.IsFrame)
			{
				Log.Error("Framedef is not frame: " + thingDef, false);
			}
			return thingDef;
		}
	}
}
