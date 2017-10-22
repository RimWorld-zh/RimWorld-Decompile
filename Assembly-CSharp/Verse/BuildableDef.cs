using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public abstract class BuildableDef : Def
	{
		public List<StatModifier> statBases = null;

		public Traversability passability = Traversability.Standable;

		public int pathCost = 0;

		public bool pathCostIgnoreRepeat = true;

		public float fertility = -1f;

		public List<ThingCountClass> costList = null;

		public int costStuffCount = -1;

		public List<StuffCategoryDef> stuffCategories = null;

		public TerrainAffordance terrainAffordanceNeeded = TerrainAffordance.Light;

		public List<ThingDef> buildingPrerequisites = null;

		public List<ResearchProjectDef> researchPrerequisites = null;

		public int constructionSkillPrerequisite = 0;

		public int placingDraggableDimensions = 0;

		public bool clearBuildingArea = true;

		public EffecterDef repairEffect = null;

		public EffecterDef constructEffect = null;

		public Rot4 defaultPlacingRot = Rot4.North;

		public float resourcesFractionWhenDeconstructed = 0.75f;

		[Unsaved]
		public ThingDef blueprintDef;

		[Unsaved]
		public ThingDef installBlueprintDef;

		[Unsaved]
		public ThingDef frameDef;

		public string uiIconPath = (string)null;

		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		[Unsaved]
		public Texture2D uiIcon = BaseContent.BadTex;

		[Unsaved]
		public float uiIconAngle;

		[Unsaved]
		public Graphic graphic = BaseContent.BadGraphic;

		public bool menuHidden = false;

		public float specialDisplayRadius = 0f;

		public List<Type> placeWorkers = null;

		[NoTranslate]
		public DesignationCategoryDef designationCategory = null;

		public KeyBindingDef designationHotKey = null;

		public TechLevel minTechLevelToBuild = TechLevel.Undefined;

		public TechLevel maxTechLevelToBuild = TechLevel.Undefined;

		[Unsaved]
		private List<PlaceWorker> placeWorkersInstantiatedInt = null;

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
				return (this.graphic != null) ? this.graphic.MatSingle : null;
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
				List<PlaceWorker> result;
				if (this.placeWorkers == null)
				{
					result = null;
				}
				else
				{
					this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
					foreach (Type placeWorker in this.placeWorkers)
					{
						this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(placeWorker));
					}
					result = this.placeWorkersInstantiatedInt;
				}
				return result;
			}
		}

		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			bool result;
			if (this.PlaceWorkers == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.PlaceWorkers.Count; i++)
				{
					if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
						goto IL_0032;
				}
				result = false;
			}
			goto IL_0056;
			IL_0032:
			result = true;
			goto IL_0056;
			IL_0056:
			return result;
		}

		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished((Action)delegate
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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string error = enumerator.Current;
					yield return error;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00bd:
			/*Error near IL_00be: Unexpected return in MoveNext()*/;
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
