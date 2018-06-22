using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDA RID: 3546
	public class Graphic_MoteSplash : Graphic_Mote
	{
		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x00296190 File Offset: 0x00294590
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x002961A8 File Offset: 0x002945A8
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

		// Token: 0x06004F73 RID: 20339 RVA: 0x00296224 File Offset: 0x00294624
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
