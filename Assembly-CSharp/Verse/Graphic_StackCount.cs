using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE4 RID: 3556
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x002955CC File Offset: 0x002939CC
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x002955F8 File Offset: 0x002939F8
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use this GetColoredVersion with a non-white colorTwo.", 908251, false);
			}
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00295648 File Offset: 0x00293A48
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

		// Token: 0x06004F91 RID: 20369 RVA: 0x00295678 File Offset: 0x00293A78
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

		// Token: 0x06004F92 RID: 20370 RVA: 0x002956AC File Offset: 0x00293AAC
		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x002956D4 File Offset: 0x00293AD4
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

		// Token: 0x06004F94 RID: 20372 RVA: 0x00295710 File Offset: 0x00293B10
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

		// Token: 0x06004F95 RID: 20373 RVA: 0x0029581C File Offset: 0x00293C1C
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
