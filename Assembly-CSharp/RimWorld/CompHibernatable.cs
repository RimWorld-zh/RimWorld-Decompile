using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000419 RID: 1049
	public class CompHibernatable : ThingComp
	{
		// Token: 0x04000AFF RID: 2815
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x04000B00 RID: 2816
		private int endStartupTick = 0;

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x0009DAA4 File Offset: 0x0009BEA4
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x0009DAC4 File Offset: 0x0009BEC4
		// (set) Token: 0x0600121B RID: 4635 RVA: 0x0009DADF File Offset: 0x0009BEDF
		public HibernatableStateDef State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					this.state = value;
					this.parent.Map.info.parent.Notify_HibernatableChanged();
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x0009DB14 File Offset: 0x0009BF14
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0009DB36 File Offset: 0x0009BF36
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0009DB60 File Offset: 0x0009BF60
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.info.parent.Notify_HibernatableChanged();
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0009DB7C File Offset: 0x0009BF7C
		public void Startup()
		{
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				Log.ErrorOnce("Attempted to start a non-hibernating object", 34361223, false);
			}
			else
			{
				this.State = HibernatableStateDefOf.Starting;
				this.endStartupTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + this.Props.startupDays * 60000f);
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0009DBE4 File Offset: 0x0009BFE4
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.State == HibernatableStateDefOf.Hibernating)
			{
				result = "HibernatableHibernating".Translate();
			}
			else if (this.State == HibernatableStateDefOf.Starting)
			{
				result = string.Format("{0}: {1}", "HibernatableStartingUp".Translate(), (this.endStartupTick - Find.TickManager.TicksGame).ToStringTicksToPeriod());
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0009DC5C File Offset: 0x0009C05C
		public override void CompTick()
		{
			if (this.State == HibernatableStateDefOf.Starting && Find.TickManager.TicksGame > this.endStartupTick)
			{
				this.State = HibernatableStateDefOf.Running;
				this.endStartupTick = 0;
				string text;
				if (this.parent.Map.Parent.GetComponent<EscapeShipComp>() != null)
				{
					text = "LetterHibernateComplete".Translate();
				}
				else
				{
					text = "LetterHibernateCompleteStandalone".Translate();
				}
				Find.LetterStack.ReceiveLetter("LetterLabelHibernateComplete".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.parent), null, null);
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0009DD04 File Offset: 0x0009C104
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}
	}
}
