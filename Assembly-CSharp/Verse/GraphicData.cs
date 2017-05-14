using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse
{
	public class GraphicData
	{
		public string texPath;

		public Type graphicClass;

		public ShaderType shaderType;

		public Color color = Color.white;

		public Color colorTwo = Color.white;

		public Vector2 drawSize = Vector2.one;

		public float onGroundRandomRotateAngle;

		public bool drawRotated = true;

		public bool allowFlip = true;

		public float flipExtraRotation;

		public ShadowData shadowData;

		public DamageGraphicData damageData;

		public LinkDrawerType linkType;

		public LinkFlags linkFlags;

		[Unsaved]
		private Graphic cachedGraphic;

		public bool Linked
		{
			get
			{
				return this.linkType != LinkDrawerType.None;
			}
		}

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
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
		}

		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
				return;
			}
			ShaderType sType = this.shaderType;
			if (this.shaderType == ShaderType.None)
			{
				sType = ShaderType.Cutout;
			}
			Shader shader = ShaderDatabase.ShaderFromType(sType);
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this);
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		public Graphic GraphicColoredFor(Thing t)
		{
			if (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		[DebuggerHidden]
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			GraphicData.<ConfigErrors>c__Iterator1C6 <ConfigErrors>c__Iterator1C = new GraphicData.<ConfigErrors>c__Iterator1C6();
			<ConfigErrors>c__Iterator1C.thingDef = thingDef;
			<ConfigErrors>c__Iterator1C.<$>thingDef = thingDef;
			<ConfigErrors>c__Iterator1C.<>f__this = this;
			GraphicData.<ConfigErrors>c__Iterator1C6 expr_1C = <ConfigErrors>c__Iterator1C;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
