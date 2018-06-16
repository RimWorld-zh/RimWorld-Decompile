using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA8 RID: 2984
	public class ThingCategoryDef : Def
	{
		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x0600407B RID: 16507 RVA: 0x0021E214 File Offset: 0x0021C614
		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				if (this.parent != null)
				{
					yield return this.parent;
					foreach (ThingCategoryDef cat in this.parent.Parents)
					{
						yield return cat;
					}
				}
				yield break;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600407C RID: 16508 RVA: 0x0021E240 File Offset: 0x0021C640
		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				yield return this;
				foreach (ThingCategoryDef child in this.childCategories)
				{
					foreach (ThingCategoryDef subChild in child.ThisAndChildCategoryDefs)
					{
						yield return subChild;
					}
				}
				yield break;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600407D RID: 16509 RVA: 0x0021E26C File Offset: 0x0021C66C
		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				foreach (ThingCategoryDef childCatDef in this.ThisAndChildCategoryDefs)
				{
					foreach (ThingDef def in childCatDef.childThingDefs)
					{
						yield return def;
					}
				}
				yield break;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600407E RID: 16510 RVA: 0x0021E298 File Offset: 0x0021C698
		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef childCatDef in this.ThisAndChildCategoryDefs)
				{
					foreach (SpecialThingFilterDef sf in childCatDef.childSpecialFilters)
					{
						yield return sf;
					}
				}
				yield break;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600407F RID: 16511 RVA: 0x0021E2C4 File Offset: 0x0021C6C4
		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef cat in this.Parents)
				{
					foreach (SpecialThingFilterDef filter in cat.childSpecialFilters)
					{
						yield return filter;
					}
				}
				yield break;
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0021E2EE File Offset: 0x0021C6EE
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

		// Token: 0x06004081 RID: 16513 RVA: 0x0021E320 File Offset: 0x0021C720
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0021E33C File Offset: 0x0021C73C
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x04002B84 RID: 11140
		public ThingCategoryDef parent = null;

		// Token: 0x04002B85 RID: 11141
		[NoTranslate]
		public string iconPath = null;

		// Token: 0x04002B86 RID: 11142
		public bool resourceReadoutRoot = false;

		// Token: 0x04002B87 RID: 11143
		[Unsaved]
		public TreeNode_ThingCategory treeNode = null;

		// Token: 0x04002B88 RID: 11144
		[Unsaved]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x04002B89 RID: 11145
		[Unsaved]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x04002B8A RID: 11146
		[Unsaved]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04002B8B RID: 11147
		[Unsaved]
		public Texture2D icon = BaseContent.BadTex;
	}
}
