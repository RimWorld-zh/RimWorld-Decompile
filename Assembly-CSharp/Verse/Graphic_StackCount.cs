using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE3 RID: 3555
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x00296F94 File Offset: 0x00295394
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x00296FC0 File Offset: 0x002953C0
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use this GetColoredVersion with a non-white colorTwo.", 908251, false);
			}
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x00297010 File Offset: 0x00295410
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

		// Token: 0x06004FA8 RID: 20392 RVA: 0x00297040 File Offset: 0x00295440
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

		// Token: 0x06004FA9 RID: 20393 RVA: 0x00297074 File Offset: 0x00295474
		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x0029709C File Offset: 0x0029549C
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

		// Token: 0x06004FAB RID: 20395 RVA: 0x002970D8 File Offset: 0x002954D8
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

		// Token: 0x06004FAC RID: 20396 RVA: 0x002971E4 File Offset: 0x002955E4
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
