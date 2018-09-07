using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Buildings
	{
		public static readonly string BlueprintDefNamePrefix = "Blueprint_";

		public static readonly string InstallBlueprintDefNamePrefix = "Install_";

		public static readonly string BuildingFrameDefNamePrefix = "Frame_";

		private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

		private static Color BlueprintColor = new Color(0.8235294f, 0.921568632f, 1f, 0.6f);

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

		private static ThingDef BaseFrameDef()
		{
			return new ThingDef
			{
				isFrameInt = true,
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

		// Note: this type is marked as 'beforefieldinit'.
		static ThingDefGenerator_Buildings()
		{
		}

		[CompilerGenerated]
		private sealed class <ImpliedBlueprintAndFrameDefs>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal List<ThingDef>.Enumerator $locvar0;

			internal ThingDef <def>__1;

			internal ThingDef <blueprint>__2;

			internal IEnumerator<TerrainDef> $locvar1;

			internal TerrainDef <terrDef>__3;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ImpliedBlueprintAndFrameDefs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = DefDatabase<ThingDef>.AllDefs.ToList<ThingDef>().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
				case 3u:
					break;
				case 4u:
				case 5u:
					goto IL_164;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						this.$current = ThingDefGenerator_Buildings.NewFrameDef_Thing(def);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					case 2u:
						break;
					case 3u:
						goto IL_126;
					default:
						goto IL_126;
					}
					IL_E8:
					if (def.Minifiable)
					{
						this.$current = ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, true, blueprint);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
					IL_126:
					if (enumerator.MoveNext())
					{
						def = enumerator.Current;
						blueprint = null;
						if (def.BuildableByPlayer)
						{
							blueprint = ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, false, null);
							this.$current = blueprint;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_E8;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				enumerator2 = DefDatabase<TerrainDef>.AllDefs.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_164:
					switch (num)
					{
					case 4u:
						this.$current = ThingDefGenerator_Buildings.NewFrameDef_Terrain(terrDef);
						if (!this.$disposing)
						{
							this.$PC = 5;
						}
						flag = true;
						return true;
					}
					while (enumerator2.MoveNext())
					{
						terrDef = enumerator2.Current;
						if (terrDef.BuildableByPlayer)
						{
							this.$current = ThingDefGenerator_Buildings.NewBlueprintDef_Terrain(terrDef);
							if (!this.$disposing)
							{
								this.$PC = 4;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
				case 2u:
				case 3u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 4u:
				case 5u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ThingDefGenerator_Buildings.<ImpliedBlueprintAndFrameDefs>c__Iterator0();
			}
		}
	}
}
