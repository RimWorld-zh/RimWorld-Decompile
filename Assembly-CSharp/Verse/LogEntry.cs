using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC8 RID: 3016
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x04002CE4 RID: 11492
		protected int logID = 0;

		// Token: 0x04002CE5 RID: 11493
		protected int ticksAbs = -1;

		// Token: 0x04002CE6 RID: 11494
		public LogEntryDef def;

		// Token: 0x04002CE7 RID: 11495
		private WeakReference<Thing> cachedStringPov = null;

		// Token: 0x04002CE8 RID: 11496
		private string cachedString = null;

		// Token: 0x04002CE9 RID: 11497
		private float cachedHeightWidth;

		// Token: 0x04002CEA RID: 11498
		private float cachedHeight;

		// Token: 0x04002CEB RID: 11499
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x04002CEC RID: 11500
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04002CED RID: 11501
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04002CEE RID: 11502
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04002CEF RID: 11503
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04002CF0 RID: 11504
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);

		// Token: 0x060041B5 RID: 16821 RVA: 0x002260E8 File Offset: 0x002244E8
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x0022614C File Offset: 0x0022454C
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x00226174 File Offset: 0x00224574
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x00226190 File Offset: 0x00224590
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x060041B9 RID: 16825 RVA: 0x002261AC File Offset: 0x002245AC
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x002261C7 File Offset: 0x002245C7
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00226200 File Offset: 0x00224600
		public string ToGameStringFromPOV(Thing pov, bool forceLog = false)
		{
			if (this.cachedString == null || pov == null != (this.cachedStringPov == null) || (this.cachedStringPov != null && pov != this.cachedStringPov.Target) || DebugViewSettings.logGrammarResolution || forceLog)
			{
				Rand.PushState();
				try
				{
					Rand.Seed = this.logID;
					this.cachedStringPov = ((pov == null) ? null : new WeakReference<Thing>(pov));
					this.cachedString = this.ToGameStringFromPOV_Worker(pov, forceLog);
					this.cachedHeightWidth = 0f;
					this.cachedHeight = 0f;
				}
				finally
				{
					Rand.PopState();
				}
			}
			return this.cachedString;
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x002262D4 File Offset: 0x002246D4
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			string rootKeyword = "r_logentry";
			GrammarRequest request = this.GenerateGrammarRequest();
			return GrammarResolver.Resolve(rootKeyword, request, null, forceLog);
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00226304 File Offset: 0x00224704
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00226324 File Offset: 0x00224724
		public float GetTextHeight(Thing pov, float width)
		{
			string text = this.ToGameStringFromPOV(pov, false);
			if (this.cachedHeightWidth != width)
			{
				this.cachedHeightWidth = width;
				this.cachedHeight = Text.CalcHeight(text, width);
			}
			return this.cachedHeight;
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x0022636A File Offset: 0x0022476A
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x060041C0 RID: 16832
		public abstract bool Concerns(Thing t);

		// Token: 0x060041C1 RID: 16833
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x060041C2 RID: 16834 RVA: 0x00226391 File Offset: 0x00224791
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x00226394 File Offset: 0x00224794
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x002263AC File Offset: 0x002247AC
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(new object[]
			{
				this.Age.ToStringTicksToPeriod()
			}).CapitalizeFirst() + ".";
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x002263F0 File Offset: 0x002247F0
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x00226406 File Offset: 0x00224806
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x00226410 File Offset: 0x00224810
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}
	}
}
