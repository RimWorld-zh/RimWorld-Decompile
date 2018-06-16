using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000417 RID: 1047
	public class CompHibernatable : ThingComp
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x0009D770 File Offset: 0x0009BB70
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x0009D790 File Offset: 0x0009BB90
		// (set) Token: 0x06001217 RID: 4631 RVA: 0x0009D7AB File Offset: 0x0009BBAB
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
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x0009D7E0 File Offset: 0x0009BBE0
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0009D802 File Offset: 0x0009BC02
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0009D82C File Offset: 0x0009BC2C
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.info.parent.Notify_HibernatableChanged();
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0009D848 File Offset: 0x0009BC48
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

		// Token: 0x0600121C RID: 4636 RVA: 0x0009D8B0 File Offset: 0x0009BCB0
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

		// Token: 0x0600121D RID: 4637 RVA: 0x0009D928 File Offset: 0x0009BD28
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

		// Token: 0x0600121E RID: 4638 RVA: 0x0009D9D0 File Offset: 0x0009BDD0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}

		// Token: 0x04000AFE RID: 2814
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x04000AFF RID: 2815
		private int endStartupTick = 0;
	}
}
