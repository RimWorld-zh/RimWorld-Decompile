using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		private int startTick;

		private int totalDuration;

		private int fadeOutDuration;

		private float angle;

		private Sustainer sustainer;

		private const float AlphaAnimationSpeed = 0.3f;

		private const float AlphaAnimationStrength = 0.025f;

		private const float BeamEndHeightRatio = 0.5f;

		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)base.props;
			}
		}

		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		private float BeamEndHeight
		{
			get
			{
				return (float)(this.Props.width * 0.5);
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		public override void CompTick()
		{
			base.CompTick();
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
				if (this.TicksLeft < this.fadeOutDuration)
				{
					this.sustainer.End();
					this.sustainer = null;
				}
			}
		}

		public override void PostDraw()
		{
			base.PostDraw();
			if (this.TicksLeft > 0)
			{
				Vector3 drawPos = base.parent.DrawPos;
				IntVec3 size = base.parent.Map.Size;
				float num = (float)(((float)size.z - drawPos.z) * 1.4142135381698608);
				Vector3 a = Vector3Utility.FromAngleFlat((float)(this.angle - 90.0));
				Vector3 a2 = drawPos + a * num * 0.5f;
				a2.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
				float num2 = Mathf.Min((float)((float)this.TicksPassed / 10.0), 1f);
				Vector3 b = a * (float)((1.0 - num2) * num);
				float num3 = (float)(0.97500002384185791 + Mathf.Sin((float)((float)this.TicksPassed * 0.30000001192092896)) * 0.02500000037252903);
				if (this.TicksLeft < this.fadeOutDuration)
				{
					num3 *= (float)this.TicksLeft / (float)this.fadeOutDuration;
				}
				Color color = this.Props.color;
				color.a *= num3;
				CompOrbitalBeam.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a2 + a * this.BeamEndHeight * 0.5f + b, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, num));
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompOrbitalBeam.BeamMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
				Vector3 pos = drawPos + b;
				pos.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(pos, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, this.BeamEndHeight));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, CompOrbitalBeam.BeamEndMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
			}
		}

		private void CheckSpawnSustainer()
		{
			if (this.TicksLeft >= this.fadeOutDuration && this.Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate
				{
					this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap((Thing)base.parent, MaintenanceType.PerTick));
				});
			}
		}
	}
}
