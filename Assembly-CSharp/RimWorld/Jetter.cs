using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006C9 RID: 1737
	public class Jetter : Thing
	{
		// Token: 0x040014F5 RID: 5365
		private Jetter.JetterState JState = Jetter.JetterState.Resting;

		// Token: 0x040014F6 RID: 5366
		private int WickTicksLeft = 0;

		// Token: 0x040014F7 RID: 5367
		private int TicksUntilMove = 0;

		// Token: 0x040014F8 RID: 5368
		protected Sustainer wickSoundSustainer = null;

		// Token: 0x040014F9 RID: 5369
		protected Sustainer jetSoundSustainer = null;

		// Token: 0x040014FA RID: 5370
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x040014FB RID: 5371
		private const int TicksBetweenMoves = 3;

		// Token: 0x060025A1 RID: 9633 RVA: 0x0014271C File Offset: 0x00140B1C
		public override void Tick()
		{
			if (this.JState == Jetter.JetterState.WickBurning)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
				this.WickTicksLeft--;
				if (this.WickTicksLeft == 0)
				{
					this.StartJetting();
				}
			}
			else if (this.JState == Jetter.JetterState.Jetting)
			{
				this.TicksUntilMove--;
				if (this.TicksUntilMove <= 0)
				{
					this.MoveJetter();
					this.TicksUntilMove = 3;
				}
			}
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x001427A5 File Offset: 0x00140BA5
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x001427DF File Offset: 0x00140BDF
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00142817 File Offset: 0x00140C17
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00142850 File Offset: 0x00140C50
		protected void MoveJetter()
		{
			IntVec3 intVec = base.Position + base.Rotation.FacingCell;
			if (!intVec.Walkable(base.Map) || base.Map.thingGrid.CellContains(intVec, ThingCategory.Pawn) || intVec.GetEdifice(base.Map) != null)
			{
				this.Destroy(DestroyMode.Vanish);
				GenExplosion.DoExplosion(base.Position, base.Map, 2.9f, DamageDefOf.Bomb, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
			else
			{
				base.Position = intVec;
			}
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x00142900 File Offset: 0x00140D00
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
			if (this.jetSoundSustainer != null)
			{
				this.jetSoundSustainer.End();
				this.jetSoundSustainer = null;
			}
		}

		// Token: 0x020006CA RID: 1738
		private enum JetterState
		{
			// Token: 0x040014FD RID: 5373
			Resting,
			// Token: 0x040014FE RID: 5374
			WickBurning,
			// Token: 0x040014FF RID: 5375
			Jetting
		}
	}
}
