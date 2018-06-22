using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CC RID: 1740
	public class StunHandler : IExposable
	{
		// Token: 0x060025AD RID: 9645 RVA: 0x00142BF3 File Offset: 0x00140FF3
		public StunHandler(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x00142C18 File Offset: 0x00141018
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x00142C38 File Offset: 0x00141038
		private int EMPAdaptationTicksDuration
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				int result;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					result = 2200;
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00142C7B File Offset: 0x0014107B
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.EMPAdaptedTicksLeft, "EMPAdaptedTicksLeft", 0, false);
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00142CA4 File Offset: 0x001410A4
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

		// Token: 0x060025B2 RID: 9650 RVA: 0x00142D50 File Offset: 0x00141150
		public void Notify_DamageApplied(DamageInfo dinfo, bool affectedByEMP)
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn == null || (!pawn.Downed && !pawn.Dead))
			{
				if (dinfo.Def == DamageDefOf.Stun)
				{
					this.StunFor(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator);
				}
				else if (dinfo.Def == DamageDefOf.EMP && affectedByEMP)
				{
					if (this.EMPAdaptedTicksLeft <= 0)
					{
						this.StunFor(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator);
						this.EMPAdaptedTicksLeft = this.EMPAdaptationTicksDuration;
					}
					else
					{
						Vector3 loc = new Vector3((float)this.parent.Position.x + 1f, (float)this.parent.Position.y, (float)this.parent.Position.z + 1f);
						MoteMaker.ThrowText(loc, this.parent.Map, "Adapted".Translate(), Color.white, -1f);
					}
				}
			}
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00142E90 File Offset: 0x00141290
		public void StunFor(int ticks, Thing instigator)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
		}

		// Token: 0x04001510 RID: 5392
		public Thing parent;

		// Token: 0x04001511 RID: 5393
		private int stunTicksLeft = 0;

		// Token: 0x04001512 RID: 5394
		private Mote moteStun = null;

		// Token: 0x04001513 RID: 5395
		private int EMPAdaptedTicksLeft = 0;

		// Token: 0x04001514 RID: 5396
		public const float StunDurationTicksPerDamage = 30f;
	}
}
