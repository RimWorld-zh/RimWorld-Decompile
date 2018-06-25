using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE0 RID: 3552
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x040034CC RID: 13516
		private Graphic subGraphic;

		// Token: 0x040034CD RID: 13517
		private float maxAngle;

		// Token: 0x06004F8E RID: 20366 RVA: 0x00296CED File Offset: 0x002950ED
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004F8F RID: 20367 RVA: 0x00296D04 File Offset: 0x00295104
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00296D24 File Offset: 0x00295124
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Material matSingle = this.subGraphic.MatSingle;
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), matSingle, 0, null, 0);
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x00296D94 File Offset: 0x00295194
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00296DC4 File Offset: 0x002951C4
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data
			};
		}
	}
}
