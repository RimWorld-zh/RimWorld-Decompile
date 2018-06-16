using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063F RID: 1599
	public abstract class ScenPart : IExposable
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00116E54 File Offset: 0x00115254
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x00116E70 File Offset: 0x00115270
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00116E90 File Offset: 0x00115290
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00116EA4 File Offset: 0x001152A4
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00116ED0 File Offset: 0x001152D0
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00116EF0 File Offset: 0x001152F0
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00116F00 File Offset: 0x00115300
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00116F20 File Offset: 0x00115320
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00116F43 File Offset: 0x00115343
		public virtual void Randomize()
		{
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00116F48 File Offset: 0x00115348
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00116F60 File Offset: 0x00115360
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00116F78 File Offset: 0x00115378
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00116F9C File Offset: 0x0011539C
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00116FB2 File Offset: 0x001153B2
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00116FB5 File Offset: 0x001153B5
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00116FB8 File Offset: 0x001153B8
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00116FBB File Offset: 0x001153BB
		public virtual void PreConfigure()
		{
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00116FBE File Offset: 0x001153BE
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00116FC1 File Offset: 0x001153C1
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00116FC4 File Offset: 0x001153C4
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00116FE7 File Offset: 0x001153E7
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00116FEA File Offset: 0x001153EA
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00116FED File Offset: 0x001153ED
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00116FF0 File Offset: 0x001153F0
		public virtual void Tick()
		{
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00116FF4 File Offset: 0x001153F4
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def == null)
			{
				yield return base.GetType().ToString() + " has null def.";
			}
			yield break;
		}

		// Token: 0x040012DB RID: 4827
		public ScenPartDef def;

		// Token: 0x040012DC RID: 4828
		public bool visible = true;

		// Token: 0x040012DD RID: 4829
		public bool summarized = false;
	}
}
