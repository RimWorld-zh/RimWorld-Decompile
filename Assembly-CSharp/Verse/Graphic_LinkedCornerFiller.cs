using UnityEngine;

namespace Verse
{
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		private const float ShiftUp = 0.09f;

		private const float CoverSize = 0.5f;

		private static readonly float CoverSizeCornerCorner = new Vector2(0.5f, 0.5f).magnitude;

		private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

		private static readonly float CoverOffsetDist = (float)(Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5);

		private static readonly Vector2[] CornerFillUVs = new Vector2[4]
		{
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f)
		};

		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Graphic_LinkedCornerFiller graphic_LinkedCornerFiller = new Graphic_LinkedCornerFiller(base.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo));
			graphic_LinkedCornerFiller.data = base.data;
			return graphic_LinkedCornerFiller;
		}

		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			IntVec3 position = thing.Position;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = thing.Position + GenAdj.DiagonalDirectionsAround[i];
				if (this.ShouldLinkWith(c, thing) && (i != 0 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))) && (i != 1 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 2 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 3 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))))
				{
					Vector3 vector = thing.DrawPos + GenAdj.DiagonalDirectionsAround[i].ToVector3().normalized * Graphic_LinkedCornerFiller.CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.09f);
					Vector3 center = vector;
					Vector2 size = new Vector2(0.5f, 0.5f);
					Material mat = base.LinkedDrawMatFrom(thing, thing.Position);
					float rot = 0f;
					Vector2[] cornerFillUVs = Graphic_LinkedCornerFiller.CornerFillUVs;
					Printer_Plane.PrintPlane(layer, center, size, mat, rot, false, cornerFillUVs, null, 0.01f);
				}
			}
		}
	}
}
