using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD8 RID: 3544
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		// Token: 0x06004F65 RID: 20325 RVA: 0x00295BA1 File Offset: 0x00293FA1
		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004F66 RID: 20326 RVA: 0x00295BAC File Offset: 0x00293FAC
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x00295BC4 File Offset: 0x00293FC4
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedCornerFiller(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x00295BFC File Offset: 0x00293FFC
		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			IntVec3 position = thing.Position;
			int i = 0;
			while (i < 4)
			{
				IntVec3 c = thing.Position + GenAdj.DiagonalDirectionsAround[i];
				if (this.ShouldLinkWith(c, thing))
				{
					if (i == 0)
					{
						if (!this.ShouldLinkWith(position + IntVec3.West, thing) || !this.ShouldLinkWith(position + IntVec3.South, thing))
						{
							goto IL_2E7;
						}
					}
					if (i == 1)
					{
						if (!this.ShouldLinkWith(position + IntVec3.West, thing) || !this.ShouldLinkWith(position + IntVec3.North, thing))
						{
							goto IL_2E7;
						}
					}
					if (i == 2)
					{
						if (!this.ShouldLinkWith(position + IntVec3.East, thing) || !this.ShouldLinkWith(position + IntVec3.North, thing))
						{
							goto IL_2E7;
						}
					}
					if (i == 3)
					{
						if (!this.ShouldLinkWith(position + IntVec3.East, thing) || !this.ShouldLinkWith(position + IntVec3.South, thing))
						{
							goto IL_2E7;
						}
					}
					Vector3 vector = thing.DrawPos + GenAdj.DiagonalDirectionsAround[i].ToVector3().normalized * Graphic_LinkedCornerFiller.CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.09f);
					Vector2 vector2 = new Vector2(0.5f, 0.5f);
					if (!c.InBounds(thing.Map))
					{
						if (c.x == -1)
						{
							vector.x -= 1f;
							vector2.x *= 5f;
						}
						if (c.z == -1)
						{
							vector.z -= 1f;
							vector2.y *= 5f;
						}
						if (c.x == thing.Map.Size.x)
						{
							vector.x += 1f;
							vector2.x *= 5f;
						}
						if (c.z == thing.Map.Size.z)
						{
							vector.z += 1f;
							vector2.y *= 5f;
						}
					}
					Vector3 center = vector;
					Vector2 size = vector2;
					Material mat = base.LinkedDrawMatFrom(thing, thing.Position);
					float rot = 0f;
					Vector2[] cornerFillUVs = Graphic_LinkedCornerFiller.CornerFillUVs;
					Printer_Plane.PrintPlane(layer, center, size, mat, rot, false, cornerFillUVs, null, 0.01f, 0f);
				}
				IL_2E7:
				i++;
				continue;
				goto IL_2E7;
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00295EFC File Offset: 0x002942FC
		// Note: this type is marked as 'beforefieldinit'.
		static Graphic_LinkedCornerFiller()
		{
			Vector2 vector = new Vector2(0.5f, 0.5f);
			Graphic_LinkedCornerFiller.CoverSizeCornerCorner = vector.magnitude;
			Vector2 vector2 = new Vector2(0.5f, 0.5f);
			Graphic_LinkedCornerFiller.DistCenterCorner = vector2.magnitude;
			Graphic_LinkedCornerFiller.CoverOffsetDist = Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5f;
			Graphic_LinkedCornerFiller.CornerFillUVs = new Vector2[]
			{
				new Vector2(0.5f, 0.6f),
				new Vector2(0.5f, 0.6f),
				new Vector2(0.5f, 0.6f),
				new Vector2(0.5f, 0.6f)
			};
		}

		// Token: 0x040034BC RID: 13500
		private const float ShiftUp = 0.09f;

		// Token: 0x040034BD RID: 13501
		private const float CoverSize = 0.5f;

		// Token: 0x040034BE RID: 13502
		private static readonly float CoverSizeCornerCorner;

		// Token: 0x040034BF RID: 13503
		private static readonly float DistCenterCorner;

		// Token: 0x040034C0 RID: 13504
		private static readonly float CoverOffsetDist;

		// Token: 0x040034C1 RID: 13505
		private static readonly Vector2[] CornerFillUVs;
	}
}
