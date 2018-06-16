using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FCB RID: 4043
	public class ThingFilter : IExposable
	{
		// Token: 0x06006199 RID: 24985 RVA: 0x003128C8 File Offset: 0x00310CC8
		public ThingFilter()
		{
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x003129A0 File Offset: 0x00310DA0
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x0600619B RID: 24987 RVA: 0x00312A80 File Offset: 0x00310E80
		public string Summary
		{
			get
			{
				string result;
				if (!this.customSummary.NullOrEmpty())
				{
					result = this.customSummary;
				}
				else if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty<string>() && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.40282347E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					result = this.thingDefs[0].label;
				}
				else if (this.thingDefs.NullOrEmpty<ThingDef>() && this.categories != null && this.categories.Count == 1 && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.40282347E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					result = DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label;
				}
				else if (this.allowedDefs.Count == 1)
				{
					result = this.allowedDefs.First<ThingDef>().label;
				}
				else
				{
					result = "UsableIngredients".Translate();
				}
				return result;
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x0600619C RID: 24988 RVA: 0x00312D28 File Offset: 0x00311128
		public ThingRequest BestThingRequest
		{
			get
			{
				ThingRequest result;
				if (this.allowedDefs.Count == 1)
				{
					result = ThingRequest.ForDef(this.allowedDefs.First<ThingDef>());
				}
				else
				{
					bool flag = true;
					bool flag2 = true;
					foreach (ThingDef thingDef in this.allowedDefs)
					{
						if (!thingDef.EverHaulable)
						{
							flag = false;
						}
						if (thingDef.category != ThingCategory.Pawn)
						{
							flag2 = false;
						}
					}
					if (flag)
					{
						result = ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
					}
					else if (flag2)
					{
						result = ThingRequest.ForGroup(ThingRequestGroup.Pawn);
					}
					else
					{
						result = ThingRequest.ForGroup(ThingRequestGroup.Everything);
					}
				}
				return result;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x0600619D RID: 24989 RVA: 0x00312DFC File Offset: 0x003111FC
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x0600619E RID: 24990 RVA: 0x00312E1C File Offset: 0x0031121C
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x0600619F RID: 24991 RVA: 0x00312E38 File Offset: 0x00311238
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060061A0 RID: 24992 RVA: 0x00312E74 File Offset: 0x00311274
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060061A1 RID: 24993 RVA: 0x00312E94 File Offset: 0x00311294
		// (set) Token: 0x060061A2 RID: 24994 RVA: 0x00312EAF File Offset: 0x003112AF
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

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060061A3 RID: 24995 RVA: 0x00312EBC File Offset: 0x003112BC
		// (set) Token: 0x060061A4 RID: 24996 RVA: 0x00312ED7 File Offset: 0x003112D7
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

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060061A5 RID: 24997 RVA: 0x00312EE4 File Offset: 0x003112E4
		// (set) Token: 0x060061A6 RID: 24998 RVA: 0x00312F26 File Offset: 0x00311326
		public TreeNode_ThingCategory DisplayRootCategory
		{
			get
			{
				if (this.displayRootCategoryInt == null)
				{
					this.RecalculateDisplayRootCategory();
				}
				TreeNode_ThingCategory rootNode;
				if (this.displayRootCategoryInt == null)
				{
					rootNode = ThingCategoryNodeDatabase.RootNode;
				}
				else
				{
					rootNode = this.displayRootCategoryInt;
				}
				return rootNode;
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

		// Token: 0x060061A7 RID: 24999 RVA: 0x00312F48 File Offset: 0x00311348
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, new object[0]);
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
		}

		// Token: 0x060061A8 RID: 25000 RVA: 0x00312FB4 File Offset: 0x003113B4
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
						Log.Error("ThingFilter could not find thing def named " + this.thingDefs[j], false);
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
			if (this.tradeTagsToAllow != null)
			{
				for (int l = 0; l < this.tradeTagsToAllow.Count; l++)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int m = 0; m < allDefsListForReading.Count; m++)
					{
						ThingDef thingDef = allDefsListForReading[m];
						if (thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tradeTagsToAllow[l]))
						{
							this.SetAllow(thingDef, true);
						}
					}
				}
			}
			if (this.tradeTagsToDisallow != null)
			{
				for (int n = 0; n < this.tradeTagsToDisallow.Count; n++)
				{
					List<ThingDef> allDefsListForReading2 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num = 0; num < allDefsListForReading2.Count; num++)
					{
						ThingDef thingDef2 = allDefsListForReading2[num];
						if (thingDef2.tradeTags != null && thingDef2.tradeTags.Contains(this.tradeTagsToDisallow[n]))
						{
							this.SetAllow(thingDef2, false);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToAllow != null)
			{
				for (int num2 = 0; num2 < this.thingSetMakerTagsToAllow.Count; num2++)
				{
					List<ThingDef> allDefsListForReading3 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num3 = 0; num3 < allDefsListForReading3.Count; num3++)
					{
						ThingDef thingDef3 = allDefsListForReading3[num3];
						if (thingDef3.thingSetMakerTags != null && thingDef3.thingSetMakerTags.Contains(this.thingSetMakerTagsToAllow[num2]))
						{
							this.SetAllow(thingDef3, true);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToDisallow != null)
			{
				for (int num4 = 0; num4 < this.thingSetMakerTagsToDisallow.Count; num4++)
				{
					List<ThingDef> allDefsListForReading4 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num5 = 0; num5 < allDefsListForReading4.Count; num5++)
					{
						ThingDef thingDef4 = allDefsListForReading4[num5];
						if (thingDef4.thingSetMakerTags != null && thingDef4.thingSetMakerTags.Contains(this.thingSetMakerTagsToDisallow[num4]))
						{
							this.SetAllow(thingDef4, false);
						}
					}
				}
			}
			if (this.disallowedCategories != null)
			{
				for (int num6 = 0; num6 < this.disallowedCategories.Count; num6++)
				{
					ThingCategoryDef named2 = DefDatabase<ThingCategoryDef>.GetNamed(this.disallowedCategories[num6], true);
					if (named2 != null)
					{
						this.SetAllow(named2, false, null, null);
					}
				}
			}
			if (this.specialFiltersToAllow != null)
			{
				for (int num7 = 0; num7 < this.specialFiltersToAllow.Count; num7++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToAllow[num7]), true);
				}
			}
			if (this.specialFiltersToDisallow != null)
			{
				for (int num8 = 0; num8 < this.specialFiltersToDisallow.Count; num8++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToDisallow[num8]), false);
				}
			}
			if (this.stuffCategoriesToAllow != null)
			{
				for (int num9 = 0; num9 < this.stuffCategoriesToAllow.Count; num9++)
				{
					this.SetAllow(this.stuffCategoriesToAllow[num9], true);
				}
			}
			if (this.allowAllWhoCanMake != null)
			{
				for (int num10 = 0; num10 < this.allowAllWhoCanMake.Count; num10++)
				{
					this.SetAllowAllWhoCanMake(this.allowAllWhoCanMake[num10]);
				}
			}
			if (this.disallowWorsePreferability != FoodPreferability.Undefined)
			{
				List<ThingDef> allDefsListForReading5 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num11 = 0; num11 < allDefsListForReading5.Count; num11++)
				{
					ThingDef thingDef5 = allDefsListForReading5[num11];
					if (thingDef5.IsIngestible && thingDef5.ingestible.preferability != FoodPreferability.Undefined && thingDef5.ingestible.preferability < this.disallowWorsePreferability)
					{
						this.SetAllow(thingDef5, false);
					}
				}
			}
			if (this.disallowInedibleByHuman)
			{
				List<ThingDef> allDefsListForReading6 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num12 = 0; num12 < allDefsListForReading6.Count; num12++)
				{
					ThingDef thingDef6 = allDefsListForReading6[num12];
					if (thingDef6.IsIngestible && !ThingDefOf.Human.race.CanEverEat(thingDef6))
					{
						this.SetAllow(thingDef6, false);
					}
				}
			}
			if (this.allowWithComp != null)
			{
				List<ThingDef> allDefsListForReading7 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num13 = 0; num13 < allDefsListForReading7.Count; num13++)
				{
					ThingDef thingDef7 = allDefsListForReading7[num13];
					if (thingDef7.HasComp(this.allowWithComp))
					{
						this.SetAllow(thingDef7, true);
					}
				}
			}
			if (this.disallowWithComp != null)
			{
				List<ThingDef> allDefsListForReading8 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading8.Count; num14++)
				{
					ThingDef thingDef8 = allDefsListForReading8[num14];
					if (thingDef8.HasComp(this.disallowWithComp))
					{
						this.SetAllow(thingDef8, false);
					}
				}
			}
			if (this.disallowCheaperThan != -3.40282347E+38f)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef9 in this.allowedDefs)
				{
					if (thingDef9.BaseMarketValue < this.disallowCheaperThan)
					{
						list.Add(thingDef9);
					}
				}
				for (int num15 = 0; num15 < list.Count; num15++)
				{
					this.SetAllow(list[num15], false);
				}
			}
			if (this.disallowedThingDefs != null)
			{
				for (int num16 = 0; num16 < this.disallowedThingDefs.Count; num16++)
				{
					if (this.disallowedThingDefs[num16] != null)
					{
						this.SetAllow(this.disallowedThingDefs[num16], false);
					}
					else
					{
						Log.Error("ThingFilter could not find excepted thing def named " + this.disallowedThingDefs[num16], false);
					}
				}
			}
			this.RecalculateDisplayRootCategory();
		}

		// Token: 0x060061A9 RID: 25001 RVA: 0x00313734 File Offset: 0x00311B34
		public void RecalculateDisplayRootCategory()
		{
			this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in ThingCategoryNodeDatabase.AllThingCategoryNodes)
			{
				bool flag = false;
				bool flag2 = false;
				foreach (ThingDef value in this.allowedDefs)
				{
					if (treeNode_ThingCategory.catDef.DescendantThingDefs.Contains(value))
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
					this.DisplayRootCategory = treeNode_ThingCategory;
				}
			}
		}

		// Token: 0x060061AA RID: 25002 RVA: 0x00313814 File Offset: 0x00311C14
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
				foreach (ThingDef thingDef in this.DisplayRootCategory.catDef.DescendantThingDefs)
				{
					if (thingDef.useHitPoints)
					{
						this.allowedHitPointsConfigurable = true;
					}
					if (thingDef.HasComp(typeof(CompQuality)))
					{
						this.allowedQualitiesConfigurable = true;
					}
					if (this.allowedHitPointsConfigurable && this.allowedQualitiesConfigurable)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060061AB RID: 25003 RVA: 0x003138EC File Offset: 0x00311CEC
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

		// Token: 0x060061AC RID: 25004 RVA: 0x00313944 File Offset: 0x00311D44
		public virtual void CopyAllowancesFrom(ThingFilter other)
		{
			this.allowedDefs.Clear();
			foreach (ThingDef thingDef in ThingFilter.AllStorableThingDefs)
			{
				this.SetAllow(thingDef, other.Allows(thingDef));
			}
			this.disallowedSpecialFilters = other.disallowedSpecialFilters.ListFullCopyOrNull<SpecialThingFilterDef>();
			this.allowedHitPointsPercents = other.allowedHitPointsPercents;
			this.allowedHitPointsConfigurable = other.allowedHitPointsConfigurable;
			this.allowedQualities = other.allowedQualities;
			this.allowedQualitiesConfigurable = other.allowedQualitiesConfigurable;
			this.thingDefs = other.thingDefs.ListFullCopyOrNull<ThingDef>();
			this.categories = other.categories.ListFullCopyOrNull<string>();
			this.tradeTagsToAllow = other.tradeTagsToAllow.ListFullCopyOrNull<string>();
			this.tradeTagsToDisallow = other.tradeTagsToDisallow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToAllow = other.thingSetMakerTagsToAllow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToDisallow = other.thingSetMakerTagsToDisallow.ListFullCopyOrNull<string>();
			this.disallowedCategories = other.disallowedCategories.ListFullCopyOrNull<string>();
			this.specialFiltersToAllow = other.specialFiltersToAllow.ListFullCopyOrNull<string>();
			this.specialFiltersToDisallow = other.specialFiltersToDisallow.ListFullCopyOrNull<string>();
			this.stuffCategoriesToAllow = other.stuffCategoriesToAllow.ListFullCopyOrNull<StuffCategoryDef>();
			this.allowAllWhoCanMake = other.allowAllWhoCanMake.ListFullCopyOrNull<ThingDef>();
			this.disallowWorsePreferability = other.disallowWorsePreferability;
			this.disallowInedibleByHuman = other.disallowInedibleByHuman;
			this.allowWithComp = other.allowWithComp;
			this.disallowWithComp = other.disallowWithComp;
			this.disallowCheaperThan = other.disallowCheaperThan;
			this.disallowedThingDefs = other.disallowedThingDefs.ListFullCopyOrNull<ThingDef>();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060061AD RID: 25005 RVA: 0x00313B14 File Offset: 0x00311F14
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

		// Token: 0x060061AE RID: 25006 RVA: 0x00313B70 File Offset: 0x00311F70
		public void SetAllow(SpecialThingFilterDef sfDef, bool allow)
		{
			if (sfDef.configurable)
			{
				if (allow != this.Allows(sfDef))
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
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x00313C00 File Offset: 0x00312000
		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.", false);
			}
			foreach (ThingDef thingDef in categoryDef.DescendantThingDefs)
			{
				if (exceptedDefs == null || !exceptedDefs.Contains(thingDef))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in categoryDef.DescendantSpecialThingFilterDefs)
			{
				if (exceptedFilters == null || !exceptedFilters.Contains(specialThingFilterDef))
				{
					this.SetAllow(specialThingFilterDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x00313CFC File Offset: 0x003120FC
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

		// Token: 0x060061B1 RID: 25009 RVA: 0x00313D70 File Offset: 0x00312170
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

		// Token: 0x060061B2 RID: 25010 RVA: 0x00313DE4 File Offset: 0x003121E4
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Manufactured, true, null, null);
				this.SetAllow(ThingCategoryDefOf.ResourcesRaw, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Items, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Buildings, true, null, null);
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

		// Token: 0x060061B3 RID: 25011 RVA: 0x00313EA8 File Offset: 0x003122A8
		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x00313F10 File Offset: 0x00312310
		public void SetAllowAll(ThingFilter parentFilter)
		{
			this.allowedDefs.Clear();
			if (parentFilter != null)
			{
				foreach (ThingDef item in parentFilter.allowedDefs)
				{
					this.allowedDefs.Add(item);
				}
			}
			else
			{
				foreach (ThingDef item2 in ThingFilter.AllStorableThingDefs)
				{
					this.allowedDefs.Add(item2);
				}
			}
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable);
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x00314020 File Offset: 0x00312420
		public virtual bool Allows(Thing t)
		{
			t = t.GetInnerIfMinified();
			bool result;
			if (!this.Allows(t.def))
			{
				result = false;
			}
			else
			{
				if (t.def.useHitPoints)
				{
					float num = (float)t.HitPoints / (float)t.MaxHitPoints;
					num = GenMath.RoundedHundredth(num);
					if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(num)))
					{
						return false;
					}
				}
				if (this.allowedQualities != QualityRange.All && t.def.FollowQualityThingFilter())
				{
					QualityCategory p;
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
				result = true;
			}
			return result;
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x00314130 File Offset: 0x00312530
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x00314154 File Offset: 0x00312554
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x00314178 File Offset: 0x00312578
		public ThingRequest GetThingRequest()
		{
			ThingRequest result;
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				result = ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			else
			{
				result = ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
			}
			return result;
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x003141CC File Offset: 0x003125CC
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x04003FC2 RID: 16322
		[Unsaved]
		private Action settingsChangedCallback;

		// Token: 0x04003FC3 RID: 16323
		[Unsaved]
		private TreeNode_ThingCategory displayRootCategoryInt = null;

		// Token: 0x04003FC4 RID: 16324
		[Unsaved]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x04003FC5 RID: 16325
		[Unsaved]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04003FC6 RID: 16326
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x04003FC7 RID: 16327
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x04003FC8 RID: 16328
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x04003FC9 RID: 16329
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x04003FCA RID: 16330
		[MustTranslate]
		public string customSummary = null;

		// Token: 0x04003FCB RID: 16331
		private List<ThingDef> thingDefs = null;

		// Token: 0x04003FCC RID: 16332
		[NoTranslate]
		private List<string> categories = null;

		// Token: 0x04003FCD RID: 16333
		[NoTranslate]
		private List<string> tradeTagsToAllow = null;

		// Token: 0x04003FCE RID: 16334
		[NoTranslate]
		private List<string> tradeTagsToDisallow = null;

		// Token: 0x04003FCF RID: 16335
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow = null;

		// Token: 0x04003FD0 RID: 16336
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow = null;

		// Token: 0x04003FD1 RID: 16337
		[NoTranslate]
		private List<string> disallowedCategories = null;

		// Token: 0x04003FD2 RID: 16338
		[NoTranslate]
		private List<string> specialFiltersToAllow = null;

		// Token: 0x04003FD3 RID: 16339
		[NoTranslate]
		private List<string> specialFiltersToDisallow = null;

		// Token: 0x04003FD4 RID: 16340
		private List<StuffCategoryDef> stuffCategoriesToAllow = null;

		// Token: 0x04003FD5 RID: 16341
		private List<ThingDef> allowAllWhoCanMake = null;

		// Token: 0x04003FD6 RID: 16342
		private FoodPreferability disallowWorsePreferability = FoodPreferability.Undefined;

		// Token: 0x04003FD7 RID: 16343
		private bool disallowInedibleByHuman = false;

		// Token: 0x04003FD8 RID: 16344
		private Type allowWithComp = null;

		// Token: 0x04003FD9 RID: 16345
		private Type disallowWithComp = null;

		// Token: 0x04003FDA RID: 16346
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x04003FDB RID: 16347
		private List<ThingDef> disallowedThingDefs = null;
	}
}
