using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDD RID: 3549
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004F58 RID: 20312 RVA: 0x00294A1C File Offset: 0x00292E1C
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x00294A32 File Offset: 0x00292E32
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x00294A44 File Offset: 0x00292E44
		public void DrawMoteInternal(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer)
		{
			Mote mote = (Mote)thing;
			float alpha = mote.Alpha;
			if (alpha > 0f)
			{
				Color color = base.Color * mote.instanceColor;
				color.a *= alpha;
				Vector3 exactScale = mote.exactScale;
				exactScale.x *= this.data.drawSize.x;
				exactScale.z *= this.data.drawSize.y;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
				Material matSingle = this.MatSingle;
				if (!this.ForcePropertyBlock && color.IndistinguishableFrom(matSingle.color))
				{
					Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, layer, null, 0);
				}
				else
				{
					Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color);
					Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, layer, null, 0, Graphic_Mote.propertyBlock);
				}
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00294B60 File Offset: 0x00292F60
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Mote(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x040034B9 RID: 13497
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
