using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Listing_ResourceReadout : Listing_Tree
	{
		private Map map;

		private static Texture2D SolidCategoryBG = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.6f));

		public Listing_ResourceReadout(Map map)
		{
			this.map = map;
		}

		protected override float LabelWidth
		{
			get
			{
				return base.ColumnWidth;
			}
		}

		public void DoCategory(TreeNode_ThingCategory node, int nestLevel, int openMask)
		{
			int countIn = this.map.resourceCounter.GetCountIn(node.catDef);
			if (countIn == 0)
			{
				return;
			}
			base.OpenCloseWidget(node, nestLevel, openMask);
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			Rect position = rect;
			position.width = 80f;
			position.yMax -= 3f;
			position.yMin += 3f;
			GUI.DrawTexture(position, Listing_ResourceReadout.SolidCategoryBG);
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			TooltipHandler.TipRegion(rect, new TipSignal(node.catDef.LabelCap, node.catDef.GetHashCode()));
			Rect position2 = new Rect(rect);
			float num = 28f;
			position2.height = num;
			position2.width = num;
			position2.y = rect.y + rect.height / 2f - position2.height / 2f;
			GUI.DrawTexture(position2, node.catDef.icon);
			Widgets.Label(new Rect(rect)
			{
				xMin = position2.xMax + 6f
			}, countIn.ToStringCached());
			base.EndLine();
			if (node.IsOpen(openMask))
			{
				this.DoCategoryChildren(node, nestLevel + 1, openMask);
			}
		}

		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask)
		{
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (!treeNode_ThingCategory.catDef.resourceReadoutRoot)
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask);
				}
			}
			foreach (ThingDef thingDef in node.catDef.childThingDefs)
			{
				if (!thingDef.menuHidden)
				{
					this.DoThingDef(thingDef, indentLevel + 1);
				}
			}
		}

		private void DoThingDef(ThingDef thingDef, int nestLevel)
		{
			int count = this.map.resourceCounter.GetCount(thingDef);
			if (count == 0)
			{
				return;
			}
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst(), (int)thingDef.shortHash));
			Rect rect2 = new Rect(rect);
			float num = 28f;
			rect2.height = num;
			rect2.width = num;
			rect2.y = rect.y + rect.height / 2f - rect2.height / 2f;
			Widgets.ThingIcon(rect2, thingDef);
			Widgets.Label(new Rect(rect)
			{
				xMin = rect2.xMax + 6f
			}, count.ToStringCached());
			base.EndLine();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Listing_ResourceReadout()
		{
		}

		[CompilerGenerated]
		private sealed class <DoThingDef>c__AnonStorey0
		{
			internal ThingDef thingDef;

			public <DoThingDef>c__AnonStorey0()
			{
			}

			internal string <>m__0()
			{
				return this.thingDef.LabelCap + ": " + this.thingDef.description.CapitalizeFirst();
			}
		}
	}
}
