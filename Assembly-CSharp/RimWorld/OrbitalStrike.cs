using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B8 RID: 1720
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x0013E0D0 File Offset: 0x0013C4D0
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x0013E0F8 File Offset: 0x0013C4F8
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x0013E11C File Offset: 0x0013C51C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x0013E18B File Offset: 0x0013C58B
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x0013E194 File Offset: 0x0013C594
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

		// Token: 0x06002511 RID: 9489 RVA: 0x0013E21A File Offset: 0x0013C61A
		public override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04001475 RID: 5237
		public int duration;

		// Token: 0x04001476 RID: 5238
		public Thing instigator;

		// Token: 0x04001477 RID: 5239
		public ThingDef weaponDef;

		// Token: 0x04001478 RID: 5240
		private float angle;

		// Token: 0x04001479 RID: 5241
		private int startTick;

		// Token: 0x0400147A RID: 5242
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x0400147B RID: 5243
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x0400147C RID: 5244
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x0400147D RID: 5245
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
