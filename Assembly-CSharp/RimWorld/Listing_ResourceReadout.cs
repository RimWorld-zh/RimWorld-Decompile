using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Listing_ResourceReadout : Listing_Tree
	{
		private Map map;

		private static Texture2D SolidCategoryBG = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.6f));

		protected override float LabelWidth
		{
			get
			{
				return base.ColumnWidth;
			}
		}

		public Listing_ResourceReadout(Map map)
		{
			this.map = map;
		}

		public void DoCategory(TreeNode_ThingCategory node, int nestLevel, int openMask)
		{
			int countIn = this.map.resourceCounter.GetCountIn(node.catDef);
			if (countIn != 0)
			{
				base.OpenCloseWidget(node, nestLevel, openMask);
				Rect rect = new Rect(0f, base.curY, this.LabelWidth, base.lineHeight);
				rect.xMin = (float)(base.XAtIndentLevel(nestLevel) + 18.0);
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
				float num3 = position2.width = (position2.height = 28f);
				position2.y = (float)(rect.y + rect.height / 2.0 - position2.height / 2.0);
				GUI.DrawTexture(position2, node.catDef.icon);
				Rect rect2 = new Rect(rect);
				rect2.xMin = (float)(position2.xMax + 6.0);
				Widgets.Label(rect2, countIn.ToStringCached());
				base.EndLine();
				if (node.IsOpen(openMask))
				{
					this.DoCategoryChildren(node, nestLevel + 1, openMask);
				}
			}
		}

		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask)
		{
			foreach (TreeNode_ThingCategory childCategoryNode in node.ChildCategoryNodes)
			{
				if (!childCategoryNode.catDef.resourceReadoutRoot)
				{
					this.DoCategory(childCategoryNode, indentLevel, openMask);
				}
			}
			foreach (ThingDef childThingDef in node.catDef.childThingDefs)
			{
				if (!childThingDef.menuHidden)
				{
					this.DoThingDef(childThingDef, indentLevel + 1);
				}
			}
		}

		private void DoThingDef(ThingDef thingDef, int nestLevel)
		{
			int count = this.map.resourceCounter.GetCount(thingDef);
			if (count != 0)
			{
				Rect rect = new Rect(0f, base.curY, this.LabelWidth, base.lineHeight);
				rect.xMin = (float)(base.XAtIndentLevel(nestLevel) + 18.0);
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description, thingDef.shortHash));
				Rect rect2 = new Rect(rect);
				float num3 = rect2.width = (rect2.height = 28f);
				rect2.y = (float)(rect.y + rect.height / 2.0 - rect2.height / 2.0);
				Widgets.ThingIcon(rect2, thingDef);
				Rect rect3 = new Rect(rect);
				rect3.xMin = (float)(rect2.xMax + 6.0);
				Widgets.Label(rect3, count.ToStringCached());
				base.EndLine();
			}
		}
	}
}
