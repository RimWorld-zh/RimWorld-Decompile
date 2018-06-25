using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD5 RID: 3541
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x040034B1 RID: 13489
		private const float PositionVariance = 0.45f;

		// Token: 0x040034B2 RID: 13490
		private const float SizeVariance = 0.2f;

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06004F52 RID: 20306 RVA: 0x00295538 File Offset: 0x00293938
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x00295567 File Offset: 0x00293967
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243, false);
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x0029557C File Offset: 0x0029397C
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 a = thing.TrueCenter();
			Rand.PushState();
			Rand.Seed = thing.Position.GetHashCode();
			Filth filth = thing as Filth;
			int num;
			if (filth == null)
			{
				num = 3;
			}
			else
			{
				num = filth.thickness;
			}
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = a + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
				Vector2 size = new Vector2(Rand.Range(0.8f, 1.2f), Rand.Range(0.8f, 1.2f));
				float rot = (float)Rand.RangeInclusive(0, 360);
				bool flipUv = Rand.Value < 0.5f;
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, null, null, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x0029567C File Offset: 0x00293A7C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Scatter(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
