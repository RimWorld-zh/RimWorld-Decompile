using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class GraphicData
	{
		[NoTranslate]
		public string texPath = null;

		public Type graphicClass = null;

		public ShaderTypeDef shaderType = null;

		public List<ShaderParameter> shaderParameters = null;

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

		public GraphicData()
		{
		}

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

		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

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

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal ThingDef thingDef;

			internal GraphicData $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.graphicClass == null)
					{
						this.$current = "graphicClass is null";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_91;
				case 3u:
					goto IL_DD;
				case 4u:
					IL_16B:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (this.texPath.NullOrEmpty())
				{
					this.$current = "texPath is null or empty";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_91:
				if (thingDef != null)
				{
					if (thingDef.drawerType == DrawerType.RealtimeOnly && base.Linked)
					{
						this.$current = "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
				}
				IL_DD:
				if ((this.shaderType == ShaderTypeDefOf.Cutout || this.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
				{
					this.$current = "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				goto IL_16B;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GraphicData.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new GraphicData.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.thingDef = thingDef;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
