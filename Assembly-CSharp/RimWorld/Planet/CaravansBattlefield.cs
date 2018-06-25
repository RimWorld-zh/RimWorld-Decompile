using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FB RID: 1531
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x04001217 RID: 4631
		private bool wonBattle;

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001E75 RID: 7797 RVA: 0x001090E8 File Offset: 0x001074E8
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00109103 File Offset: 0x00107503
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00109120 File Offset: 0x00107520
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			bool result;
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				result = true;
			}
			else
			{
				alsoRemoveWorldObject = false;
				result = false;
			}
			return result;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0010915A File Offset: 0x0010755A
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00109174 File Offset: 0x00107574
		private void CheckWonBattle()
		{
			if (!this.wonBattle)
			{
				if (!GenHostility.AnyHostileActiveThreatToPlayer(base.Map))
				{
					string forceExitAndRemoveMapCountdownTimeLeftString = TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(60000);
					Find.LetterStack.ReceiveLetter("LetterLabelCaravansBattlefieldVictory".Translate(), "LetterCaravansBattlefieldVictory".Translate(new object[]
					{
						forceExitAndRemoveMapCountdownTimeLeftString
					}), LetterDefOf.PositiveEvent, this, null, null);
					TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushDefeated, new object[]
					{
						base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
					this.wonBattle = true;
					base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown();
				}
			}
		}
	}
}
