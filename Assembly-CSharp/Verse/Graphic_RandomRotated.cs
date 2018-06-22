using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDD RID: 3549
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x06004F8A RID: 20362 RVA: 0x002968E1 File Offset: 0x00294CE1
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004F8B RID: 20363 RVA: 0x002968F8 File Offset: 0x00294CF8
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00296918 File Offset: 0x00294D18
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

		// Token: 0x06004F8D RID: 20365 RVA: 0x00296988 File Offset: 0x00294D88
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x002969B8 File Offset: 0x00294DB8
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data
			};
		}

		// Token: 0x040034C5 RID: 13509
		private Graphic subGraphic;

		// Token: 0x040034C6 RID: 13510
		private float maxAngle;
	}
}
