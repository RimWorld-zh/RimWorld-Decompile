using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class GraphicData
	{
		public string texPath = (string)null;

		public Type graphicClass = null;

		public ShaderType shaderType = ShaderType.None;

		public Color color = Color.white;

		public Color colorTwo = Color.white;

		public Vector2 drawSize = Vector2.one;

		public float onGroundRandomRotateAngle = 0f;

		public bool drawRotated = true;

		public bool allowFlip = true;

		public float flipExtraRotation = 0f;

		public ShadowData shadowData = null;

		public DamageGraphicData damageData = null;

		public LinkDrawerType linkType = LinkDrawerType.None;

		public LinkFlags linkFlags = LinkFlags.None;

		[Unsaved]
		private Graphic cachedGraphic = null;

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
			this.flipExtraRotation = other.flipExtraRotation;
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
			}
			else
			{
				ShaderType sType = this.shaderType;
				if (this.shaderType == ShaderType.None)
				{
					sType = ShaderType.Cutout;
				}
				Shader shader = ShaderDatabase.ShaderFromType(sType);
				this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this);
				if (this.onGroundRandomRotateAngle > 0.0099999997764825821)
				{
					this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
				}
				if (this.Linked)
				{
					this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
				}
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
			return (!t.DrawColor.IndistinguishableFrom(this.Graphic.Color) || !t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo)) ? this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo) : this.Graphic;
		}

		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.graphicClass == null)
			{
				yield return "graphicClass is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is null or empty";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (thingDef != null && thingDef.drawerType == DrawerType.RealtimeOnly && this.Linked)
			{
				yield return "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.shaderType != ShaderType.Cutout && this.shaderType != ShaderType.CutoutComplex)
				yield break;
			if (thingDef.mote == null)
				yield break;
			if (!(thingDef.mote.fadeInTime > 0.0) && !(thingDef.mote.fadeOutTime > 0.0))
				yield break;
			yield return "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
