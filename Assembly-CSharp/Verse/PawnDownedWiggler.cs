using RimWorld;

namespace Verse
{
	public class PawnDownedWiggler
	{
		private Pawn pawn;

		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		public int ticksToIncapIcon = 0;

		private bool usingCustomRotation = false;

		private const float DownedAngleWidth = 45f;

		private const float DamageTakenDownedAngleShift = 10f;

		private const int IncapWigglePeriod = 300;

		private const int IncapWiggleLength = 90;

		private const float IncapWiggleSpeed = 0.35f;

		private const int TicksBetweenIncapIcons = 200;

		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5)
				{
					num = (float)(num + 180.0);
				}
				return num;
			}
		}

		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
		}

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

		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		public void Notify_DamageApplied(DamageInfo dam)
		{
			if (!this.pawn.Downed && !this.pawn.Dead)
				return;
			if (dam.Def.hasForcefulImpact)
			{
				this.downedAngle += (float)(10.0 * Rand.Range(-1f, 1f));
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315.0)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45.0)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135.0 && this.downedAngle < 225.0)
					{
						if (this.downedAngle > 180.0)
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
					if (this.downedAngle >= 360.0)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0.0)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}
	}
}
