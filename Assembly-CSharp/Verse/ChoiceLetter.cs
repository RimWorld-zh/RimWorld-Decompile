using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E74 RID: 3700
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x060056FF RID: 22271
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06005700 RID: 22272 RVA: 0x001A049C File Offset: 0x0019E89C
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

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06005701 RID: 22273 RVA: 0x001A04DC File Offset: 0x0019E8DC
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

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06005702 RID: 22274 RVA: 0x001A0534 File Offset: 0x0019E934
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

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06005703 RID: 22275 RVA: 0x001A0574 File Offset: 0x0019E974
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

		// Token: 0x06005704 RID: 22276 RVA: 0x001A05E8 File Offset: 0x0019E9E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001A0628 File Offset: 0x0019EA28
		protected override string GetMouseoverText()
		{
			return this.text;
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001A0644 File Offset: 0x0019EA44
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

		// Token: 0x040039B3 RID: 14771
		public string title;

		// Token: 0x040039B4 RID: 14772
		public string text;

		// Token: 0x040039B5 RID: 14773
		public bool radioMode;
	}
}
