using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063D RID: 1597
	public abstract class ScenPart : IExposable
	{
		// Token: 0x040012DC RID: 4828
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x040012DD RID: 4829
		public bool visible = true;

		// Token: 0x040012DE RID: 4830
		public bool summarized = false;

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x00117330 File Offset: 0x00115730
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x0011734C File Offset: 0x0011574C
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x0011736C File Offset: 0x0011576C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00117380 File Offset: 0x00115780
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x001173AC File Offset: 0x001157AC
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x001173CC File Offset: 0x001157CC
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x001173DC File Offset: 0x001157DC
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x001173FC File Offset: 0x001157FC
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0011741F File Offset: 0x0011581F
		public virtual void Randomize()
		{
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00117424 File Offset: 0x00115824
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0011743C File Offset: 0x0011583C
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00117454 File Offset: 0x00115854
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00117478 File Offset: 0x00115878
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0011748E File Offset: 0x0011588E
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00117491 File Offset: 0x00115891
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00117494 File Offset: 0x00115894
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00117497 File Offset: 0x00115897
		public virtual void PreConfigure()
		{
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0011749A File Offset: 0x0011589A
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0011749D File Offset: 0x0011589D
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x001174A0 File Offset: 0x001158A0
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x001174C3 File Offset: 0x001158C3
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x001174C6 File Offset: 0x001158C6
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x001174C9 File Offset: 0x001158C9
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x001174CC File Offset: 0x001158CC
		public virtual void Tick()
		{
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x001174D0 File Offset: 0x001158D0
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
