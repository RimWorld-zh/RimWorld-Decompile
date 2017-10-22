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
		private TreeNode_ThingCategory displayRootCategoryInt = null;

		[Unsaved]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		[Unsaved]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		public bool allowedHitPointsConfigurable = true;

		private QualityRange allowedQualities = QualityRange.All;

		public bool allowedQualitiesConfigurable = true;

		public string customSummary = (string)null;

		private List<ThingDef> thingDefs = null;

		private List<string> categories = null;

		private List<ThingDef> exceptedThingDefs = null;

		private List<string> exceptedCategories = null;

		private List<string> specialFiltersToAllow = null;

		private List<string> specialFiltersToDisallow = null;

		private List<StuffCategoryDef> stuffCategoriesToAllow = null;

		private List<ThingDef> allowAllWhoCanMake = null;

		public string Summary
		{
			get
			{
				return this.customSummary.NullOrEmpty() ? ((this.thingDefs == null || this.thingDefs.Count != 1 || !this.categories.NullOrEmpty() || !this.exceptedThingDefs.NullOrEmpty() || !this.exceptedCategories.NullOrEmpty() || !this.specialFiltersToAllow.NullOrEmpty() || !this.specialFiltersToDisallow.NullOrEmpty() || !this.stuffCategoriesToAllow.NullOrEmpty() || !this.allowAllWhoCanMake.NullOrEmpty()) ? ((!this.thingDefs.NullOrEmpty() || this.categories == null || this.categories.Count != 1 || !this.exceptedThingDefs.NullOrEmpty() || !this.exceptedCategories.NullOrEmpty() || !this.specialFiltersToAllow.NullOrEmpty() || !this.specialFiltersToDisallow.NullOrEmpty() || !this.stuffCategoriesToAllow.NullOrEmpty() || !this.allowAllWhoCanMake.NullOrEmpty()) ? ((this.allowedDefs.Count != 1) ? "UsableIngredients".Translate() : this.allowedDefs.First().label) : DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label) : this.thingDefs[0].label) : this.customSummary;
			}
		}

		public ThingRequest BestThingRequest
		{
			get
			{
				return (this.allowedDefs.Count != 1) ? ThingRequest.ForGroup(ThingRequestGroup.HaulableEver) : ThingRequest.ForDef(this.allowedDefs.First());
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
				return (this.displayRootCategoryInt != null) ? this.displayRootCategoryInt : ThingCategoryNodeDatabase.RootNode;
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
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.disallowedSpecialFilters.Count)
				{
					if (this.disallowedSpecialFilters[num].Worker.AlwaysMatches(def))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
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
			if ((object)this.settingsChangedCallback != null)
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
				if ((object)this.settingsChangedCallback != null)
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
				if ((object)this.settingsChangedCallback != null)
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
			if ((object)this.settingsChangedCallback != null)
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
			if ((object)this.settingsChangedCallback != null)
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
			if ((object)this.settingsChangedCallback != null)
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
			if ((object)this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((Predicate<ThingDef>)((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d)));
			this.disallowedSpecialFilters.RemoveAll((Predicate<SpecialThingFilterDef>)((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf))));
			foreach (SpecialThingFilterDef allDef in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				if (allDef.configurable && (exceptedFilters == null || !exceptedFilters.Contains(allDef)))
				{
					this.disallowedSpecialFilters.Add(allDef);
				}
			}
			if ((object)this.settingsChangedCallback != null)
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
			this.disallowedSpecialFilters.RemoveAll((Predicate<SpecialThingFilterDef>)((SpecialThingFilterDef sf) => sf.configurable));
			if ((object)this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		public virtual bool Allows(Thing t)
		{
			bool result;
			if (!this.Allows(t.def))
			{
				result = false;
			}
			else
			{
				if (t.def.useHitPoints)
				{
					float f = (float)t.HitPoints / (float)t.MaxHitPoints;
					f = GenMath.RoundedHundredth(f);
					if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(f)))
					{
						result = false;
						goto IL_00f7;
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
						result = false;
						goto IL_00f7;
					}
				}
				for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
				{
					if (this.disallowedSpecialFilters[i].Worker.Matches(t))
						goto IL_00d3;
				}
				result = true;
			}
			goto IL_00f7;
			IL_00d3:
			result = false;
			goto IL_00f7;
			IL_00f7:
			return result;
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
			return (!this.AllowedThingDefs.Any((Func<ThingDef, bool>)((ThingDef def) => !def.alwaysHaulable))) ? ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways) : ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
		}

		public override string ToString()
		{
			return this.Summary;
		}
	}
}
