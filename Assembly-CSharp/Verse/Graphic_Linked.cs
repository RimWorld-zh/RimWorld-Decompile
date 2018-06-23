using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD7 RID: 3543
	public class Graphic_Linked : Graphic
	{
		// Token: 0x040034BB RID: 13499
		protected Graphic subGraphic = null;

		// Token: 0x06004F5C RID: 20316 RVA: 0x0012D3F8 File Offset: 0x0012B7F8
		public Graphic_Linked()
		{
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x0012D408 File Offset: 0x0012B808
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06004F5E RID: 20318 RVA: 0x0012D420 File Offset: 0x0012B820
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06004F5F RID: 20319 RVA: 0x0012D438 File Offset: 0x0012B838
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x0012D460 File Offset: 0x0012B860
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x0012D498 File Offset: 0x0012B898
		public override void Print(SectionLayer layer, Thing thing)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x0012D4E4 File Offset: 0x0012B8E4
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x0012D508 File Offset: 0x0012B908
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
			LinkDirections linkSet = (LinkDirections)num;
			Material mat = this.subGraphic.MatSingleFor(parent);
			return MaterialAtlasPool.SubMaterialFromAtlas(mat, linkSet);
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x0012D580 File Offset: 0x0012B980
		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			bool result;
			if (!parent.Spawned)
			{
				result = false;
			}
			else if (!c.InBounds(parent.Map))
			{
				result = ((parent.def.graphicData.linkFlags & LinkFlags.MapEdge) != LinkFlags.None);
			}
			else
			{
				result = ((parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) != LinkFlags.None);
			}
			return result;
		}
	}
}
