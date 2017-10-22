using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class Listing_TreeThingFilter : Listing_Tree
	{
		private ThingFilter filter;

		private ThingFilter parentFilter;

		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		private List<ThingDef> forceHiddenDefs;

		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		private List<ThingDef> suppressSmallVolumeTags;

		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags)
		{
			this.filter = filter;
			this.parentFilter = parentFilter;
			if (forceHiddenDefs != null)
			{
				this.forceHiddenDefs = forceHiddenDefs.ToList();
			}
			if (forceHiddenFilters != null)
			{
				this.tempForceHiddenSpecialFilters = forceHiddenFilters.ToList();
			}
			this.suppressSmallVolumeTags = suppressSmallVolumeTags;
		}

		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, bool isRoot = false)
		{
			if (isRoot)
			{
				foreach (SpecialThingFilterDef parentsSpecialThingFilterDef in node.catDef.ParentsSpecialThingFilterDefs)
				{
					if (this.Visible(parentsSpecialThingFilterDef))
					{
						this.DoSpecialFilter(parentsSpecialThingFilterDef, indentLevel);
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
			foreach (TreeNode_ThingCategory childCategoryNode in node.ChildCategoryNodes)
			{
				if (this.Visible(childCategoryNode))
				{
					this.DoCategory(childCategoryNode, indentLevel, openMask);
				}
			}
			foreach (ThingDef item in from n in node.catDef.childThingDefs
			orderby n.label
			select n)
			{
				if (this.Visible(item))
				{
					this.DoThingDef(item, indentLevel);
				}
			}
		}

		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (sfDef.configurable)
			{
				base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel);
				bool flag;
				bool flag2 = flag = this.filter.Allows(sfDef);
				Widgets.Checkbox(new Vector2(this.LabelWidth, base.curY), ref flag2, base.lineHeight, false);
				if (flag2 != flag)
				{
					this.filter.SetAllow(sfDef, flag2);
				}
				base.EndLine();
			}
		}

		public void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask)
		{
			base.OpenCloseWidget(node, indentLevel, openMask);
			base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel);
			MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
			if (Widgets.CheckboxMulti(new Vector2(this.LabelWidth, base.curY), multiCheckboxState, base.lineHeight))
			{
				bool allow = (byte)((multiCheckboxState == MultiCheckboxState.Off) ? 1 : 0) != 0;
				this.filter.SetAllow(node.catDef, allow, this.forceHiddenDefs, this.hiddenSpecialFilters);
			}
			base.EndLine();
			if (node.IsOpen(openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, false);
			}
		}

		private void DoThingDef(ThingDef tDef, int nestLevel)
		{
			int num;
			if ((this.suppressSmallVolumeTags == null || !this.suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff)
			{
				num = (tDef.smallVolume ? 1 : 0);
				goto IL_0030;
			}
			num = 0;
			goto IL_0030;
			IL_0030:
			bool flag = (byte)num != 0;
			string text = tDef.description;
			if (flag)
			{
				text = text + "\n\n" + "ThisIsSmallVolume".Translate(10.ToStringCached());
			}
			base.LabelLeft(tDef.LabelCap, text, nestLevel);
			if (flag)
			{
				Rect rect = new Rect((float)(this.LabelWidth - 30.0), base.curY, 30f, 30f);
				Text.Font = GameFont.Tiny;
				GUI.color = Color.gray;
				Widgets.Label(rect, "x" + 10.ToStringCached());
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
			}
			bool flag2;
			bool flag3 = flag2 = this.filter.Allows(tDef);
			Widgets.Checkbox(new Vector2(this.LabelWidth, base.curY), ref flag3, base.lineHeight, false);
			if (flag3 != flag2)
			{
				this.filter.SetAllow(tDef, flag3);
			}
			base.EndLine();
		}

		public MultiCheckboxState AllowanceStateOf(TreeNode_ThingCategory cat)
		{
			int num = 0;
			int num2 = 0;
			foreach (ThingDef descendantThingDef in cat.catDef.DescendantThingDefs)
			{
				if (this.Visible(descendantThingDef))
				{
					num++;
					if (this.filter.Allows(descendantThingDef))
					{
						num2++;
					}
				}
			}
			foreach (SpecialThingFilterDef descendantSpecialThingFilterDef in cat.catDef.DescendantSpecialThingFilterDefs)
			{
				if (this.Visible(descendantSpecialThingFilterDef))
				{
					num++;
					if (this.filter.Allows(descendantSpecialThingFilterDef))
					{
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return MultiCheckboxState.Off;
			}
			if (num == num2)
			{
				return MultiCheckboxState.On;
			}
			return MultiCheckboxState.Partial;
		}

		private bool Visible(ThingDef td)
		{
			if (td.menuHidden)
			{
				return false;
			}
			if (this.forceHiddenDefs != null && this.forceHiddenDefs.Contains(td))
			{
				return false;
			}
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
			return true;
		}

		private bool Visible(TreeNode_ThingCategory node)
		{
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		private bool Visible(SpecialThingFilterDef filter)
		{
			if (this.parentFilter != null && !this.parentFilter.Allows(filter))
			{
				return false;
			}
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
			return true;
		}

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
			foreach (SpecialThingFilterDef item in enumerable)
			{
				bool flag = false;
				foreach (ThingDef item2 in enumerable2)
				{
					if (item.Worker.CanEverMatch(item2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.hiddenSpecialFilters.Add(item);
				}
			}
		}
	}
}
