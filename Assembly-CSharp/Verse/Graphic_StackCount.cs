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
			return (thing != null) ? this.MatSingleFor(thing) : this.MatSingle;
		}

		public override Material MatSingleFor(Thing thing)
		{
			return (thing != null) ? this.SubGraphicFor(thing).MatSingle : this.MatSingle;
		}

		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic = (thing == null) ? base.subGraphics[0] : this.SubGraphicFor(thing);
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			Graphic result;
			switch (base.subGraphics.Length)
			{
			case 1:
			{
				result = base.subGraphics[0];
				break;
			}
			case 2:
			{
				result = ((stackCount != 1) ? base.subGraphics[1] : base.subGraphics[0]);
				break;
			}
			case 3:
			{
				result = ((stackCount != 1) ? ((stackCount != def.stackLimit) ? base.subGraphics[1] : base.subGraphics[2]) : base.subGraphics[0]);
				break;
			}
			default:
			{
				if (stackCount == 1)
				{
					result = base.subGraphics[0];
				}
				else if (stackCount == def.stackLimit)
				{
					result = base.subGraphics[base.subGraphics.Length - 1];
				}
				else
				{
					int num = 1 + Mathf.RoundToInt(Mathf.InverseLerp(2f, (float)(def.stackLimit - 1), (float)(base.subGraphics.Length - 2)));
					result = base.subGraphics[num];
				}
				break;
			}
			}
			return result;
		}

		public override string ToString()
		{
			return "StackCount(path=" + base.path + ", count=" + base.subGraphics.Length + ")";
		}
	}
}
