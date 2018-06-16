using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FD RID: 1533
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001E79 RID: 7801 RVA: 0x00108C64 File Offset: 0x00107064
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x00108C7F File Offset: 0x0010707F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x00108C9C File Offset: 0x0010709C
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

		// Token: 0x06001E7C RID: 7804 RVA: 0x00108CD6 File Offset: 0x001070D6
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x00108CF0 File Offset: 0x001070F0
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

		// Token: 0x04001216 RID: 4630
		private bool wonBattle;
	}
}
