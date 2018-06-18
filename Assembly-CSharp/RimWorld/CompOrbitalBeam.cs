using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000726 RID: 1830
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x00158234 File Offset: 0x00156634
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x00158254 File Offset: 0x00156654
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x0015827C File Offset: 0x0015667C
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x001582A0 File Offset: 0x001566A0
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x001582C8 File Offset: 0x001566C8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x00158328 File Offset: 0x00156728
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x00158356 File Offset: 0x00156756
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x00158368 File Offset: 0x00156768
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

		// Token: 0x06002854 RID: 10324 RVA: 0x001583BC File Offset: 0x001567BC
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

		// Token: 0x06002855 RID: 10325 RVA: 0x001585D9 File Offset: 0x001569D9
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

		// Token: 0x040015FF RID: 5631
		private int startTick;

		// Token: 0x04001600 RID: 5632
		private int totalDuration;

		// Token: 0x04001601 RID: 5633
		private int fadeOutDuration;

		// Token: 0x04001602 RID: 5634
		private float angle;

		// Token: 0x04001603 RID: 5635
		private Sustainer sustainer;

		// Token: 0x04001604 RID: 5636
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x04001605 RID: 5637
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x04001606 RID: 5638
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x04001607 RID: 5639
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04001608 RID: 5640
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04001609 RID: 5641
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
	}
}
