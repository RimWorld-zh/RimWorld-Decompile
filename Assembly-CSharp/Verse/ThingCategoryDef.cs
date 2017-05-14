using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class ThingCategoryDef : Def
	{
		public ThingCategoryDef parent;

		[NoTranslate]
		public string iconPath;

		public bool resourceReadoutRoot;

		[Unsaved]
		public TreeNode_ThingCategory treeNode;

		[Unsaved]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		[Unsaved]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		[Unsaved]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		[Unsaved]
		public Texture2D icon = BaseContent.BadTex;

		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				ThingCategoryDef.<>c__Iterator1DF <>c__Iterator1DF = new ThingCategoryDef.<>c__Iterator1DF();
				<>c__Iterator1DF.<>f__this = this;
				ThingCategoryDef.<>c__Iterator1DF expr_0E = <>c__Iterator1DF;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				ThingCategoryDef.<>c__Iterator1E0 <>c__Iterator1E = new ThingCategoryDef.<>c__Iterator1E0();
				<>c__Iterator1E.<>f__this = this;
				ThingCategoryDef.<>c__Iterator1E0 expr_0E = <>c__Iterator1E;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				ThingCategoryDef.<>c__Iterator1E1 <>c__Iterator1E = new ThingCategoryDef.<>c__Iterator1E1();
				<>c__Iterator1E.<>f__this = this;
				ThingCategoryDef.<>c__Iterator1E1 expr_0E = <>c__Iterator1E;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				ThingCategoryDef.<>c__Iterator1E2 <>c__Iterator1E = new ThingCategoryDef.<>c__Iterator1E2();
				<>c__Iterator1E.<>f__this = this;
				ThingCategoryDef.<>c__Iterator1E2 expr_0E = <>c__Iterator1E;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				ThingCategoryDef.<>c__Iterator1E3 <>c__Iterator1E = new ThingCategoryDef.<>c__Iterator1E3();
				<>c__Iterator1E.<>f__this = this;
				ThingCategoryDef.<>c__Iterator1E3 expr_0E = <>c__Iterator1E;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}
	}
}
