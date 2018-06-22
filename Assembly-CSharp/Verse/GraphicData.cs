using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B18 RID: 2840
	public class GraphicData
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x00210768 File Offset: 0x0020EB68
		public bool Linked
		{
			get
			{
				return this.linkType != LinkDrawerType.None;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x0021078C File Offset: 0x0020EB8C
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

		// Token: 0x06003EB7 RID: 16055 RVA: 0x002107B8 File Offset: 0x0020EBB8
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

		// Token: 0x06003EB8 RID: 16056 RVA: 0x00210870 File Offset: 0x0020EC70
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

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0021092A File Offset: 0x0020ED2A
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x00210944 File Offset: 0x0020ED44
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

		// Token: 0x06003EBB RID: 16059 RVA: 0x002109C0 File Offset: 0x0020EDC0
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

		// Token: 0x04002834 RID: 10292
		[NoTranslate]
		public string texPath = null;

		// Token: 0x04002835 RID: 10293
		public Type graphicClass = null;

		// Token: 0x04002836 RID: 10294
		public ShaderTypeDef shaderType = null;

		// Token: 0x04002837 RID: 10295
		public List<ShaderParameter> shaderParameters = null;

		// Token: 0x04002838 RID: 10296
		public Color color = Color.white;

		// Token: 0x04002839 RID: 10297
		public Color colorTwo = Color.white;

		// Token: 0x0400283A RID: 10298
		public Vector2 drawSize = Vector2.one;

		// Token: 0x0400283B RID: 10299
		public float onGroundRandomRotateAngle = 0f;

		// Token: 0x0400283C RID: 10300
		public bool drawRotated = true;

		// Token: 0x0400283D RID: 10301
		public bool allowFlip = true;

		// Token: 0x0400283E RID: 10302
		public float flipExtraRotation = 0f;

		// Token: 0x0400283F RID: 10303
		public ShadowData shadowData = null;

		// Token: 0x04002840 RID: 10304
		public DamageGraphicData damageData = null;

		// Token: 0x04002841 RID: 10305
		public LinkDrawerType linkType = LinkDrawerType.None;

		// Token: 0x04002842 RID: 10306
		public LinkFlags linkFlags = LinkFlags.None;

		// Token: 0x04002843 RID: 10307
		[Unsaved]
		private Graphic cachedGraphic = null;
	}
}
