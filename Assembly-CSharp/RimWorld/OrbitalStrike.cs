using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BA RID: 1722
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x04001479 RID: 5241
		public int duration;

		// Token: 0x0400147A RID: 5242
		public Thing instigator;

		// Token: 0x0400147B RID: 5243
		public ThingDef weaponDef;

		// Token: 0x0400147C RID: 5244
		private float angle;

		// Token: 0x0400147D RID: 5245
		private int startTick;

		// Token: 0x0400147E RID: 5246
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x0400147F RID: 5247
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x04001480 RID: 5248
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x04001481 RID: 5249
		private const int OrbitalBeamFadeOutTicks = 10;

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x0013E488 File Offset: 0x0013C888
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x0013E4B0 File Offset: 0x0013C8B0
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x0013E4D4 File Offset: 0x0013C8D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0013E543 File Offset: 0x0013C943
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x0013E54C File Offset: 0x0013C94C
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

		// Token: 0x06002514 RID: 9492 RVA: 0x0013E5D2 File Offset: 0x0013C9D2
		public override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
