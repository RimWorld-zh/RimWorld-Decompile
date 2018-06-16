using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDB RID: 3547
	public class Graphic_Linked : Graphic
	{
		// Token: 0x06004F49 RID: 20297 RVA: 0x0012D238 File Offset: 0x0012B638
		public Graphic_Linked()
		{
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x0012D248 File Offset: 0x0012B648
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06004F4B RID: 20299 RVA: 0x0012D260 File Offset: 0x0012B660
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06004F4C RID: 20300 RVA: 0x0012D278 File Offset: 0x0012B678
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x0012D2A0 File Offset: 0x0012B6A0
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x0012D2D8 File Offset: 0x0012B6D8
		public override void Print(SectionLayer layer, Thing thing)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x0012D324 File Offset: 0x0012B724
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x0012D348 File Offset: 0x0012B748
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

		// Token: 0x06004F51 RID: 20305 RVA: 0x0012D3C0 File Offset: 0x0012B7C0
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

		// Token: 0x040034B2 RID: 13490
		protected Graphic subGraphic = null;
	}
}
