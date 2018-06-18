using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063F RID: 1599
	public abstract class ScenPart : IExposable
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x00116ECC File Offset: 0x001152CC
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x00116EE8 File Offset: 0x001152E8
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00116F08 File Offset: 0x00115308
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00116F1C File Offset: 0x0011531C
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00116F48 File Offset: 0x00115348
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00116F68 File Offset: 0x00115368
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00116F78 File Offset: 0x00115378
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00116F98 File Offset: 0x00115398
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00116FBB File Offset: 0x001153BB
		public virtual void Randomize()
		{
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00116FC0 File Offset: 0x001153C0
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00116FD8 File Offset: 0x001153D8
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00116FF0 File Offset: 0x001153F0
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00117014 File Offset: 0x00115414
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0011702A File Offset: 0x0011542A
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0011702D File Offset: 0x0011542D
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00117030 File Offset: 0x00115430
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00117033 File Offset: 0x00115433
		public virtual void PreConfigure()
		{
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00117036 File Offset: 0x00115436
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00117039 File Offset: 0x00115439
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x0011703C File Offset: 0x0011543C
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0011705F File Offset: 0x0011545F
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00117062 File Offset: 0x00115462
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00117065 File Offset: 0x00115465
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x00117068 File Offset: 0x00115468
		public virtual void Tick()
		{
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0011706C File Offset: 0x0011546C
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
