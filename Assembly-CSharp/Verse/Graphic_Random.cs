using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDC RID: 3548
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004F82 RID: 20354 RVA: 0x0029670C File Offset: 0x00294B0C
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x0029673C File Offset: 0x00294B3C
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x00296790 File Offset: 0x00294B90
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

		// Token: 0x06004F85 RID: 20357 RVA: 0x002967C0 File Offset: 0x00294BC0
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

		// Token: 0x06004F86 RID: 20358 RVA: 0x002967F4 File Offset: 0x00294BF4
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

		// Token: 0x06004F87 RID: 20359 RVA: 0x00296830 File Offset: 0x00294C30
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

		// Token: 0x06004F88 RID: 20360 RVA: 0x00296870 File Offset: 0x00294C70
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x00296890 File Offset: 0x00294C90
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
