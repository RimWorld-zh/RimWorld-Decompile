using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000722 RID: 1826
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x040015FD RID: 5629
		private int startTick;

		// Token: 0x040015FE RID: 5630
		private int totalDuration;

		// Token: 0x040015FF RID: 5631
		private int fadeOutDuration;

		// Token: 0x04001600 RID: 5632
		private float angle;

		// Token: 0x04001601 RID: 5633
		private Sustainer sustainer;

		// Token: 0x04001602 RID: 5634
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x04001603 RID: 5635
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x04001604 RID: 5636
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x04001605 RID: 5637
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04001606 RID: 5638
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04001607 RID: 5639
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x001583F0 File Offset: 0x001567F0
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x00158410 File Offset: 0x00156810
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x00158438 File Offset: 0x00156838
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x0015845C File Offset: 0x0015685C
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x00158484 File Offset: 0x00156884
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x001584E4 File Offset: 0x001568E4
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x00158512 File Offset: 0x00156912
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x00158524 File Offset: 0x00156924
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

		// Token: 0x0600284C RID: 10316 RVA: 0x00158578 File Offset: 0x00156978
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.TicksLeft > 0)
			{
				Vector3 drawPos = this.parent.DrawPos;
				float num = ((float)this.parent.Map.Size.z - drawPos.z) * 1.41421354f;
				Vector3 a = Vector3Utility.FromAngleFlat(this.angle - 90f);
				Vector3 a2 = drawPos + a * num * 0.5f;
				a2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				float num2 = Mathf.Min((float)this.TicksPassed / 10f, 1f);
				Vector3 b = a * ((1f - num2) * num);
				float num3 = 0.975f + Mathf.Sin((float)this.TicksPassed * 0.3f) * 0.025f;
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
				pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(pos, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, this.BeamEndHeight));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, CompOrbitalBeam.BeamEndMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
			}
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x00158795 File Offset: 0x00156B95
		private void CheckSpawnSustainer()
		{
			if (this.TicksLeft >= this.fadeOutDuration && this.Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
				});
			}
		}
	}
}
