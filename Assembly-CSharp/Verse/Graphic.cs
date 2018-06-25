using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCB RID: 3531
	public class Graphic
	{
		// Token: 0x04003490 RID: 13456
		public GraphicData data;

		// Token: 0x04003491 RID: 13457
		public string path;

		// Token: 0x04003492 RID: 13458
		public Color color = Color.white;

		// Token: 0x04003493 RID: 13459
		public Color colorTwo = Color.white;

		// Token: 0x04003494 RID: 13460
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04003495 RID: 13461
		private Graphic_Shadow cachedShadowGraphicInt = null;

		// Token: 0x04003496 RID: 13462
		private Graphic cachedShadowlessGraphicInt;

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004F14 RID: 20244 RVA: 0x0012CFA4 File Offset: 0x0012B3A4
		public Shader Shader
		{
			get
			{
				Material matSingle = this.MatSingle;
				Shader result;
				if (matSingle != null)
				{
					result = matSingle.shader;
				}
				else
				{
					result = ShaderDatabase.Cutout;
				}
				return result;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06004F15 RID: 20245 RVA: 0x0012CFE0 File Offset: 0x0012B3E0
		public Graphic_Shadow ShadowGraphic
		{
			get
			{
				if (this.cachedShadowGraphicInt == null && this.data != null && this.data.shadowData != null)
				{
					this.cachedShadowGraphicInt = new Graphic_Shadow(this.data.shadowData);
				}
				return this.cachedShadowGraphicInt;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06004F16 RID: 20246 RVA: 0x0012D038 File Offset: 0x0012B438
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x0012D054 File Offset: 0x0012B454
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004F18 RID: 20248 RVA: 0x0012D070 File Offset: 0x0012B470
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06004F19 RID: 20249 RVA: 0x0012D08C File Offset: 0x0012B48C
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06004F1A RID: 20250 RVA: 0x0012D0A8 File Offset: 0x0012B4A8
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06004F1B RID: 20251 RVA: 0x0012D0C4 File Offset: 0x0012B4C4
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06004F1C RID: 20252 RVA: 0x0012D0E0 File Offset: 0x0012B4E0
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06004F1D RID: 20253 RVA: 0x0012D0FC File Offset: 0x0012B4FC
		public virtual bool WestFlipped
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06004F1E RID: 20254 RVA: 0x0012D130 File Offset: 0x0012B530
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x0012D146 File Offset: 0x0012B546
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928, false);
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x0012D16C File Offset: 0x0012B56C
		public virtual Material MatAt(Rot4 rot, Thing thing = null)
		{
			Material result;
			switch (rot.AsInt)
			{
			case 0:
				result = this.MatNorth;
				break;
			case 1:
				result = this.MatEast;
				break;
			case 2:
				result = this.MatSouth;
				break;
			case 3:
				result = this.MatWest;
				break;
			default:
				result = BaseContent.BadMat;
				break;
			}
			return result;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x0012D1DC File Offset: 0x0012B5DC
		public virtual Mesh MeshAt(Rot4 rot)
		{
			Mesh result;
			if (this.ShouldDrawRotated)
			{
				result = MeshPool.GridPlane(this.drawSize);
			}
			else
			{
				Vector2 vector = this.drawSize;
				if (rot.IsHorizontal)
				{
					vector = vector.Rotated();
				}
				if (rot == Rot4.West && this.WestFlipped)
				{
					result = MeshPool.GridPlaneFlip(vector);
				}
				else
				{
					result = MeshPool.GridPlane(vector);
				}
			}
			return result;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0012D258 File Offset: 0x0012B658
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x0012D273 File Offset: 0x0012B673
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x0012D287 File Offset: 0x0012B687
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x0012D298 File Offset: 0x0012B698
		public virtual void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			Quaternion quaternion = this.QuatFromRot(rot);
			if (extraRotation != 0f)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			Material material = this.MatAt(rot, thing);
			Graphics.DrawMesh(mesh, loc, quaternion, material, 0);
			if (this.ShadowGraphic != null)
			{
				this.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x0012D30C File Offset: 0x0012B70C
		public virtual void Print(SectionLayer layer, Thing thing)
		{
			Vector2 size;
			bool flag;
			if (this.ShouldDrawRotated)
			{
				size = this.drawSize;
				flag = false;
			}
			else
			{
				if (!thing.Rotation.IsHorizontal)
				{
					size = this.drawSize;
				}
				else
				{
					size = this.drawSize.Rotated();
				}
				flag = (thing.Rotation == Rot4.West && this.WestFlipped);
			}
			float num = 0f;
			if (this.ShouldDrawRotated)
			{
				num = thing.Rotation.AsAngle;
			}
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), size, this.MatAt(thing.Rotation, thing), num, flag, null, null, 0.01f, 0f);
			if (this.ShadowGraphic != null && thing != null)
			{
				this.ShadowGraphic.Print(layer, thing);
			}
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x0012D408 File Offset: 0x0012B808
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300, false);
			return BaseContent.BadGraphic;
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x0012D444 File Offset: 0x0012B844
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, this.Shader, newDrawSize, this.color, this.colorTwo);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x0012D480 File Offset: 0x0012B880
		public virtual Graphic GetShadowlessGraphic()
		{
			Graphic result;
			if (this.data == null || this.data.shadowData == null)
			{
				result = this;
			}
			else
			{
				if (this.cachedShadowlessGraphicInt == null)
				{
					GraphicData graphicData = new GraphicData();
					graphicData.CopyFrom(this.data);
					graphicData.shadowData = null;
					this.cachedShadowlessGraphicInt = graphicData.Graphic;
				}
				result = this.cachedShadowlessGraphicInt;
			}
			return result;
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x0012D4F0 File Offset: 0x0012B8F0
		protected Quaternion QuatFromRot(Rot4 rot)
		{
			Quaternion result;
			if (this.data != null && !this.data.drawRotated)
			{
				result = Quaternion.identity;
			}
			else if (this.ShouldDrawRotated)
			{
				result = rot.AsQuat;
			}
			else
			{
				result = Quaternion.identity;
			}
			return result;
		}
	}
}
