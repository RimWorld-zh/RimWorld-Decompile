using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E73 RID: 3699
	public class DeathLetter : ChoiceLetter
	{
		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06005728 RID: 22312 RVA: 0x002CD778 File Offset: 0x002CBB78
		protected DiaOption ReadMore
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("ReadMore".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target);
					Find.LetterStack.RemoveLetter(this);
					InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
				};
				diaOption.resolveTree = true;
				if (!target.IsValid)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06005729 RID: 22313 RVA: 0x002CD7EC File Offset: 0x002CBBEC
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.OK;
				if (this.lookTargets.IsValid())
				{
					yield return this.ReadMore;
				}
				yield break;
			}
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x002CD818 File Offset: 0x002CBC18
		public override void OpenLetter()
		{
			Pawn targetPawn = this.lookTargets.TryGetPrimaryTarget().Thing as Pawn;
			string text = this.text;
			string text2 = (from entry in (from entry in (from battle in Find.BattleLog.Battles
			where battle.Concerns(targetPawn)
			select battle).SelectMany((Battle battle) => from entry in battle.Entries
			where entry.Concerns(targetPawn) && entry.ShowInCompactView()
			select entry)
			orderby entry.Age
			select entry).Take(5).Reverse<LogEntry>()
			select "  " + entry.ToGameStringFromPOV(null, false)).ToLineList("");
			if (text2.Length > 0)
			{
				text = string.Format("{0}\n\n{1}\n{2}", text, "LastEventsInLife".Translate(new object[]
				{
					targetPawn.LabelDefinite()
				}) + ":", text2);
			}
			DiaNode diaNode = new DiaNode(text);
			diaNode.options.AddRange(this.Choices);
			WindowStack windowStack = Find.WindowStack;
			DiaNode nodeRoot = diaNode;
			Faction relatedFaction = this.relatedFaction;
			bool radioMode = this.radioMode;
			windowStack.Add(new Dialog_NodeTreeWithFactionInfo(nodeRoot, relatedFaction, false, radioMode, this.title));
		}
	}
}
