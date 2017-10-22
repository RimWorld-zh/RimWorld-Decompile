#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	public class Pawn_MeleeVerbs : IExposable
	{
		private Pawn pawn;

		private Verb curMeleeVerb = null;

		private int curMeleeVerbUpdateTick = 0;

		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		private const int BestMeleeVerbUpdateInterval = 60;

		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public Verb TryGetMeleeVerb()
		{
			if (this.curMeleeVerb == null || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				this.ChooseMeleeVerb();
			}
			return this.curMeleeVerb;
		}

		private void ChooseMeleeVerb()
		{
			List<VerbEntry> updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList();
			if (updatedAvailableVerbsList.Count == 0)
			{
				Log.ErrorOnce(string.Format("{0} has no available melee attack", this.pawn), 1664289);
				this.SetCurMeleeVerb(null);
			}
			else
			{
				VerbEntry verbEntry = updatedAvailableVerbsList.RandomElementByWeight((Func<VerbEntry, float>)((VerbEntry ve) => ve.SelectionWeight));
				this.SetCurMeleeVerb(verbEntry.verb);
			}
		}

		public bool TryMeleeAttack(Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			bool result;
			if (this.pawn.stances.FullBodyBusy)
			{
				result = false;
			}
			else
			{
				if (verbToUse != null)
				{
					if (!verbToUse.IsStillUsableBy(this.pawn))
					{
						result = false;
						goto IL_00c8;
					}
					if (!(verbToUse is Verb_MeleeAttack))
					{
						Log.Warning("Pawn " + this.pawn + " tried to melee attack " + target + " with non melee-attack verb " + verbToUse + ".");
						result = false;
						goto IL_00c8;
					}
				}
				Verb verb = (verbToUse == null) ? this.TryGetMeleeVerb() : verbToUse;
				if (verb == null)
				{
					result = false;
				}
				else
				{
					verb.TryStartCastOn(target, surpriseAttack, true);
					result = true;
				}
			}
			goto IL_00c8;
			IL_00c8:
			return result;
		}

		public List<VerbEntry> GetUpdatedAvailableVerbsList()
		{
			Profiler.BeginSample("GetUpdatedAvailableVerbsList");
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			List<Verb> allVerbs = this.pawn.verbTracker.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsStillUsableBy(this.pawn))
				{
					Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(allVerbs[i], this.pawn, null));
				}
			}
			if (this.pawn.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = this.pawn.equipment.AllEquipmentListForReading;
				for (int j = 0; j < allEquipmentListForReading.Count; j++)
				{
					ThingWithComps thingWithComps = allEquipmentListForReading[j];
					CompEquippable comp = thingWithComps.GetComp<CompEquippable>();
					if (comp != null)
					{
						List<Verb> allVerbs2 = comp.AllVerbs;
						if (allVerbs2 != null)
						{
							for (int k = 0; k < allVerbs2.Count; k++)
							{
								Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(allVerbs2[k], this.pawn, thingWithComps));
							}
						}
					}
				}
			}
			if (this.pawn.apparel != null)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int l = 0; l < wornApparel.Count; l++)
				{
					Apparel apparel = wornApparel[l];
					CompEquippable comp2 = apparel.GetComp<CompEquippable>();
					if (comp2 != null)
					{
						List<Verb> allVerbs3 = comp2.AllVerbs;
						if (allVerbs3 != null)
						{
							for (int m = 0; m < allVerbs3.Count; m++)
							{
								Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(allVerbs3[m], this.pawn, apparel));
							}
						}
					}
				}
			}
			foreach (Verb hediffsVerb in this.pawn.health.hediffSet.GetHediffsVerbs())
			{
				if (hediffsVerb.IsStillUsableBy(this.pawn))
				{
					Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(hediffsVerb, this.pawn, null));
				}
			}
			Profiler.EndSample();
			return Pawn_MeleeVerbs.meleeVerbs;
		}

		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null);
		}

		private void SetCurMeleeVerb(Verb v)
		{
			this.curMeleeVerb = v;
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.curMeleeVerbUpdateTick = 0;
			}
			else
			{
				this.curMeleeVerbUpdateTick = Find.TickManager.TicksGame;
			}
		}

		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.curMeleeVerb != null && !this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				this.curMeleeVerb = null;
			}
			Scribe_References.Look<Verb>(ref this.curMeleeVerb, "curMeleeVerb", false);
			Scribe_Values.Look<int>(ref this.curMeleeVerbUpdateTick, "curMeleeVerbUpdateTick", 0, false);
		}
	}
}
