using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063B RID: 1595
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
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x00116F78 File Offset: 0x00115378
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x00116F94 File Offset: 0x00115394
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00116FB4 File Offset: 0x001153B4
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x00116FC8 File Offset: 0x001153C8
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00116FF4 File Offset: 0x001153F4
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00117014 File Offset: 0x00115414
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00117024 File Offset: 0x00115424
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00117044 File Offset: 0x00115444
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00117067 File Offset: 0x00115467
		public virtual void Randomize()
		{
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0011706C File Offset: 0x0011546C
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00117084 File Offset: 0x00115484
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0011709C File Offset: 0x0011549C
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x001170C0 File Offset: 0x001154C0
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x001170D6 File Offset: 0x001154D6
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x001170D9 File Offset: 0x001154D9
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x001170DC File Offset: 0x001154DC
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x001170DF File Offset: 0x001154DF
		public virtual void PreConfigure()
		{
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x001170E2 File Offset: 0x001154E2
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x001170E5 File Offset: 0x001154E5
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x001170E8 File Offset: 0x001154E8
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0011710B File Offset: 0x0011550B
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0011710E File Offset: 0x0011550E
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00117111 File Offset: 0x00115511
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00117114 File Offset: 0x00115514
		public virtual void Tick()
		{
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00117118 File Offset: 0x00115518
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
