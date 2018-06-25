using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063D RID: 1597
	public abstract class ScenPart : IExposable
	{
		// Token: 0x040012D8 RID: 4824
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x040012D9 RID: 4825
		public bool visible = true;

		// Token: 0x040012DA RID: 4826
		public bool summarized = false;

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x001170C8 File Offset: 0x001154C8
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x001170E4 File Offset: 0x001154E4
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00117104 File Offset: 0x00115504
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00117118 File Offset: 0x00115518
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00117144 File Offset: 0x00115544
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00117164 File Offset: 0x00115564
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00117174 File Offset: 0x00115574
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00117194 File Offset: 0x00115594
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x001171B7 File Offset: 0x001155B7
		public virtual void Randomize()
		{
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x001171BC File Offset: 0x001155BC
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x001171D4 File Offset: 0x001155D4
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x001171EC File Offset: 0x001155EC
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00117210 File Offset: 0x00115610
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00117226 File Offset: 0x00115626
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00117229 File Offset: 0x00115629
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0011722C File Offset: 0x0011562C
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0011722F File Offset: 0x0011562F
		public virtual void PreConfigure()
		{
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00117232 File Offset: 0x00115632
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00117235 File Offset: 0x00115635
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00117238 File Offset: 0x00115638
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0011725B File Offset: 0x0011565B
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0011725E File Offset: 0x0011565E
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00117261 File Offset: 0x00115661
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00117264 File Offset: 0x00115664
		public virtual void Tick()
		{
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00117268 File Offset: 0x00115668
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def == null)
			{
				yield return base.GetType().ToString() + " has null def.";
			}
			yield break;
		}
	}
}
