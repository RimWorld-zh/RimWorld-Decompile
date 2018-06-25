using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CF RID: 1743
	[StaticConstructorOnStartup]
	public class MoteBubble : MoteDualAttached
	{
		// Token: 0x04001519 RID: 5401
		public Material iconMat;

		// Token: 0x0400151A RID: 5402
		public Pawn arrowTarget;

		// Token: 0x0400151B RID: 5403
		private static readonly Material InteractionArrowTex = MaterialPool.MatFrom("Things/Mote/InteractionArrow");

		// Token: 0x060025B8 RID: 9656 RVA: 0x001438B1 File Offset: 0x00141CB1
		public void SetupMoteBubble(Texture2D icon, Pawn target)
		{
			this.iconMat = MaterialPool.MatFrom(icon, ShaderDatabase.TransparentPostLight, Color.white);
			this.arrowTarget = target;
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x001438D4 File Offset: 0x00141CD4
		public override void Draw()
		{
			base.Draw();
			if (this.iconMat != null)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.01f;
				float alpha = this.Alpha;
				if (alpha <= 0f)
				{
					return;
				}
				Color instanceColor = this.instanceColor;
				instanceColor.a *= alpha;
				Material material = this.iconMat;
				if (instanceColor != material.color)
				{
					material = MaterialPool.MatFrom((Texture2D)material.mainTexture, material.shader, instanceColor);
				}
				Vector3 s = new Vector3(this.def.graphicData.drawSize.x * 0.64f, 1f, this.def.graphicData.drawSize.y * 0.64f);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(drawPos, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
			}
			if (this.arrowTarget != null)
			{
				Vector3 a = this.arrowTarget.TrueCenter();
				float angle = (a - this.DrawPos).AngleFlat();
				Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
				Vector3 vector = this.DrawPos;
				vector.y -= 0.01f;
				vector += 0.6f * (rotation * Vector3.forward);
				Graphics.DrawMesh(MeshPool.plane05, vector, rotation, MoteBubble.InteractionArrowTex, 0);
			}
		}
	}
}
