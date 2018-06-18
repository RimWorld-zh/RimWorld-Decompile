using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD9 RID: 3545
	public class Graphic_Gas : Graphic_Single
	{
		// Token: 0x06004F46 RID: 20294 RVA: 0x002944C4 File Offset: 0x002928C4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Rand.PushState();
			Rand.Seed = thing.thingIDNumber.GetHashCode();
			Gas gas = thing as Gas;
			float angle = (float)Rand.Range(0, 360) + ((gas != null) ? gas.graphicRotation : 0f);
			Vector3 pos = thing.TrueCenter() + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
			Vector3 s = new Vector3(Rand.Range(0.8f, 1.2f) * this.drawSize.x, 0f, Rand.Range(0.8f, 1.2f) * this.drawSize.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.MatSingle, 0);
			Rand.PopState();
		}

		// Token: 0x040034AE RID: 13486
		private const float PositionVariance = 0.45f;

		// Token: 0x040034AF RID: 13487
		private const float SizeVariance = 0.2f;
	}
}
