using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000419 RID: 1049
	public class CompHibernatable : ThingComp
	{
		// Token: 0x04000B02 RID: 2818
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x04000B03 RID: 2819
		private int endStartupTick = 0;

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x0009DAB4 File Offset: 0x0009BEB4
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x0009DAD4 File Offset: 0x0009BED4
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x0009DAEF File Offset: 0x0009BEEF
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
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x0009DB24 File Offset: 0x0009BF24
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0009DB46 File Offset: 0x0009BF46
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0009DB70 File Offset: 0x0009BF70
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.info.parent.Notify_HibernatableChanged();
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0009DB8C File Offset: 0x0009BF8C
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

		// Token: 0x0600121F RID: 4639 RVA: 0x0009DBF4 File Offset: 0x0009BFF4
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

		// Token: 0x06001220 RID: 4640 RVA: 0x0009DC6C File Offset: 0x0009C06C
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

		// Token: 0x06001221 RID: 4641 RVA: 0x0009DD14 File Offset: 0x0009C114
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}
	}
}
