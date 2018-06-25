using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDD RID: 3549
	public class Graphic_MoteSplash : Graphic_Mote
	{
		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x0029659C File Offset: 0x0029499C
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x002965B4 File Offset: 0x002949B4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteSplash moteSplash = (MoteSplash)thing;
			float alpha = moteSplash.Alpha;
			if (alpha > 0f)
			{
				Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, alpha));
				Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, moteSplash.CalculatedShockwaveSpan());
				base.DrawMoteInternal(loc, rot, thingDef, thing, SubcameraDefOf.WaterDepth.LayerId);
			}
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x00296630 File Offset: 0x00294A30
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"MoteSplash(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
