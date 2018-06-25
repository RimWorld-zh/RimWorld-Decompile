using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDB RID: 3547
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x040034C2 RID: 13506
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004F6F RID: 20335 RVA: 0x00296104 File Offset: 0x00294504
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x0029611A File Offset: 0x0029451A
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x0029612C File Offset: 0x0029452C
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

		// Token: 0x06004F72 RID: 20338 RVA: 0x00296248 File Offset: 0x00294648
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
