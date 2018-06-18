using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E73 RID: 3699
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x060056FD RID: 22269
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x060056FE RID: 22270 RVA: 0x001A0564 File Offset: 0x0019E964
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

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x060056FF RID: 22271 RVA: 0x001A05A4 File Offset: 0x0019E9A4
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

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06005700 RID: 22272 RVA: 0x001A05FC File Offset: 0x0019E9FC
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

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06005701 RID: 22273 RVA: 0x001A063C File Offset: 0x0019EA3C
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

		// Token: 0x06005702 RID: 22274 RVA: 0x001A06B0 File Offset: 0x0019EAB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x001A06F0 File Offset: 0x0019EAF0
		protected override string GetMouseoverText()
		{
			return this.text;
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x001A070C File Offset: 0x0019EB0C
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

		// Token: 0x040039B1 RID: 14769
		public string title;

		// Token: 0x040039B2 RID: 14770
		public string text;

		// Token: 0x040039B3 RID: 14771
		public bool radioMode;
	}
}
