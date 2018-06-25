using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B1B RID: 2843
	public class GraphicData
	{
		// Token: 0x0400283C RID: 10300
		[NoTranslate]
		public string texPath = null;

		// Token: 0x0400283D RID: 10301
		public Type graphicClass = null;

		// Token: 0x0400283E RID: 10302
		public ShaderTypeDef shaderType = null;

		// Token: 0x0400283F RID: 10303
		public List<ShaderParameter> shaderParameters = null;

		// Token: 0x04002840 RID: 10304
		public Color color = Color.white;

		// Token: 0x04002841 RID: 10305
		public Color colorTwo = Color.white;

		// Token: 0x04002842 RID: 10306
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04002843 RID: 10307
		public float onGroundRandomRotateAngle = 0f;

		// Token: 0x04002844 RID: 10308
		public bool drawRotated = true;

		// Token: 0x04002845 RID: 10309
		public bool allowFlip = true;

		// Token: 0x04002846 RID: 10310
		public float flipExtraRotation = 0f;

		// Token: 0x04002847 RID: 10311
		public ShadowData shadowData = null;

		// Token: 0x04002848 RID: 10312
		public DamageGraphicData damageData = null;

		// Token: 0x04002849 RID: 10313
		public LinkDrawerType linkType = LinkDrawerType.None;

		// Token: 0x0400284A RID: 10314
		public LinkFlags linkFlags = LinkFlags.None;

		// Token: 0x0400284B RID: 10315
		[Unsaved]
		private Graphic cachedGraphic = null;

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x00210B74 File Offset: 0x0020EF74
		public bool Linked
		{
			get
			{
				return this.linkType != LinkDrawerType.None;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003EBA RID: 16058 RVA: 0x00210B98 File Offset: 0x0020EF98
		public Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.Init();
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00210BC4 File Offset: 0x0020EFC4
		public void CopyFrom(GraphicData other)
		{
			this.texPath = other.texPath;
			this.graphicClass = other.graphicClass;
			this.shaderType = other.shaderType;
			this.color = other.color;
			this.colorTwo = other.colorTwo;
			this.drawSize = other.drawSize;
			this.onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
			this.drawRotated = other.drawRotated;
			this.allowFlip = other.allowFlip;
			this.flipExtraRotation = other.flipExtraRotation;
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00210C7C File Offset: 0x0020F07C
		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
			}
			else
			{
				ShaderTypeDef cutout = this.shaderType;
				if (cutout == null)
				{
					cutout = ShaderTypeDefOf.Cutout;
				}
				Shader shader = cutout.Shader;
				this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters);
				if (this.onGroundRandomRotateAngle > 0.01f)
				{
					this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
				}
				if (this.Linked)
				{
					this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
				}
			}
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x00210D36 File Offset: 0x0020F136
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x00210D50 File Offset: 0x0020F150
		public Graphic GraphicColoredFor(Thing t)
		{
			Graphic result;
			if (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo))
			{
				result = this.Graphic;
			}
			else
			{
				result = this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
			}
			return result;
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x00210DCC File Offset: 0x0020F1CC
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.graphicClass == null)
			{
				yield return "graphicClass is null";
			}
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is null or empty";
			}
			if (thingDef != null)
			{
				if (thingDef.drawerType == DrawerType.RealtimeOnly && this.Linked)
				{
					yield return "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
				}
			}
			if ((this.shaderType == ShaderTypeDefOf.Cutout || this.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
			{
				yield return "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
			}
			yield break;
		}
	}
}
