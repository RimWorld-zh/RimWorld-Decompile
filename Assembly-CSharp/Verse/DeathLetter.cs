using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E74 RID: 3700
	public class DeathLetter : ChoiceLetter
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06005708 RID: 22280 RVA: 0x002CBB68 File Offset: 0x002C9F68
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

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06005709 RID: 22281 RVA: 0x002CBBDC File Offset: 0x002C9FDC
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

		// Token: 0x0600570A RID: 22282 RVA: 0x002CBC08 File Offset: 0x002CA008
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
