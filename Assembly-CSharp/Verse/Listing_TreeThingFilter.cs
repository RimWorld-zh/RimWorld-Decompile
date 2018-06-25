using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class Listing_TreeThingFilter : Listing_Tree
	{
		private ThingFilter filter;

		private ThingFilter parentFilter;

		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		private List<ThingDef> forceHiddenDefs = null;

		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		private List<ThingDef> suppressSmallVolumeTags;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache0;

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

		private bool Visible(TreeNode_ThingCategory node)
		{
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

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

		[CompilerGenerated]
		private static string <DoCategoryChildren>m__0(ThingDef n)
		{
			return n.label;
		}

		[CompilerGenerated]
		private bool <CalculateHiddenSpecialFilters>m__1(ThingDef x)
		{
			return this.parentFilter.Allows(x);
		}
	}
}
