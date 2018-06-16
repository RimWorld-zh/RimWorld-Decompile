using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE1 RID: 3553
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x06004F77 RID: 20343 RVA: 0x00295325 File Offset: 0x00293725
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004F78 RID: 20344 RVA: 0x0029533C File Offset: 0x0029373C
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x0029535C File Offset: 0x0029375C
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

		// Token: 0x06004F7A RID: 20346 RVA: 0x002953CC File Offset: 0x002937CC
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x002953FC File Offset: 0x002937FC
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data
			};
		}

		// Token: 0x040034BC RID: 13500
		private Graphic subGraphic;

		// Token: 0x040034BD RID: 13501
		private float maxAngle;
	}
}
