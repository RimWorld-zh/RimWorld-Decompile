using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Graphic_Flicker : Graphic_Collection
	{
		private const int BaseTicksPerFrameChange = 15;

		private const int ExtraTicksPerFrameChange = 10;

		private const float MaxOffset = 0.05f;

		public override Material MatSingle
		{
			get
			{
				return base.subGraphics[Rand.Range(0, base.subGraphics.Length)].MatSingle;
			}
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (thingDef == null)
			{
				Log.ErrorOnce("Fire DrawWorker with null thingDef: " + loc, 3427324);
			}
			else if (base.subGraphics == null)
			{
				Log.ErrorOnce("Graphic_Flicker has no subgraphics " + thingDef, 358773632);
			}
			else
			{
				int ticksGame = Find.TickManager.TicksGame;
				int num = 0;
				int num2 = 0;
				Fire fire = null;
				float num3 = 1f;
				CompFireOverlay compFireOverlay = null;
				if (thing != null)
				{
					compFireOverlay = thing.TryGetComp<CompFireOverlay>();
					ticksGame += Mathf.Abs(thing.thingIDNumber ^ 8453458);
					num = ticksGame / 15;
					num2 = Mathf.Abs(num ^ thing.thingIDNumber * 391) % base.subGraphics.Length;
					fire = (thing as Fire);
					if (fire != null)
					{
						num3 = fire.fireSize;
					}
					else if (compFireOverlay != null)
					{
						num3 = compFireOverlay.Props.fireSize;
					}
				}
				if (num2 < 0 || num2 >= base.subGraphics.Length)
				{
					Log.ErrorOnce("Fire drawing out of range: " + num2, 7453435);
					num2 = 0;
				}
				Graphic graphic = base.subGraphics[num2];
				float num4 = Mathf.Min((float)(num3 / 1.2000000476837158), 1.2f);
				Vector3 a = GenRadial.RadialPattern[num % GenRadial.RadialPattern.Length].ToVector3() / GenRadial.MaxRadialPatternRadius;
				a *= 0.05f;
				Vector3 vector = loc + a * num3;
				if (compFireOverlay != null)
				{
					vector += compFireOverlay.Props.offset;
				}
				Vector3 s = new Vector3(num4, 1f, num4);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, graphic.MatSingle, 0);
			}
		}

		public override string ToString()
		{
			return "Flicker(subGraphic[0]=" + base.subGraphics[0].ToString() + ", count=" + base.subGraphics.Length + ")";
		}
	}
}
