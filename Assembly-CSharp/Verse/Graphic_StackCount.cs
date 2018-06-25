using System;
using UnityEngine;

namespace Verse
{
	public class Graphic_StackCount : Graphic_Collection
	{
		public Graphic_StackCount()
		{
		}

		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use this GetColoredVersion with a non-white colorTwo.", 908251, false);
			}
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
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

		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
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

		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			Graphic result;
			switch (this.subGraphics.Length)
			{
			case 1:
				result = this.subGraphics[0];
				break;
			case 2:
				if (stackCount == 1)
				{
					result = this.subGraphics[0];
				}
				else
				{
					result = this.subGraphics[1];
				}
				break;
			case 3:
				if (stackCount == 1)
				{
					result = this.subGraphics[0];
				}
				else if (stackCount == def.stackLimit)
				{
					result = this.subGraphics[2];
				}
				else
				{
					result = this.subGraphics[1];
				}
				break;
			default:
				if (stackCount == 1)
				{
					result = this.subGraphics[0];
				}
				else if (stackCount == def.stackLimit)
				{
					result = this.subGraphics[this.subGraphics.Length - 1];
				}
				else
				{
					int num = 1 + Mathf.RoundToInt(Mathf.InverseLerp(2f, (float)(def.stackLimit - 1), (float)(this.subGraphics.Length - 2)));
					result = this.subGraphics[num];
				}
				break;
			}
			return result;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"StackCount(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
