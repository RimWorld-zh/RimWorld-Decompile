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
				if (this.parent != null)
				{
					yield return this.parent;
					foreach (ThingCategoryDef parent2 in this.parent.Parents)
					{
						yield return parent2;
					}
				}
			}
		}

		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				yield return this;
				List<ThingCategoryDef>.Enumerator enumerator = this.childCategories.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingCategoryDef child = enumerator.Current;
						foreach (ThingCategoryDef thisAndChildCategoryDef in child.ThisAndChildCategoryDefs)
						{
							yield return thisAndChildCategoryDef;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
		}

		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				foreach (ThingCategoryDef thisAndChildCategoryDef in this.ThisAndChildCategoryDefs)
				{
					List<ThingDef>.Enumerator enumerator2 = thisAndChildCategoryDef.childThingDefs.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							ThingDef def = enumerator2.Current;
							yield return def;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
		}

		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thisAndChildCategoryDef in this.ThisAndChildCategoryDefs)
				{
					List<SpecialThingFilterDef>.Enumerator enumerator2 = thisAndChildCategoryDef.childSpecialFilters.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							SpecialThingFilterDef sf = enumerator2.Current;
							yield return sf;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
		}

		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef parent2 in this.Parents)
				{
					List<SpecialThingFilterDef>.Enumerator enumerator2 = parent2.childSpecialFilters.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							SpecialThingFilterDef filter = enumerator2.Current;
							yield return filter;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
		}

		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate
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
			return base.defName.GetHashCode();
		}
	}
}
