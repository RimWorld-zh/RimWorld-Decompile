using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F9 RID: 1529
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x00108D30 File Offset: 0x00107130
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00108D4B File Offset: 0x0010714B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x00108D68 File Offset: 0x00107168
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

		// Token: 0x06001E75 RID: 7797 RVA: 0x00108DA2 File Offset: 0x001071A2
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00108DBC File Offset: 0x001071BC
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

		// Token: 0x04001213 RID: 4627
		private bool wonBattle;
	}
}
