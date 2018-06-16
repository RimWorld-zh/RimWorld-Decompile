using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BCC RID: 3020
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x060041B1 RID: 16817 RVA: 0x00225940 File Offset: 0x00223D40
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x002259A4 File Offset: 0x00223DA4
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x002259CC File Offset: 0x00223DCC
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x002259E8 File Offset: 0x00223DE8
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x00225A04 File Offset: 0x00223E04
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x00225A1F File Offset: 0x00223E1F
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x00225A58 File Offset: 0x00223E58
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

		// Token: 0x060041B8 RID: 16824 RVA: 0x00225B2C File Offset: 0x00223F2C
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			string rootKeyword = "r_logentry";
			GrammarRequest request = this.GenerateGrammarRequest();
			return GrammarResolver.Resolve(rootKeyword, request, null, forceLog);
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x00225B5C File Offset: 0x00223F5C
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x00225B7C File Offset: 0x00223F7C
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

		// Token: 0x060041BB RID: 16827 RVA: 0x00225BC2 File Offset: 0x00223FC2
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x060041BC RID: 16828
		public abstract bool Concerns(Thing t);

		// Token: 0x060041BD RID: 16829
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x060041BE RID: 16830 RVA: 0x00225BE9 File Offset: 0x00223FE9
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x00225BEC File Offset: 0x00223FEC
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x00225C04 File Offset: 0x00224004
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(new object[]
			{
				this.Age.ToStringTicksToPeriod()
			}).CapitalizeFirst() + ".";
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x00225C48 File Offset: 0x00224048
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x00225C5E File Offset: 0x0022405E
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x00225C68 File Offset: 0x00224068
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x04002CDF RID: 11487
		protected int logID = 0;

		// Token: 0x04002CE0 RID: 11488
		protected int ticksAbs = -1;

		// Token: 0x04002CE1 RID: 11489
		public LogEntryDef def;

		// Token: 0x04002CE2 RID: 11490
		private WeakReference<Thing> cachedStringPov = null;

		// Token: 0x04002CE3 RID: 11491
		private string cachedString = null;

		// Token: 0x04002CE4 RID: 11492
		private float cachedHeightWidth;

		// Token: 0x04002CE5 RID: 11493
		private float cachedHeight;

		// Token: 0x04002CE6 RID: 11494
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x04002CE7 RID: 11495
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04002CE8 RID: 11496
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04002CE9 RID: 11497
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04002CEA RID: 11498
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04002CEB RID: 11499
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
