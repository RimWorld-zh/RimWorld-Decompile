using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000563 RID: 1379
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x04000F41 RID: 3905
		private Faction faction;

		// Token: 0x04000F42 RID: 3906
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x04000F43 RID: 3907
		private const int TryRecruitInterval = 15051;

		// Token: 0x04000F44 RID: 3908
		private const float RecruitMTBDays = 30f;

		// Token: 0x06001A0B RID: 6667 RVA: 0x000E255F File Offset: 0x000E095F
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x000E257C File Offset: 0x000E097C
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000E2598 File Offset: 0x000E0998
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, new object[0]);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000E25F4 File Offset: 0x000E09F4
		public void KidnapPawn(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + pawn, false);
			}
			else if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + pawn, false);
			}
			else
			{
				pawn.PreKidnapped(kidnapper);
				if (pawn.Spawned)
				{
					pawn.DeSpawn(DestroyMode.Vanish);
				}
				this.kidnappedPawns.Add(pawn);
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					if (!Find.WorldPawns.Contains(pawn))
					{
						Log.Error("WorldPawns discarded kidnapped pawn.", false);
						this.kidnappedPawns.Remove(pawn);
					}
				}
				if (pawn.Faction == Faction.OfPlayer)
				{
					BillUtility.Notify_ColonistUnavailable(pawn);
				}
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000E26D5 File Offset: 0x000E0AD5
		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (!this.kidnappedPawns.Remove(pawn))
			{
				Log.Warning("Tried to remove kidnapped pawn " + pawn + " but he's not here.", false);
			}
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000E2700 File Offset: 0x000E0B00
		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000E2778 File Offset: 0x000E0B78
		public void KidnappedPawnsTrackerTick()
		{
			for (int i = this.kidnappedPawns.Count - 1; i >= 0; i--)
			{
				if (this.kidnappedPawns[i].DestroyedOrNull())
				{
					this.kidnappedPawns.RemoveAt(i);
				}
			}
			if (Find.TickManager.TicksGame % 15051 == 0)
			{
				for (int j = this.kidnappedPawns.Count - 1; j >= 0; j--)
				{
					if (Rand.MTBEventOccurs(30f, 60000f, 15051f))
					{
						this.kidnappedPawns[j].SetFaction(this.faction, null);
						this.kidnappedPawns.RemoveAt(j);
					}
				}
			}
		}
	}
}
