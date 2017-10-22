using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Buildings
	{
		public static readonly string BlueprintDefNameSuffix = "_Blueprint";

		public static readonly string InstallBlueprintDefNameSuffix = "_Install";

		public static readonly string BuildingFrameDefNameSuffix = "_Frame";

		private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

		private static Color BlueprintColor = new Color(0.5f, 0.5f, 1f, 0.35f);

		public static IEnumerable<ThingDef> ImpliedBlueprintAndFrameDefs()
		{
			List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefs.ToList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					ThingDef blueprint = null;
					if (def.designationCategory != null)
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
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			foreach (TerrainDef allDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (allDef.designationCategory != null)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Terrain(allDef);
					yield return ThingDefGenerator_Buildings.NewFrameDef_Terrain(allDef);
				}
			}
		}

		private static ThingDef BaseBlueprintDef()
		{
			ThingDef thingDef = new ThingDef();
			thingDef.category = ThingCategory.Ethereal;
			thingDef.label = "Unspecified blueprint";
			thingDef.altitudeLayer = AltitudeLayer.Blueprint;
			thingDef.useHitPoints = false;
			thingDef.selectable = true;
			thingDef.seeThroughFog = true;
			thingDef.comps.Add(new CompProperties_Forbiddable());
			thingDef.drawerType = DrawerType.MapMeshAndRealTime;
			return thingDef;
		}

		private static ThingDef BaseFrameDef()
		{
			ThingDef thingDef = new ThingDef();
			thingDef.isFrame = true;
			thingDef.category = ThingCategory.Building;
			thingDef.label = "Unspecified building frame";
			thingDef.thingClass = typeof(Frame);
			thingDef.altitudeLayer = AltitudeLayer.Building;
			thingDef.useHitPoints = true;
			thingDef.selectable = true;
			thingDef.building = new BuildingProperties();
			thingDef.comps.Add(new CompProperties_Forbiddable());
			thingDef.scatterableOnMapGen = false;
			thingDef.leaveResourcesWhenKilled = true;
			return thingDef;
		}

		private static ThingDef NewBlueprintDef_Thing(ThingDef def, bool isInstallBlueprint, ThingDef normalBlueprint = null)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.defName = def.defName + ThingDefGenerator_Buildings.BlueprintDefNameSuffix;
			thingDef.label = def.label + "BlueprintLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (isInstallBlueprint)
			{
				ThingDef obj = thingDef;
				obj.defName += ThingDefGenerator_Buildings.InstallBlueprintDefNameSuffix;
			}
			if (isInstallBlueprint && normalBlueprint != null)
			{
				thingDef.graphicData = normalBlueprint.graphicData;
			}
			else
			{
				thingDef.graphicData = new GraphicData();
				if (def.blueprintGraphicData != null)
				{
					thingDef.graphicData.CopyFrom(def.blueprintGraphicData);
					if (thingDef.graphicData.graphicClass == null)
					{
						thingDef.graphicData.graphicClass = typeof(Graphic_Single);
					}
					if (thingDef.graphicData.shaderType == ShaderType.None)
					{
						thingDef.graphicData.shaderType = ShaderType.MetaOverlay;
					}
					thingDef.graphicData.drawSize = def.graphicData.drawSize;
					thingDef.graphicData.linkFlags = def.graphicData.linkFlags;
					thingDef.graphicData.linkType = def.graphicData.linkType;
				}
				else
				{
					thingDef.graphicData.CopyFrom(def.graphicData);
					thingDef.graphicData.shaderType = ShaderType.Transparent;
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					thingDef.graphicData.colorTwo = Color.white;
					thingDef.graphicData.shadowData = null;
				}
			}
			if (thingDef.graphicData.shadowData != null)
			{
				Log.Error("Blueprint has shadow: " + def);
			}
			if (isInstallBlueprint)
			{
				thingDef.thingClass = typeof(Blueprint_Install);
			}
			else
			{
				thingDef.thingClass = def.blueprintClass;
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

		private static ThingDef NewFrameDef_Thing(ThingDef def)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = def.defName + ThingDefGenerator_Buildings.BuildingFrameDefNameSuffix;
			thingDef.label = def.label + "FrameLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)((float)def.BaseMaxHitPoints * 0.25));
			thingDef.SetStatBaseValue(StatDefOf.Beauty, -8f);
			thingDef.fillPercent = 0.2f;
			thingDef.pathCost = 10;
			thingDef.description = def.description;
			thingDef.passability = def.passability;
			if ((int)thingDef.passability > 1)
			{
				thingDef.passability = Traversability.PassThroughOnly;
			}
			thingDef.selectable = def.selectable;
			thingDef.constructEffect = def.constructEffect;
			thingDef.building.isEdifice = def.building.isEdifice;
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (def.designationCategory != null)
			{
				thingDef.stuffCategories = def.stuffCategories;
			}
			thingDef.entityDefToBuild = def;
			def.frameDef = thingDef;
			return thingDef;
		}

		private static ThingDef NewBlueprintDef_Terrain(TerrainDef terrDef)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.thingClass = typeof(Blueprint_Build);
			thingDef.defName = terrDef.defName + ThingDefGenerator_Buildings.BlueprintDefNameSuffix;
			thingDef.label = terrDef.label + "BlueprintLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.graphicData = new GraphicData();
			thingDef.graphicData.shaderType = ShaderType.MetaOverlay;
			thingDef.graphicData.texPath = ThingDefGenerator_Buildings.TerrainBlueprintGraphicPath;
			thingDef.graphicData.graphicClass = typeof(Graphic_Single);
			thingDef.entityDefToBuild = terrDef;
			terrDef.blueprintDef = thingDef;
			return thingDef;
		}

		private static ThingDef NewFrameDef_Terrain(TerrainDef terrDef)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = terrDef.defName + ThingDefGenerator_Buildings.BuildingFrameDefNameSuffix;
			thingDef.label = terrDef.label + "FrameLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.useHitPoints = false;
			thingDef.fillPercent = 0f;
			thingDef.description = "Terrain building in progress.";
			thingDef.passability = Traversability.Standable;
			thingDef.selectable = true;
			thingDef.constructEffect = terrDef.constructEffect;
			thingDef.building.isEdifice = false;
			thingDef.category = ThingCategory.Ethereal;
			thingDef.entityDefToBuild = terrDef;
			terrDef.frameDef = thingDef;
			if (!thingDef.IsFrame)
			{
				Log.Error("Framedef is not frame: " + thingDef);
			}
			return thingDef;
		}
	}
}
