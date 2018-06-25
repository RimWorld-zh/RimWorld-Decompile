using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDF RID: 3551
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004F86 RID: 20358 RVA: 0x00296B18 File Offset: 0x00294F18
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x00296B48 File Offset: 0x00294F48
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x00296B9C File Offset: 0x00294F9C
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

		// Token: 0x06004F89 RID: 20361 RVA: 0x00296BCC File Offset: 0x00294FCC
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

		// Token: 0x06004F8A RID: 20362 RVA: 0x00296C00 File Offset: 0x00295000
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

		// Token: 0x06004F8B RID: 20363 RVA: 0x00296C3C File Offset: 0x0029503C
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

		// Token: 0x06004F8C RID: 20364 RVA: 0x00296C7C File Offset: 0x0029507C
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x00296C9C File Offset: 0x0029509C
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
