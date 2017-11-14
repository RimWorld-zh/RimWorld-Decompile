using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class MoteBubble : MoteDualAttached
	{
		public Material iconMat;

		public Pawn arrowTarget;

		private static readonly Material InteractionArrowTex = MaterialPool.MatFrom("Things/Mote/InteractionArrow");

		public void SetupMoteBubble(Texture2D icon, Pawn target)
		{
			this.iconMat = MaterialPool.MatFrom(icon, ShaderDatabase.TransparentPostLight, Color.white);
			this.arrowTarget = target;
		}

		public override void Draw()
		{
			base.Draw();
			if ((Object)this.iconMat != (Object)null)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.01f;
				float num = Graphic_Mote.CalculateMoteAlpha(this);
				if (!(num <= 0.0))
				{
					Color instanceColor = base.instanceColor;
					instanceColor.a *= num;
					Material material = this.iconMat;
					if (instanceColor != material.color)
					{
						material = MaterialPool.MatFrom((Texture2D)material.mainTexture, material.shader, instanceColor);
					}
					Vector3 s = new Vector3((float)(base.def.graphicData.drawSize.x * 0.63999998569488525), 1f, (float)(base.def.graphicData.drawSize.y * 0.63999998569488525));
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(drawPos, Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
					goto IL_00f1;
				}
				return;
			}
			goto IL_00f1;
			IL_00f1:
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
