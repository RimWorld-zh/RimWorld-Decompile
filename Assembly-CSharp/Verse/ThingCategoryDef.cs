using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA4 RID: 2980
	public class ThingCategoryDef : Def
	{
		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600407F RID: 16511 RVA: 0x0021E984 File Offset: 0x0021CD84
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

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06004080 RID: 16512 RVA: 0x0021E9B0 File Offset: 0x0021CDB0
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

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06004081 RID: 16513 RVA: 0x0021E9DC File Offset: 0x0021CDDC
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

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06004082 RID: 16514 RVA: 0x0021EA08 File Offset: 0x0021CE08
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

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06004083 RID: 16515 RVA: 0x0021EA34 File Offset: 0x0021CE34
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

		// Token: 0x06004084 RID: 16516 RVA: 0x0021EA5E File Offset: 0x0021CE5E
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

		// Token: 0x06004085 RID: 16517 RVA: 0x0021EA90 File Offset: 0x0021CE90
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x0021EAAC File Offset: 0x0021CEAC
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x04002B89 RID: 11145
		public ThingCategoryDef parent = null;

		// Token: 0x04002B8A RID: 11146
		[NoTranslate]
		public string iconPath = null;

		// Token: 0x04002B8B RID: 11147
		public bool resourceReadoutRoot = false;

		// Token: 0x04002B8C RID: 11148
		[Unsaved]
		public TreeNode_ThingCategory treeNode = null;

		// Token: 0x04002B8D RID: 11149
		[Unsaved]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x04002B8E RID: 11150
		[Unsaved]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x04002B8F RID: 11151
		[Unsaved]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04002B90 RID: 11152
		[Unsaved]
		public Texture2D icon = BaseContent.BadTex;
	}
}
