using UnityEngine;

namespace Verse
{
	public class Graphic_StackCount : Graphic_Collection
	{
		public override Material MatSingle
		{
			get
			{
				return base.subGraphics[base.subGraphics.Length - 1].MatSingle;
			}
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use this GetColoredVersion with a non-white colorTwo.", 908251);
			}
			return GraphicDatabase.Get<Graphic_StackCount>(base.path, newShader, base.drawSize, newColor, newColorTwo, base.data);
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

		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing)
		{
			Graphic graphic = (thing == null) ? base.subGraphics[0] : this.SubGraphicFor(thing);
			graphic.DrawWorker(loc, rot, thingDef, thing);
		}

		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			switch (base.subGraphics.Length)
			{
			case 1:
			{
				return base.subGraphics[0];
			}
			case 2:
			{
				if (stackCount == 1)
				{
					return base.subGraphics[0];
				}
				return base.subGraphics[1];
			}
			case 3:
			{
				if (stackCount == 1)
				{
					return base.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return base.subGraphics[2];
				}
				return base.subGraphics[1];
			}
			default:
			{
				if (stackCount == 1)
				{
					return base.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return base.subGraphics[base.subGraphics.Length - 1];
				}
				int num = 1 + Mathf.RoundToInt(Mathf.InverseLerp(2f, (float)(def.stackLimit - 1), (float)(base.subGraphics.Length - 2)));
				return base.subGraphics[num];
			}
			}
		}

		public override string ToString()
		{
			return "StackCount(path=" + base.path + ", count=" + base.subGraphics.Length + ")";
		}
	}
}
