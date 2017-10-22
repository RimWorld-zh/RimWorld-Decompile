using UnityEngine;

namespace Verse
{
	public class Graphic_Random : Graphic_Collection
	{
		public override Material MatSingle
		{
			get
			{
				return base.subGraphics[Rand.Range(0, base.subGraphics.Length)].MatSingle;
			}
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Random>(base.path, newShader, base.drawSize, newColor, Color.white, base.data);
		}

		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing)
		{
			Graphic graphic = (thing == null) ? base.subGraphics[0] : this.SubGraphicFor(thing);
			graphic.DrawWorker(loc, rot, thingDef, thing);
		}

		public Graphic SubGraphicFor(Thing thing)
		{
			return base.subGraphics[thing.thingIDNumber % base.subGraphics.Length];
		}

		public override string ToString()
		{
			return "Random(path=" + base.path + ", count=" + base.subGraphics.Length + ")";
		}
	}
}
