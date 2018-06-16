using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CF0 RID: 3312
	public class PawnDownedWiggler
	{
		// Token: 0x060048D0 RID: 18640 RVA: 0x0026289E File Offset: 0x00260C9E
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x002628C8 File Offset: 0x00260CC8
		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5f)
				{
					num += 180f;
				}
				return num;
			}
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x00262908 File Offset: 0x00260D08
		public void WigglerTick()
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon--;
				if (this.ticksToIncapIcon <= 0)
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_IncapIcon);
					this.ticksToIncapIcon = 200;
				}
				if (this.pawn.Awake())
				{
					int num = Find.TickManager.TicksGame % 300 * 2;
					if (num < 90)
					{
						this.downedAngle += 0.35f;
					}
					else if (num < 390 && num >= 300)
					{
						this.downedAngle -= 0.35f;
					}
				}
			}
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x002629FC File Offset: 0x00260DFC
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x00262A10 File Offset: 0x00260E10
		public void Notify_DamageApplied(DamageInfo dam)
		{
			if ((this.pawn.Downed || this.pawn.Dead) && dam.Def.hasForcefulImpact)
			{
				this.downedAngle += 10f * Rand.Range(-1f, 1f);
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315f)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45f)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135f && this.downedAngle < 225f)
					{
						if (this.downedAngle > 180f)
						{
							this.downedAngle = 225f;
						}
						else
						{
							this.downedAngle = 135f;
						}
					}
				}
				else
				{
					if (this.downedAngle >= 360f)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0f)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}

		// Token: 0x0400314E RID: 12622
		private Pawn pawn;

		// Token: 0x0400314F RID: 12623
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x04003150 RID: 12624
		public int ticksToIncapIcon = 0;

		// Token: 0x04003151 RID: 12625
		private bool usingCustomRotation = false;

		// Token: 0x04003152 RID: 12626
		private const float DownedAngleWidth = 45f;

		// Token: 0x04003153 RID: 12627
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x04003154 RID: 12628
		private const int IncapWigglePeriod = 300;

		// Token: 0x04003155 RID: 12629
		private const int IncapWiggleLength = 90;

		// Token: 0x04003156 RID: 12630
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x04003157 RID: 12631
		private const int TicksBetweenIncapIcons = 200;
	}
}
