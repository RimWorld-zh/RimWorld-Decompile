using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public abstract class BuildableDef : Def
	{
		public List<StatModifier> statBases;

		public Traversability passability;

		public int pathCost;

		public bool pathCostIgnoreRepeat = true;

		public float fertility = -1f;

		public List<ThingCountClass> costList;

		public int costStuffCount = -1;

		public List<StuffCategoryDef> stuffCategories;

		public TerrainAffordance terrainAffordanceNeeded = TerrainAffordance.Light;

		public List<ThingDef> buildingPrerequisites;

		public List<ResearchProjectDef> researchPrerequisites;

		public int constructionSkillPrerequisite;

		public int placingDraggableDimensions;

		public bool clearBuildingArea = true;

		public EffecterDef repairEffect;

		public EffecterDef constructEffect;

		public Rot4 defaultPlacingRot = Rot4.North;

		public float resourcesFractionWhenDeconstructed = 0.75f;

		[Unsaved]
		public ThingDef blueprintDef;

		[Unsaved]
		public ThingDef installBlueprintDef;

		[Unsaved]
		public ThingDef frameDef;

		public string uiIconPath;

		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		[Unsaved]
		public Texture2D uiIcon = BaseContent.BadTex;

		[Unsaved]
		public float uiIconAngle;

		[Unsaved]
		public Graphic graphic = BaseContent.BadGraphic;

		public bool menuHidden;

		public float specialDisplayRadius;

		public List<Type> placeWorkers;

		[NoTranslate]
		public DesignationCategoryDef designationCategory;

		public KeyBindingDef designationHotKey;

		public TechLevel minTechLevelToBuild;

		public TechLevel maxTechLevelToBuild;

		[Unsaved]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty();
			}
		}

		public abstract Color IconDrawColor
		{
			get;
		}

		public Material DrawMatSingle
		{
			get
			{
				if (this.graphic == null)
				{
					return null;
				}
				return this.graphic.MatSingle;
			}
		}

		public float Altitude
		{
			get
			{
				return Altitudes.AltitudeFor(this.altitudeLayer);
			}
		}

		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				if (this.placeWorkers == null)
				{
					return null;
				}
				this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
				foreach (Type placeWorker in this.placeWorkers)
				{
					this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(placeWorker));
				}
				return this.placeWorkersInstantiatedInt;
			}
		}

		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			if (this.PlaceWorkers == null)
			{
				return false;
			}
			for (int i = 0; i < this.PlaceWorkers.Count; i++)
			{
				if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
				{
					return true;
				}
			}
			return false;
		}

		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
				else if (this.graphic != null)
				{
					Graphic_Random graphic_Random = this.graphic as Graphic_Random;
					Material material = (graphic_Random == null) ? this.graphic.MatAt(this.defaultPlacingRot, null) : graphic_Random.FirstSubgraphic().MatAt(this.defaultPlacingRot, null);
					if ((UnityEngine.Object)material != (UnityEngine.Object)BaseContent.BadMat)
					{
						this.uiIcon = (Texture2D)material.mainTexture;
						ThingDef thingDef = this as ThingDef;
						if (thingDef != null && thingDef.rotatable && this.graphic.ShouldDrawRotated && this.defaultPlacingRot == Rot4.South)
						{
							this.uiIconAngle = 180f;
						}
					}
				}
			});
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string error = enumerator.Current;
					yield return error;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00b9:
			/*Error near IL_00ba: Unexpected return in MoveNext()*/;
		}

		public override string ToString()
		{
			return base.defName;
		}

		public override int GetHashCode()
		{
			return base.defName.GetHashCode();
		}
	}
}
