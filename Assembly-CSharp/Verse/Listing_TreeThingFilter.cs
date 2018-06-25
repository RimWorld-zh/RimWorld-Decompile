using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E84 RID: 3716
	public class Listing_TreeThingFilter : Listing_Tree
	{
		// Token: 0x040039FA RID: 14842
		private ThingFilter filter;

		// Token: 0x040039FB RID: 14843
		private ThingFilter parentFilter;

		// Token: 0x040039FC RID: 14844
		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		// Token: 0x040039FD RID: 14845
		private List<ThingDef> forceHiddenDefs = null;

		// Token: 0x040039FE RID: 14846
		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		// Token: 0x040039FF RID: 14847
		private List<ThingDef> suppressSmallVolumeTags;

		// Token: 0x060057AB RID: 22443 RVA: 0x002D018C File Offset: 0x002CE58C
		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags)
		{
			this.filter = filter;
			this.parentFilter = parentFilter;
			if (forceHiddenDefs != null)
			{
				this.forceHiddenDefs = forceHiddenDefs.ToList<ThingDef>();
			}
			if (forceHiddenFilters != null)
			{
				this.tempForceHiddenSpecialFilters = forceHiddenFilters.ToList<SpecialThingFilterDef>();
			}
			this.suppressSmallVolumeTags = suppressSmallVolumeTags;
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x002D01E4 File Offset: 0x002CE5E4
		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool isRoot = false)
		{
			if (isRoot)
			{
				foreach (SpecialThingFilterDef sfDef in node.catDef.ParentsSpecialThingFilterDefs)
				{
					if (this.Visible(sfDef))
					{
						this.DoSpecialFilter(sfDef, indentLevel);
					}
				}
			}
			List<SpecialThingFilterDef> childSpecialFilters = node.catDef.childSpecialFilters;
			for (int i = 0; i < childSpecialFilters.Count; i++)
			{
				if (this.Visible(childSpecialFilters[i]))
				{
					this.DoSpecialFilter(childSpecialFilters[i], indentLevel);
				}
			}
			foreach (TreeNode_ThingCategory node2 in node.ChildCategoryNodes)
			{
				if (this.Visible(node2))
				{
					this.DoCategory(node2, indentLevel, openMask, map);
				}
			}
			foreach (ThingDef thingDef in from n in node.catDef.childThingDefs
			orderby n.label
			select n)
			{
				if (this.Visible(thingDef))
				{
					this.DoThingDef(thingDef, indentLevel, map);
				}
			}
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x002D0390 File Offset: 0x002CE790
		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (sfDef.configurable)
			{
				base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel);
				bool flag = this.filter.Allows(sfDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(sfDef, flag);
				}
				base.EndLine();
			}
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x002D0418 File Offset: 0x002CE818
		public void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map)
		{
			base.OpenCloseWidget(node, indentLevel, openMask);
			base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel);
			MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
			MultiCheckboxState multiCheckboxState2 = Widgets.CheckboxMulti(new Rect(this.LabelWidth, this.curY, this.lineHeight, this.lineHeight), multiCheckboxState, true);
			if (multiCheckboxState != multiCheckboxState2)
			{
				this.filter.SetAllow(node.catDef, multiCheckboxState2 == MultiCheckboxState.On, this.forceHiddenDefs, this.hiddenSpecialFilters);
			}
			base.EndLine();
			if (node.IsOpen(openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, map, false);
			}
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x002D04C0 File Offset: 0x002CE8C0
		private void DoThingDef(ThingDef tDef, int nestLevel, Map map)
		{
			bool flag = (this.suppressSmallVolumeTags == null || !this.suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
			string text = tDef.DescriptionDetailed;
			if (flag)
			{
				text = text + "\n\n" + "ThisIsSmallVolume".Translate(new object[]
				{
					10.ToStringCached()
				});
			}
			base.LabelLeft(tDef.LabelCap, text, nestLevel);
			if (map != null)
			{
				int count = map.resourceCounter.GetCount(tDef);
				if (count > 0)
				{
					Rect rect = new Rect(this.LabelWidth - 40f, this.curY, 40f, 40f);
					Text.Font = GameFont.Tiny;
					GUI.color = Color.gray;
					Widgets.Label(rect, count.ToStringCached());
					Text.Font = GameFont.Small;
					GUI.color = Color.white;
				}
			}
			if (flag)
			{
				Rect rect2 = new Rect(this.LabelWidth - 20f, this.curY, 20f, 20f);
				Text.Font = GameFont.Tiny;
				GUI.color = Color.gray;
				Widgets.Label(rect2, "/" + 10.ToStringCached());
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
			}
			bool flag2 = this.filter.Allows(tDef);
			bool flag3 = flag2;
			Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag2, this.lineHeight, false, true, null, null);
			if (flag2 != flag3)
			{
				this.filter.SetAllow(tDef, flag2);
			}
			base.EndLine();
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x002D0664 File Offset: 0x002CEA64
		public MultiCheckboxState AllowanceStateOf(TreeNode_ThingCategory cat)
		{
			int num = 0;
			int num2 = 0;
			foreach (ThingDef thingDef in cat.catDef.DescendantThingDefs)
			{
				if (this.Visible(thingDef))
				{
					num++;
					if (this.filter.Allows(thingDef))
					{
						num2++;
					}
				}
			}
			bool flag = false;
			foreach (SpecialThingFilterDef sf in cat.catDef.DescendantSpecialThingFilterDefs)
			{
				if (this.Visible(sf) && !this.filter.Allows(sf))
				{
					flag = true;
					break;
				}
			}
			MultiCheckboxState result;
			if (num2 == 0)
			{
				result = MultiCheckboxState.Off;
			}
			else if (num == num2 && !flag)
			{
				result = MultiCheckboxState.On;
			}
			else
			{
				result = MultiCheckboxState.Partial;
			}
			return result;
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x002D0794 File Offset: 0x002CEB94
		private bool Visible(ThingDef td)
		{
			bool result;
			if (td.menuHidden)
			{
				result = false;
			}
			else if (this.forceHiddenDefs != null && this.forceHiddenDefs.Contains(td))
			{
				result = false;
			}
			else
			{
				if (this.parentFilter != null)
				{
					if (!this.parentFilter.Allows(td))
					{
						return false;
					}
					if (this.parentFilter.IsAlwaysDisallowedDueToSpecialFilters(td))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x002D081C File Offset: 0x002CEC1C
		private bool Visible(TreeNode_ThingCategory node)
		{
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x002D0850 File Offset: 0x002CEC50
		private bool Visible(SpecialThingFilterDef filter)
		{
			bool result;
			if (this.parentFilter != null && !this.parentFilter.Allows(filter))
			{
				result = false;
			}
			else
			{
				if (this.hiddenSpecialFilters == null)
				{
					this.CalculateHiddenSpecialFilters();
				}
				for (int i = 0; i < this.hiddenSpecialFilters.Count; i++)
				{
					if (this.hiddenSpecialFilters[i] == filter)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x002D08D4 File Offset: 0x002CECD4
		private void CalculateHiddenSpecialFilters()
		{
			this.hiddenSpecialFilters = new List<SpecialThingFilterDef>();
			if (this.tempForceHiddenSpecialFilters != null)
			{
				this.hiddenSpecialFilters.AddRange(this.tempForceHiddenSpecialFilters);
			}
			IEnumerable<SpecialThingFilterDef> enumerable = this.filter.DisplayRootCategory.catDef.DescendantSpecialThingFilterDefs.Concat(this.filter.DisplayRootCategory.catDef.ParentsSpecialThingFilterDefs);
			IEnumerable<ThingDef> enumerable2 = this.filter.DisplayRootCategory.catDef.DescendantThingDefs;
			if (this.parentFilter != null)
			{
				enumerable2 = from x in enumerable2
				where this.parentFilter.Allows(x)
				select x;
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in enumerable)
			{
				bool flag = false;
				foreach (ThingDef def in enumerable2)
				{
					if (specialThingFilterDef.Worker.CanEverMatch(def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.hiddenSpecialFilters.Add(specialThingFilterDef);
				}
			}
		}
	}
}
