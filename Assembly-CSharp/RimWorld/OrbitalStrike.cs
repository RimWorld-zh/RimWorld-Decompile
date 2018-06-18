using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BC RID: 1724
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002514 RID: 9492 RVA: 0x0013DF88 File Offset: 0x0013C388
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x0013DFB0 File Offset: 0x0013C3B0
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x0013DFD4 File Offset: 0x0013C3D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x0013E043 File Offset: 0x0013C443
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x0013E04C File Offset: 0x0013C44C
		public virtual void StartStrike()
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartStrike() on unspawned thing.", false);
			}
			else
			{
				this.angle = OrbitalStrike.AngleRange.RandomInRange;
				this.startTick = Find.TickManager.TicksGame;
				base.GetComp<CompAffectsSky>().StartFadeInHoldFadeOut(30, this.duration - 30 - 15, 15, 1f);
				base.GetComp<CompOrbitalBeam>().StartAnimation(this.duration, 10, this.angle);
			}
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x0013E0D2 File Offset: 0x0013C4D2
		public override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04001477 RID: 5239
		public int duration;

		// Token: 0x04001478 RID: 5240
		public Thing instigator;

		// Token: 0x04001479 RID: 5241
		public ThingDef weaponDef;

		// Token: 0x0400147A RID: 5242
		private float angle;

		// Token: 0x0400147B RID: 5243
		private int startTick;

		// Token: 0x0400147C RID: 5244
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x0400147D RID: 5245
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x0400147E RID: 5246
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x0400147F RID: 5247
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
