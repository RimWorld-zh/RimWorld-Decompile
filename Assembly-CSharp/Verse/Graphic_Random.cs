using System;
using UnityEngine;

namespace Verse
{
	public class Graphic_Random : Graphic_Collection
	{
		public Graphic_Random()
		{
		}

		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			Material result;
			if (thing == null)
			{
				result = this.MatSingle;
			}
			else
			{
				result = this.MatSingleFor(thing);
			}
			return result;
		}

		public override Material MatSingleFor(Thing thing)
		{
			Material matSingle;
			if (thing == null)
			{
				matSingle = this.MatSingle;
			}
			else
			{
				matSingle = this.SubGraphicFor(thing).MatSingle;
			}
			return matSingle;
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		public Graphic SubGraphicFor(Thing thing)
		{
			Graphic result;
			if (thing == null)
			{
				result = this.subGraphics[0];
			}
			else
			{
				result = this.subGraphics[thing.thingIDNumber % this.subGraphics.Length];
			}
			return result;
		}

		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Random(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
