using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDF RID: 3551
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004F6D RID: 20333 RVA: 0x00295130 File Offset: 0x00293530
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x00295160 File Offset: 0x00293560
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x002951B4 File Offset: 0x002935B4
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

		// Token: 0x06004F70 RID: 20336 RVA: 0x002951E4 File Offset: 0x002935E4
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

		// Token: 0x06004F71 RID: 20337 RVA: 0x00295218 File Offset: 0x00293618
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

		// Token: 0x06004F72 RID: 20338 RVA: 0x00295254 File Offset: 0x00293654
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

		// Token: 0x06004F73 RID: 20339 RVA: 0x00295294 File Offset: 0x00293694
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x002952B4 File Offset: 0x002936B4
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
