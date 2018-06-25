using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD8 RID: 3544
	public class Graphic_Flicker : Graphic_Collection
	{
		// Token: 0x040034BD RID: 13501
		private const int BaseTicksPerFrameChange = 15;

		// Token: 0x040034BE RID: 13502
		private const int ExtraTicksPerFrameChange = 10;

		// Token: 0x040034BF RID: 13503
		private const float MaxOffset = 0.05f;

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06004F5B RID: 20315 RVA: 0x002959BC File Offset: 0x00293DBC
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x002959EC File Offset: 0x00293DEC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (thingDef == null)
			{
				Log.ErrorOnce("Fire DrawWorker with null thingDef: " + loc, 3427324, false);
			}
			else if (this.subGraphics == null)
			{
				Log.ErrorOnce("Graphic_Flicker has no subgraphics " + thingDef, 358773632, false);
			}
			else
			{
				int num = Find.TickManager.TicksGame;
				if (thing != null)
				{
					num += Mathf.Abs(thing.thingIDNumber ^ 8453458);
				}
				int num2 = num / 15;
				int num3 = Mathf.Abs(num2 ^ ((thing == null) ? 0 : thing.thingIDNumber) * 391) % this.subGraphics.Length;
				float num4 = 1f;
				CompProperties_FireOverlay compProperties_FireOverlay = null;
				Fire fire = thing as Fire;
				if (fire != null)
				{
					num4 = fire.fireSize;
				}
				else if (thingDef != null)
				{
					compProperties_FireOverlay = thingDef.GetCompProperties<CompProperties_FireOverlay>();
					if (compProperties_FireOverlay != null)
					{
						num4 = compProperties_FireOverlay.fireSize;
					}
				}
				if (num3 < 0 || num3 >= this.subGraphics.Length)
				{
					Log.ErrorOnce("Fire drawing out of range: " + num3, 7453435, false);
					num3 = 0;
				}
				Graphic graphic = this.subGraphics[num3];
				float num5 = Mathf.Min(num4 / 1.2f, 1.2f);
				Vector3 a = GenRadial.RadialPattern[num2 % GenRadial.RadialPattern.Length].ToVector3() / GenRadial.MaxRadialPatternRadius;
				a *= 0.05f;
				Vector3 vector = loc + a * num4;
				if (compProperties_FireOverlay != null)
				{
					vector += compProperties_FireOverlay.offset;
				}
				Vector3 s = new Vector3(num5, 1f, num5);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, graphic.MatSingle, 0);
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x00295BCC File Offset: 0x00293FCC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Flicker(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
