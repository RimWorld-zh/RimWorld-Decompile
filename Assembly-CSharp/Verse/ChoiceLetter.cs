using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		public string title;

		public string text;

		public bool radioMode;

		protected ChoiceLetter()
		{
		}

		public abstract IEnumerable<DiaOption> Choices { get; }

		protected DiaOption Option_Dismiss
		{
			get
			{
				return new DiaOption("Dismiss".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		protected DiaOption Option_JumpToLocation
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

		protected DiaOption Option_Reject
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

		protected DiaOption Option_Postpone
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

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
		}

		protected override string GetMouseoverText()
		{
			return this.text;
		}

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

		[CompilerGenerated]
		private void <get_Option_Dismiss>m__0()
		{
			Find.LetterStack.RemoveLetter(this);
		}

		[CompilerGenerated]
		private void <get_Option_Reject>m__1()
		{
			Find.LetterStack.RemoveLetter(this);
		}

		[CompilerGenerated]
		private sealed class <>c__AnonStorey0
		{
			internal GlobalTargetInfo target;

			internal ChoiceLetter $this;

			public <>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				CameraJumper.TryJumpAndSelect(this.target);
				Find.LetterStack.RemoveLetter(this.$this);
			}
		}
	}
}
