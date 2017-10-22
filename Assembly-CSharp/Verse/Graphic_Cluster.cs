using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Graphic_Cluster : Graphic_Collection
	{
		private const float PositionVariance = 0.45f;

		private const float SizeVariance = 0.2f;

		public override Material MatSingle
		{
			get
			{
				return base.subGraphics[Rand.Range(0, base.subGraphics.Length)].MatSingle;
			}
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243);
		}

		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 a = thing.TrueCenter();
			Rand.PushState();
			Rand.Seed = thing.Position.GetHashCode();
			Filth filth = thing as Filth;
			int num = (filth != null) ? filth.thickness : 3;
			for (int num2 = 0; num2 < num; num2++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = a + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
				Vector2 size = new Vector2(Rand.Range(0.8f, 1.2f), Rand.Range(0.8f, 1.2f));
				float rot = (float)Rand.RangeInclusive(0, 360);
				bool flipUv = Rand.Value < 0.5;
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, null, null, 0.01f);
			}
			Rand.PopState();
		}

		public override string ToString()
		{
			return "Scatter(subGraphic[0]=" + base.subGraphics[0].ToString() + ", count=" + base.subGraphics.Length + ")";
		}
	}
}
