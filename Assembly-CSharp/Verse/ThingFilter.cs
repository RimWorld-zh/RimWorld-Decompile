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
		// Token: 0x060061C0 RID: 25024 RVA: 0x00314A78 File Offset: 0x00312E78
		public ThingFilter()
		{
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00314B50 File Offset: 0x00312F50
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060061C2 RID: 25026 RVA: 0x00314C30 File Offset: 0x00313030
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

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x060061C3 RID: 25027 RVA: 0x00314ED8 File Offset: 0x003132D8
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

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060061C4 RID: 25028 RVA: 0x00314FAC File Offset: 0x003133AC
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060061C5 RID: 25029 RVA: 0x00314FCC File Offset: 0x003133CC
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060061C6 RID: 25030 RVA: 0x00314FE8 File Offset: 0x003133E8
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060061C7 RID: 25031 RVA: 0x00315024 File Offset: 0x00313424
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060061C8 RID: 25032 RVA: 0x00315044 File Offset: 0x00313444
		// (set) Token: 0x060061C9 RID: 25033 RVA: 0x0031505F File Offset: 0x0031345F
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

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060061CA RID: 25034 RVA: 0x0031506C File Offset: 0x0031346C
		// (set) Token: 0x060061CB RID: 25035 RVA: 0x00315087 File Offset: 0x00313487
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

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060061CC RID: 25036 RVA: 0x00315094 File Offset: 0x00313494
		// (set) Token: 0x060061CD RID: 25037 RVA: 0x003150D6 File Offset: 0x003134D6
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

		// Token: 0x060061CE RID: 25038 RVA: 0x003150F8 File Offset: 0x003134F8
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, new object[0]);
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x00315164 File Offset: 0x00313564
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

		// Token: 0x060061D0 RID: 25040 RVA: 0x003158E4 File Offset: 0x00313CE4
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

		// Token: 0x060061D1 RID: 25041 RVA: 0x003159C4 File Offset: 0x00313DC4
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

		// Token: 0x060061D2 RID: 25042 RVA: 0x00315A9C File Offset: 0x00313E9C
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

		// Token: 0x060061D3 RID: 25043 RVA: 0x00315AF4 File Offset: 0x00313EF4
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

		// Token: 0x060061D4 RID: 25044 RVA: 0x00315CC4 File Offset: 0x003140C4
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

		// Token: 0x060061D5 RID: 25045 RVA: 0x00315D20 File Offset: 0x00314120
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

		// Token: 0x060061D6 RID: 25046 RVA: 0x00315DB0 File Offset: 0x003141B0
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

		// Token: 0x060061D7 RID: 25047 RVA: 0x00315EAC File Offset: 0x003142AC
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

		// Token: 0x060061D8 RID: 25048 RVA: 0x00315F20 File Offset: 0x00314320
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

		// Token: 0x060061D9 RID: 25049 RVA: 0x00315F94 File Offset: 0x00314394
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

		// Token: 0x060061DA RID: 25050 RVA: 0x00316058 File Offset: 0x00314458
		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x003160C0 File Offset: 0x003144C0
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

		// Token: 0x060061DC RID: 25052 RVA: 0x003161D0 File Offset: 0x003145D0
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

		// Token: 0x060061DD RID: 25053 RVA: 0x003162E0 File Offset: 0x003146E0
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x00316304 File Offset: 0x00314704
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x00316328 File Offset: 0x00314728
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

		// Token: 0x060061E0 RID: 25056 RVA: 0x0031637C File Offset: 0x0031477C
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x04003FDE RID: 16350
		[Unsaved]
		private Action settingsChangedCallback;

		// Token: 0x04003FDF RID: 16351
		[Unsaved]
		private TreeNode_ThingCategory displayRootCategoryInt = null;

		// Token: 0x04003FE0 RID: 16352
		[Unsaved]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x04003FE1 RID: 16353
		[Unsaved]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04003FE2 RID: 16354
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x04003FE3 RID: 16355
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x04003FE4 RID: 16356
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x04003FE5 RID: 16357
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x04003FE6 RID: 16358
		[MustTranslate]
		public string customSummary = null;

		// Token: 0x04003FE7 RID: 16359
		private List<ThingDef> thingDefs = null;

		// Token: 0x04003FE8 RID: 16360
		[NoTranslate]
		private List<string> categories = null;

		// Token: 0x04003FE9 RID: 16361
		[NoTranslate]
		private List<string> tradeTagsToAllow = null;

		// Token: 0x04003FEA RID: 16362
		[NoTranslate]
		private List<string> tradeTagsToDisallow = null;

		// Token: 0x04003FEB RID: 16363
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow = null;

		// Token: 0x04003FEC RID: 16364
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow = null;

		// Token: 0x04003FED RID: 16365
		[NoTranslate]
		private List<string> disallowedCategories = null;

		// Token: 0x04003FEE RID: 16366
		[NoTranslate]
		private List<string> specialFiltersToAllow = null;

		// Token: 0x04003FEF RID: 16367
		[NoTranslate]
		private List<string> specialFiltersToDisallow = null;

		// Token: 0x04003FF0 RID: 16368
		private List<StuffCategoryDef> stuffCategoriesToAllow = null;

		// Token: 0x04003FF1 RID: 16369
		private List<ThingDef> allowAllWhoCanMake = null;

		// Token: 0x04003FF2 RID: 16370
		private FoodPreferability disallowWorsePreferability = FoodPreferability.Undefined;

		// Token: 0x04003FF3 RID: 16371
		private bool disallowInedibleByHuman = false;

		// Token: 0x04003FF4 RID: 16372
		private Type allowWithComp = null;

		// Token: 0x04003FF5 RID: 16373
		private Type disallowWithComp = null;

		// Token: 0x04003FF6 RID: 16374
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x04003FF7 RID: 16375
		private List<ThingDef> disallowedThingDefs = null;
	}
}
