using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class ThingFilter : IExposable
	{
		[Unsaved]
		private Action settingsChangedCallback;

		[Unsaved]
		private TreeNode_ThingCategory displayRootCategoryInt;

		[Unsaved]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		[Unsaved]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		public bool allowedHitPointsConfigurable = true;

		private QualityRange allowedQualities = QualityRange.All;

		public bool allowedQualitiesConfigurable = true;

		public string customSummary;

		private List<ThingDef> thingDefs;

		private List<string> categories;

		private List<ThingDef> exceptedThingDefs;

		private List<string> exceptedCategories;

		private List<string> specialFiltersToAllow;

		private List<string> specialFiltersToDisallow;

		private List<StuffCategoryDef> stuffCategoriesToAllow;

		private List<ThingDef> allowAllWhoCanMake;

		public string Summary
		{
			get
			{
				if (!this.customSummary.NullOrEmpty())
				{
					return this.customSummary;
				}
				if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty() && this.exceptedThingDefs.NullOrEmpty() && this.exceptedCategories.NullOrEmpty() && this.specialFiltersToAllow.NullOrEmpty() && this.specialFiltersToDisallow.NullOrEmpty() && this.stuffCategoriesToAllow.NullOrEmpty() && this.allowAllWhoCanMake.NullOrEmpty())
				{
					return this.thingDefs[0].label;
				}
				if (this.thingDefs.NullOrEmpty() && this.categories != null && this.categories.Count == 1 && this.exceptedThingDefs.NullOrEmpty() && this.exceptedCategories.NullOrEmpty() && this.specialFiltersToAllow.NullOrEmpty() && this.specialFiltersToDisallow.NullOrEmpty() && this.stuffCategoriesToAllow.NullOrEmpty() && this.allowAllWhoCanMake.NullOrEmpty())
				{
					return DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label;
				}
				if (this.allowedDefs.Count == 1)
				{
					return this.allowedDefs.First().label;
				}
				return "UsableIngredients".Translate();
			}
		}

		public ThingRequest BestThingRequest
		{
			get
			{
				if (this.allowedDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.allowedDefs.First());
				}
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
		}

		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault();
			}
		}

		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStoreable
				select def;
			}
		}

		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		public FloatRange AllowedHitPointsPercents
		{
			get
			{
				return this.allowedHitPointsPercents;
			}
			set
			{
				this.allowedHitPointsPercents = value;
			}
		}

		public QualityRange AllowedQualityLevels
		{
			get
			{
				return this.allowedQualities;
			}
			set
			{
				this.allowedQualities = value;
			}
		}

		public TreeNode_ThingCategory DisplayRootCategory
		{
			get
			{
				if (this.displayRootCategoryInt == null)
				{
					this.RecalculateDisplayRootCategory();
				}
				if (this.displayRootCategoryInt == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.displayRootCategoryInt;
			}
			set
			{
				if (value != this.displayRootCategoryInt)
				{
					this.displayRootCategoryInt = value;
					this.RecalculateSpecialFilterConfigurability();
				}
			}
		}

		public ThingFilter()
		{
		}

		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, new object[0]);
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
		}

		public void ResolveReferences()
		{
			for (int i = 0; i < DefDatabase<SpecialThingFilterDef>.AllDefsListForReading.Count; i++)
			{
				SpecialThingFilterDef specialThingFilterDef = DefDatabase<SpecialThingFilterDef>.AllDefsListForReading[i];
				if (!specialThingFilterDef.allowedByDefault)
				{
					this.SetAllow(specialThingFilterDef, false);
				}
			}
			if (this.thingDefs != null)
			{
				for (int j = 0; j < this.thingDefs.Count; j++)
				{
					if (this.thingDefs[j] != null)
					{
						this.SetAllow(this.thingDefs[j], true);
					}
					else
					{
						Log.Error("ThingFilter could not find thing def named " + this.thingDefs[j]);
					}
				}
			}
			if (this.categories != null)
			{
				for (int k = 0; k < this.categories.Count; k++)
				{
					ThingCategoryDef named = DefDatabase<ThingCategoryDef>.GetNamed(this.categories[k], true);
					if (named != null)
					{
						this.SetAllow(named, true, null, null);
					}
				}
			}
			if (this.exceptedThingDefs != null)
			{
				for (int l = 0; l < this.exceptedThingDefs.Count; l++)
				{
					if (this.exceptedThingDefs[l] != null)
					{
						this.SetAllow(this.exceptedThingDefs[l], false);
					}
					else
					{
						Log.Error("ThingFilter could not find excepted thing def named " + this.exceptedThingDefs[l]);
					}
				}
			}
			if (this.exceptedCategories != null)
			{
				for (int m = 0; m < this.exceptedCategories.Count; m++)
				{
					ThingCategoryDef named2 = DefDatabase<ThingCategoryDef>.GetNamed(this.exceptedCategories[m], true);
					if (named2 != null)
					{
						this.SetAllow(named2, false, null, null);
					}
				}
			}
			if (this.specialFiltersToAllow != null)
			{
				for (int n = 0; n < this.specialFiltersToAllow.Count; n++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToAllow[n]), true);
				}
			}
			if (this.specialFiltersToDisallow != null)
			{
				for (int num = 0; num < this.specialFiltersToDisallow.Count; num++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToDisallow[num]), false);
				}
			}
			if (this.stuffCategoriesToAllow != null)
			{
				for (int num2 = 0; num2 < this.stuffCategoriesToAllow.Count; num2++)
				{
					this.SetAllow(this.stuffCategoriesToAllow[num2], true);
				}
			}
			if (this.allowAllWhoCanMake != null)
			{
				for (int num3 = 0; num3 < this.allowAllWhoCanMake.Count; num3++)
				{
					this.SetAllowAllWhoCanMake(this.allowAllWhoCanMake[num3]);
				}
			}
			this.RecalculateDisplayRootCategory();
		}

		public void RecalculateDisplayRootCategory()
		{
			this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
			foreach (TreeNode_ThingCategory allThingCategoryNode in ThingCategoryNodeDatabase.AllThingCategoryNodes)
			{
				bool flag = false;
				bool flag2 = false;
				foreach (ThingDef allowedDef in this.allowedDefs)
				{
					if (allThingCategoryNode.catDef.DescendantThingDefs.Contains(allowedDef))
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
				if (!flag && flag2)
				{
					this.DisplayRootCategory = allThingCategoryNode;
				}
			}
		}

		private void RecalculateSpecialFilterConfigurability()
		{
			if (this.DisplayRootCategory == null)
			{
				this.allowedHitPointsConfigurable = true;
				this.allowedQualitiesConfigurable = true;
			}
			else
			{
				this.allowedHitPointsConfigurable = false;
				this.allowedQualitiesConfigurable = false;
				foreach (ThingDef descendantThingDef in this.DisplayRootCategory.catDef.DescendantThingDefs)
				{
					if (descendantThingDef.useHitPoints)
					{
						this.allowedHitPointsConfigurable = true;
					}
					if (descendantThingDef.HasComp(typeof(CompQuality)))
					{
						this.allowedQualitiesConfigurable = true;
					}
					if (this.allowedHitPointsConfigurable && this.allowedQualitiesConfigurable)
						break;
				}
			}
		}

		public bool IsAlwaysDisallowedDueToSpecialFilters(ThingDef def)
		{
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.AlwaysMatches(def))
				{
					return true;
				}
			}
			return false;
		}

		public virtual void CopyAllowancesFrom(ThingFilter other)
		{
			this.allowedDefs.Clear();
			foreach (ThingDef allStorableThingDef in ThingFilter.AllStorableThingDefs)
			{
				this.SetAllow(allStorableThingDef, other.Allows(allStorableThingDef));
			}
			this.disallowedSpecialFilters = other.disallowedSpecialFilters.ListFullCopyOrNull();
			this.allowedHitPointsPercents = other.allowedHitPointsPercents;
			this.allowedHitPointsConfigurable = other.allowedHitPointsConfigurable;
			this.allowedQualities = other.allowedQualities;
			this.allowedQualitiesConfigurable = other.allowedQualitiesConfigurable;
			this.thingDefs = other.thingDefs.ListFullCopyOrNull();
			this.categories = other.categories.ListFullCopyOrNull();
			this.exceptedThingDefs = other.exceptedThingDefs.ListFullCopyOrNull();
			this.exceptedCategories = other.exceptedCategories.ListFullCopyOrNull();
			this.specialFiltersToAllow = other.specialFiltersToAllow.ListFullCopyOrNull();
			this.specialFiltersToDisallow = other.specialFiltersToDisallow.ListFullCopyOrNull();
			this.stuffCategoriesToAllow = other.stuffCategoriesToAllow.ListFullCopyOrNull();
			this.allowAllWhoCanMake = other.allowAllWhoCanMake.ListFullCopyOrNull();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetAllow(ThingDef thingDef, bool allow)
		{
			if (allow != this.Allows(thingDef))
			{
				if (allow)
				{
					this.allowedDefs.Add(thingDef);
				}
				else
				{
					this.allowedDefs.Remove(thingDef);
				}
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		public void SetAllow(SpecialThingFilterDef sfDef, bool allow)
		{
			if (sfDef.configurable && allow != this.Allows(sfDef))
			{
				if (allow)
				{
					if (this.disallowedSpecialFilters.Contains(sfDef))
					{
						this.disallowedSpecialFilters.Remove(sfDef);
					}
				}
				else if (!this.disallowedSpecialFilters.Contains(sfDef))
				{
					this.disallowedSpecialFilters.Add(sfDef);
				}
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.");
			}
			foreach (ThingDef descendantThingDef in categoryDef.DescendantThingDefs)
			{
				if (exceptedDefs == null || !exceptedDefs.Contains(descendantThingDef))
				{
					this.SetAllow(descendantThingDef, allow);
				}
			}
			foreach (SpecialThingFilterDef descendantSpecialThingFilterDef in categoryDef.DescendantSpecialThingFilterDefs)
			{
				if (exceptedFilters == null || !exceptedFilters.Contains(descendantSpecialThingFilterDef))
				{
					this.SetAllow(descendantSpecialThingFilterDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetAllow(StuffCategoryDef cat, bool allow)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffCategories.Contains(cat))
				{
					this.SetAllow(thingDef, true);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetAllowAllWhoCanMake(ThingDef thing)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.CanMake(thing))
				{
					this.SetAllow(thingDef, true);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetFromPreset(StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Manufactured, true, null, null);
				this.SetAllow(ThingCategoryDefOf.ResourcesRaw, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Items, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Art, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Weapons, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				this.SetAllow(ThingCategoryDefOf.BodyParts, true, null, null);
			}
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Chunks, true, null, null);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			foreach (SpecialThingFilterDef allDef in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				if (allDef.configurable && (exceptedFilters == null || !exceptedFilters.Contains(allDef)))
				{
					this.disallowedSpecialFilters.Add(allDef);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetAllowAll(ThingFilter parentFilter)
		{
			this.allowedDefs.Clear();
			if (parentFilter != null)
			{
				foreach (ThingDef allowedDef in parentFilter.allowedDefs)
				{
					this.allowedDefs.Add(allowedDef);
				}
			}
			else
			{
				foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (!allDef.thingCategories.NullOrEmpty())
					{
						this.allowedDefs.Add(allDef);
					}
				}
			}
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable);
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public virtual bool Allows(Thing t)
		{
			if (!this.Allows(t.def))
			{
				return false;
			}
			if (t.def.useHitPoints)
			{
				float f = (float)t.HitPoints / (float)t.MaxHitPoints;
				f = GenMath.RoundedHundredth(f);
				if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(f)))
				{
					return false;
				}
			}
			if (this.allowedQualities != QualityRange.All && t.def.FollowQualityThingFilter())
			{
				QualityCategory p = default(QualityCategory);
				if (!t.TryGetQuality(out p))
				{
					p = QualityCategory.Normal;
				}
				if (!this.allowedQualities.Includes(p))
				{
					return false;
				}
			}
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.Matches(t))
				{
					return false;
				}
			}
			return true;
		}

		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		public ThingRequest GetThingRequest()
		{
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			return ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
		}

		public override string ToString()
		{
			return this.Summary;
		}
	}
}
