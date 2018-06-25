using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BCB RID: 3019
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x04002CEB RID: 11499
		protected int logID = 0;

		// Token: 0x04002CEC RID: 11500
		protected int ticksAbs = -1;

		// Token: 0x04002CED RID: 11501
		public LogEntryDef def;

		// Token: 0x04002CEE RID: 11502
		private WeakReference<Thing> cachedStringPov = null;

		// Token: 0x04002CEF RID: 11503
		private string cachedString = null;

		// Token: 0x04002CF0 RID: 11504
		private float cachedHeightWidth;

		// Token: 0x04002CF1 RID: 11505
		private float cachedHeight;

		// Token: 0x04002CF2 RID: 11506
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x04002CF3 RID: 11507
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04002CF4 RID: 11508
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04002CF5 RID: 11509
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04002CF6 RID: 11510
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04002CF7 RID: 11511
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);

		// Token: 0x060041B8 RID: 16824 RVA: 0x002264A4 File Offset: 0x002248A4
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x060041B9 RID: 16825 RVA: 0x00226508 File Offset: 0x00224908
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x00226530 File Offset: 0x00224930
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x0022654C File Offset: 0x0022494C
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x00226568 File Offset: 0x00224968
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00226583 File Offset: 0x00224983
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x002265BC File Offset: 0x002249BC
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

		// Token: 0x060041BF RID: 16831 RVA: 0x00226690 File Offset: 0x00224A90
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			string rootKeyword = "r_logentry";
			GrammarRequest request = this.GenerateGrammarRequest();
			return GrammarResolver.Resolve(rootKeyword, request, null, forceLog);
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x002266C0 File Offset: 0x00224AC0
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x002266E0 File Offset: 0x00224AE0
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

		// Token: 0x060041C2 RID: 16834 RVA: 0x00226726 File Offset: 0x00224B26
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x060041C3 RID: 16835
		public abstract bool Concerns(Thing t);

		// Token: 0x060041C4 RID: 16836
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x060041C5 RID: 16837 RVA: 0x0022674D File Offset: 0x00224B4D
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x00226750 File Offset: 0x00224B50
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x00226768 File Offset: 0x00224B68
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(new object[]
			{
				this.Age.ToStringTicksToPeriod()
			}).CapitalizeFirst() + ".";
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x002267AC File Offset: 0x00224BAC
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x002267C2 File Offset: 0x00224BC2
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x002267CC File Offset: 0x00224BCC
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}
	}
}
