using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000565 RID: 1381
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x06001A10 RID: 6672 RVA: 0x000E20FF File Offset: 0x000E04FF
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x000E211C File Offset: 0x000E051C
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000E2138 File Offset: 0x000E0538
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, new object[0]);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000E2194 File Offset: 0x000E0594
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

		// Token: 0x06001A14 RID: 6676 RVA: 0x000E2275 File Offset: 0x000E0675
		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (!this.kidnappedPawns.Remove(pawn))
			{
				Log.Warning("Tried to remove kidnapped pawn " + pawn + " but he's not here.", false);
			}
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000E22A0 File Offset: 0x000E06A0
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

		// Token: 0x06001A16 RID: 6678 RVA: 0x000E2318 File Offset: 0x000E0718
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

		// Token: 0x04000F40 RID: 3904
		private Faction faction;

		// Token: 0x04000F41 RID: 3905
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x04000F42 RID: 3906
		private const int TryRecruitInterval = 15051;

		// Token: 0x04000F43 RID: 3907
		private const float RecruitMTBDays = 30f;
	}
}
