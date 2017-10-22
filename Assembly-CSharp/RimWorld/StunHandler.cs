using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StunHandler : IExposable
	{
		public Thing parent;

		private int stunTicksLeft = 0;

		private Mote moteStun = null;

		private int EMPAdaptedTicksLeft = 0;

		private const float StunDurationFactor_Standard = 20f;

		private const float StunDurationFactor_EMP = 15f;

		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0;
			}
		}

		private int EMPAdaptationTicksDuration
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (pawn != null && pawn.RaceProps.IsMechanoid) ? 2200 : 0;
			}
		}

		public StunHandler(Thing parent)
		{
			this.parent = parent;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.EMPAdaptedTicksLeft, "EMPAdaptedTicksLeft", 0, false);
		}

		public void StunHandlerTick()
		{
			if (this.EMPAdaptedTicksLeft > 0)
			{
				this.EMPAdaptedTicksLeft--;
			}
			if (this.stunTicksLeft > 0)
			{
				this.stunTicksLeft--;
				if (this.moteStun == null || this.moteStun.Destroyed)
				{
					this.moteStun = MoteMaker.MakeStunOverlay(this.parent);
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.Downed)
				{
					this.stunTicksLeft = 0;
				}
				if (this.moteStun != null)
				{
					this.moteStun.Maintain();
				}
			}
		}

		public void Notify_DamageApplied(DamageInfo dinfo, bool affectedByEMP)
		{
			if (dinfo.Def == DamageDefOf.Stun)
			{
				this.StunFor(Mathf.RoundToInt((float)((float)dinfo.Amount * 20.0)));
			}
			else if (dinfo.Def == DamageDefOf.EMP && affectedByEMP)
			{
				if (this.EMPAdaptedTicksLeft <= 0)
				{
					this.StunFor(Mathf.RoundToInt((float)((float)dinfo.Amount * 15.0)));
					this.EMPAdaptedTicksLeft = this.EMPAdaptationTicksDuration;
				}
				else
				{
					IntVec3 position = this.parent.Position;
					double x = (float)position.x + 1.0;
					IntVec3 position2 = this.parent.Position;
					float y = (float)position2.y;
					IntVec3 position3 = this.parent.Position;
					Vector3 loc = new Vector3((float)x, y, (float)((float)position3.z + 1.0));
					MoteMaker.ThrowText(loc, this.parent.Map, "Adapted".Translate(), Color.white, -1f);
				}
			}
		}

		public void StunFor(int ticks)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
		}
	}
}
