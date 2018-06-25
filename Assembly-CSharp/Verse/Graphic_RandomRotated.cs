using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDF RID: 3551
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x040034C5 RID: 13509
		private Graphic subGraphic;

		// Token: 0x040034C6 RID: 13510
		private float maxAngle;

		// Token: 0x06004F8E RID: 20366 RVA: 0x00296A0D File Offset: 0x00294E0D
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004F8F RID: 20367 RVA: 0x00296A24 File Offset: 0x00294E24
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00296A44 File Offset: 0x00294E44
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

		// Token: 0x06004F91 RID: 20369 RVA: 0x00296AB4 File Offset: 0x00294EB4
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00296AE4 File Offset: 0x00294EE4
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data
			};
		}
	}
}
