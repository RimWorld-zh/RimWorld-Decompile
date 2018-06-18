using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FD RID: 1533
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x00108CDC File Offset: 0x001070DC
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x00108CF7 File Offset: 0x001070F7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x00108D14 File Offset: 0x00107114
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

		// Token: 0x06001E7E RID: 7806 RVA: 0x00108D4E File Offset: 0x0010714E
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x00108D68 File Offset: 0x00107168
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
