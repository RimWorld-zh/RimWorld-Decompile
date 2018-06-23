using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD9 RID: 3545
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x040034C2 RID: 13506
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06004F6B RID: 20331 RVA: 0x00295FD8 File Offset: 0x002943D8
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x00295FEE File Offset: 0x002943EE
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00296000 File Offset: 0x00294400
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

		// Token: 0x06004F6E RID: 20334 RVA: 0x0029611C File Offset: 0x0029451C
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
	}
}
