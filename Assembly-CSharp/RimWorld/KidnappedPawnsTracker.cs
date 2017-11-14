using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class KidnappedPawnsTracker : IExposable
	{
		private Faction faction;

		private List<Pawn> kidnappedPawns = new List<Pawn>();

		private const int TryRecruitInterval = 15051;

		private const float RecruitMTBDays = 30f;

		[CompilerGenerated]
		private static Predicate<Pawn> _003C_003Ef__mg_0024cache0;

		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, new object[0]);
		}

		public void KidnapPawn(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + pawn);
			}
			else if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + pawn);
			}
			else
			{
				pawn.PreKidnapped(kidnapper);
				if (pawn.Spawned)
				{
					pawn.DeSpawn();
				}
				this.kidnappedPawns.Add(pawn);
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					if (!Find.WorldPawns.Contains(pawn))
					{
						Log.Error("WorldPawns discarded kidnapped pawn.");
						this.kidnappedPawns.Remove(pawn);
					}
				}
			}
		}

		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (!this.kidnappedPawns.Remove(pawn))
			{
				Log.Warning("Tried to remove kidnapped pawn " + pawn + " but he's not here.");
			}
		}

		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString());
		}

		public void KidnappedPawnsTrackerTick()
		{
			this.kidnappedPawns.RemoveAll(ThingUtility.DestroyedOrNull);
			if (Find.TickManager.TicksGame % 15051 == 0)
			{
				for (int num = this.kidnappedPawns.Count - 1; num >= 0; num--)
				{
					if (Rand.MTBEventOccurs(30f, 60000f, 15051f))
					{
						this.kidnappedPawns[num].SetFaction(this.faction, null);
						this.kidnappedPawns.RemoveAt(num);
					}
				}
			}
		}
	}
}
