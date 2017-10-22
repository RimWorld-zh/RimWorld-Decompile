using UnityEngine;

namespace Verse
{
	public class Graphic_Linked : Graphic
	{
		protected Graphic subGraphic;

		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		public Graphic_Linked()
		{
		}

		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Graphic_Linked graphic_Linked = new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo));
			graphic_Linked.data = base.data;
			return graphic_Linked;
		}

		public override void Print(SectionLayer layer, Thing thing)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, 0f, false, null, null, 0.01f);
		}

		protected Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
		{
			int num = 0;
			int num2 = 1;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (this.ShouldLinkWith(c, parent))
				{
					num += num2;
				}
				num2 *= 2;
			}
			LinkDirections linkSet = (LinkDirections)(byte)num;
			Material mat = this.subGraphic.MatSingleFor(parent);
			return MaterialAtlasPool.SubMaterialFromAtlas(mat, linkSet);
		}

		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (!c.InBounds(parent.Map))
			{
				return ((int)parent.def.graphicData.linkFlags & 1) != 0;
			}
			return (parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) != LinkFlags.None;
		}
	}
}
