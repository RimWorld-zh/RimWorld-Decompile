using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCC RID: 3532
	public class Graphic
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004EFB RID: 20219 RVA: 0x0012CD0C File Offset: 0x0012B10C
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

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004EFC RID: 20220 RVA: 0x0012CD48 File Offset: 0x0012B148
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

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06004EFD RID: 20221 RVA: 0x0012CDA0 File Offset: 0x0012B1A0
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06004EFE RID: 20222 RVA: 0x0012CDBC File Offset: 0x0012B1BC
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004EFF RID: 20223 RVA: 0x0012CDD8 File Offset: 0x0012B1D8
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004F00 RID: 20224 RVA: 0x0012CDF4 File Offset: 0x0012B1F4
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06004F01 RID: 20225 RVA: 0x0012CE10 File Offset: 0x0012B210
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06004F02 RID: 20226 RVA: 0x0012CE2C File Offset: 0x0012B22C
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06004F03 RID: 20227 RVA: 0x0012CE48 File Offset: 0x0012B248
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06004F04 RID: 20228 RVA: 0x0012CE64 File Offset: 0x0012B264
		public virtual bool WestFlipped
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06004F05 RID: 20229 RVA: 0x0012CE98 File Offset: 0x0012B298
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x0012CEAE File Offset: 0x0012B2AE
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928, false);
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x0012CED4 File Offset: 0x0012B2D4
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

		// Token: 0x06004F08 RID: 20232 RVA: 0x0012CF44 File Offset: 0x0012B344
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

		// Token: 0x06004F09 RID: 20233 RVA: 0x0012CFC0 File Offset: 0x0012B3C0
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x0012CFDB File Offset: 0x0012B3DB
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x0012CFEF File Offset: 0x0012B3EF
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x0012D000 File Offset: 0x0012B400
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

		// Token: 0x06004F0D RID: 20237 RVA: 0x0012D074 File Offset: 0x0012B474
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

		// Token: 0x06004F0E RID: 20238 RVA: 0x0012D170 File Offset: 0x0012B570
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300, false);
			return BaseContent.BadGraphic;
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x0012D1AC File Offset: 0x0012B5AC
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, this.Shader, newDrawSize, this.color, this.colorTwo);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x0012D1E8 File Offset: 0x0012B5E8
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

		// Token: 0x06004F11 RID: 20241 RVA: 0x0012D258 File Offset: 0x0012B658
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

		// Token: 0x04003485 RID: 13445
		public GraphicData data;

		// Token: 0x04003486 RID: 13446
		public string path;

		// Token: 0x04003487 RID: 13447
		public Color color = Color.white;

		// Token: 0x04003488 RID: 13448
		public Color colorTwo = Color.white;

		// Token: 0x04003489 RID: 13449
		public Vector2 drawSize = Vector2.one;

		// Token: 0x0400348A RID: 13450
		private Graphic_Shadow cachedShadowGraphicInt = null;

		// Token: 0x0400348B RID: 13451
		private Graphic cachedShadowlessGraphicInt;
	}
}
