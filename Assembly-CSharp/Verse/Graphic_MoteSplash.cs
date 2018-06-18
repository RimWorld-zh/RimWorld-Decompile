using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDD RID: 3549
	public class Graphic_MoteSplash : Graphic_Mote
	{
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004F5C RID: 20316 RVA: 0x00294BB4 File Offset: 0x00292FB4
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x00294BCC File Offset: 0x00292FCC
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

		// Token: 0x06004F5E RID: 20318 RVA: 0x00294C48 File Offset: 0x00293048
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
