using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E74 RID: 3700
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x040039C1 RID: 14785
		public string title;

		// Token: 0x040039C2 RID: 14786
		public string text;

		// Token: 0x040039C3 RID: 14787
		public bool radioMode;

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06005721 RID: 22305
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06005722 RID: 22306 RVA: 0x001A0894 File Offset: 0x0019EC94
		protected DiaOption Reject
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06005723 RID: 22307 RVA: 0x001A08D4 File Offset: 0x0019ECD4
		protected DiaOption Postpone
		{
			get
			{
				DiaOption diaOption = new DiaOption("PostponeLetter".Translate());
				diaOption.resolveTree = true;
				if (base.TimeoutActive && this.disappearAtTick <= Find.TickManager.TicksGame + 1)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06005724 RID: 22308 RVA: 0x001A092C File Offset: 0x0019ED2C
		protected DiaOption OK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06005725 RID: 22309 RVA: 0x001A096C File Offset: 0x0019ED6C
		protected DiaOption JumpToLocation
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x001A09E0 File Offset: 0x0019EDE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001A0A20 File Offset: 0x0019EE20
		protected override string GetMouseoverText()
		{
			return this.text;
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x001A0A3C File Offset: 0x0019EE3C
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			DiaNode nodeRoot = diaNode;
			Faction relatedFaction = this.relatedFaction;
			bool flag = this.radioMode;
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(nodeRoot, relatedFaction, false, flag, this.title);
			Find.WindowStack.Add(window);
		}
	}
}
