using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B05 RID: 2821
	public abstract class BuildableDef : Def
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003E71 RID: 15985 RVA: 0x0020E64C File Offset: 0x0020CA4C
		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003E72 RID: 15986 RVA: 0x0020E668 File Offset: 0x0020CA68
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003E73 RID: 15987 RVA: 0x0020E68C File Offset: 0x0020CA8C
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003E74 RID: 15988 RVA: 0x0020E6B0 File Offset: 0x0020CAB0
		public Material DrawMatSingle
		{
			get
			{
				Material result;
				if (this.graphic == null)
				{
					result = null;
				}
				else
				{
					result = this.graphic.MatSingle;
				}
				return result;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06003E75 RID: 15989 RVA: 0x0020E6E4 File Offset: 0x0020CAE4
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06003E76 RID: 15990 RVA: 0x0020E704 File Offset: 0x0020CB04
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
					foreach (Type type in this.placeWorkers)
					{
						this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
					}
					result = this.placeWorkersInstantiatedInt;
				}
				return result;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06003E77 RID: 15991 RVA: 0x0020E7A0 File Offset: 0x0020CBA0
		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x0020E800 File Offset: 0x0020CC00
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
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x0020E864 File Offset: 0x0020CC64
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
				else
				{
					this.ResolveIcon();
				}
			});
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x0020E880 File Offset: 0x0020CC80
		protected virtual void ResolveIcon()
		{
			if (this.graphic != null && this.graphic != BaseContent.BadGraphic)
			{
				Material material = this.graphic.ExtractInnerGraphicFor(null).MatAt(this.defaultPlacingRot, null);
				this.uiIcon = (Texture2D)material.mainTexture;
				this.uiIconColor = material.color;
			}
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x0020E8E1 File Offset: 0x0020CCE1
		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0020E8EC File Offset: 0x0020CCEC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return error;
			}
			yield break;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x0020E918 File Offset: 0x0020CD18
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry stat in this.<SpecialDisplayStats>__BaseCallProxy1())
			{
				yield return stat;
			}
			IEnumerable<TerrainAffordanceDef> affdefs = Enumerable.Empty<TerrainAffordanceDef>();
			if (this.PlaceWorkers != null)
			{
				affdefs = affdefs.Concat(this.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
			}
			if (this.terrainAffordanceNeeded != null)
			{
				affdefs = affdefs.Concat(this.terrainAffordanceNeeded);
			}
			string[] affordances = (from ta in affdefs.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (affordances.Length > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), affordances.ToCommaList(false).CapitalizeFirst(), 0, "");
			}
			yield break;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0020E944 File Offset: 0x0020CD44
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x0020E960 File Offset: 0x0020CD60
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x0400278D RID: 10125
		public List<StatModifier> statBases = null;

		// Token: 0x0400278E RID: 10126
		public Traversability passability = Traversability.Standable;

		// Token: 0x0400278F RID: 10127
		public int pathCost = 0;

		// Token: 0x04002790 RID: 10128
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x04002791 RID: 10129
		public float fertility = -1f;

		// Token: 0x04002792 RID: 10130
		public List<ThingDefCountClass> costList = null;

		// Token: 0x04002793 RID: 10131
		public int costStuffCount = 0;

		// Token: 0x04002794 RID: 10132
		public List<StuffCategoryDef> stuffCategories = null;

		// Token: 0x04002795 RID: 10133
		public int placingDraggableDimensions = 0;

		// Token: 0x04002796 RID: 10134
		public bool clearBuildingArea = true;

		// Token: 0x04002797 RID: 10135
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x04002798 RID: 10136
		public float resourcesFractionWhenDeconstructed = 0.75f;

		// Token: 0x04002799 RID: 10137
		public TerrainAffordanceDef terrainAffordanceNeeded = null;

		// Token: 0x0400279A RID: 10138
		public List<ThingDef> buildingPrerequisites = null;

		// Token: 0x0400279B RID: 10139
		public List<ResearchProjectDef> researchPrerequisites = null;

		// Token: 0x0400279C RID: 10140
		public int constructionSkillPrerequisite = 0;

		// Token: 0x0400279D RID: 10141
		public TechLevel minTechLevelToBuild = TechLevel.Undefined;

		// Token: 0x0400279E RID: 10142
		public TechLevel maxTechLevelToBuild = TechLevel.Undefined;

		// Token: 0x0400279F RID: 10143
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x040027A0 RID: 10144
		public EffecterDef repairEffect = null;

		// Token: 0x040027A1 RID: 10145
		public EffecterDef constructEffect = null;

		// Token: 0x040027A2 RID: 10146
		public bool menuHidden = false;

		// Token: 0x040027A3 RID: 10147
		public float specialDisplayRadius = 0f;

		// Token: 0x040027A4 RID: 10148
		public List<Type> placeWorkers = null;

		// Token: 0x040027A5 RID: 10149
		public DesignationCategoryDef designationCategory = null;

		// Token: 0x040027A6 RID: 10150
		public DesignatorDropdownGroupDef designatorDropdown = null;

		// Token: 0x040027A7 RID: 10151
		public KeyBindingDef designationHotKey = null;

		// Token: 0x040027A8 RID: 10152
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x040027A9 RID: 10153
		public Vector2 uiIconOffset;

		// Token: 0x040027AA RID: 10154
		[Unsaved]
		public ThingDef blueprintDef;

		// Token: 0x040027AB RID: 10155
		[Unsaved]
		public ThingDef installBlueprintDef;

		// Token: 0x040027AC RID: 10156
		[Unsaved]
		public ThingDef frameDef;

		// Token: 0x040027AD RID: 10157
		[Unsaved]
		private List<PlaceWorker> placeWorkersInstantiatedInt = null;

		// Token: 0x040027AE RID: 10158
		[Unsaved]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x040027AF RID: 10159
		[Unsaved]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x040027B0 RID: 10160
		[Unsaved]
		public float uiIconAngle;

		// Token: 0x040027B1 RID: 10161
		[Unsaved]
		public Color uiIconColor = Color.white;
	}
}
