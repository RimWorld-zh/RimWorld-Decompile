using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Pawn_MeleeVerbs : IExposable
	{
		private Pawn pawn;

		private Verb curMeleeVerb = null;

		private Thing curMeleeVerbTarget = null;

		private int curMeleeVerbUpdateTick = 0;

		private Pawn_MeleeVerbs_TerrainSource terrainVerbs = null;

		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		private const int BestMeleeVerbUpdateInterval = 60;

		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
		}

		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		private void ChooseMeleeVerb(Thing target)
		{
			List<VerbEntry> updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList();
			VerbEntry verbEntry;
			if (updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry))
			{
				this.SetCurMeleeVerb(verbEntry.verb, target);
			}
			else
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn.ToStringSafe<Pawn>(),
					" has no available melee attack, spawned=",
					this.pawn.Spawned,
					" dead=",
					this.pawn.Dead,
					" downed=",
					this.pawn.Downed,
					" curJob=",
					this.pawn.CurJob.ToStringSafe<Job>(),
					" verbList=",
					updatedAvailableVerbsList.ToStringSafeEnumerable(),
					" bodyVerbs=",
					this.pawn.verbTracker.AllVerbs.ToStringSafeEnumerable()
				}), this.pawn.thingIDNumber ^ 195867354, false);
				this.SetCurMeleeVerb(null, null);
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
						return false;
					}
					if (!verbToUse.IsMeleeAttack)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Pawn ",
							this.pawn,
							" tried to melee attack ",
							target,
							" with non melee-attack verb ",
							verbToUse,
							"."
						}), false);
						return false;
					}
				}
				Verb verb;
				if (verbToUse != null)
				{
					verb = verbToUse;
				}
				else
				{
					verb = this.TryGetMeleeVerb(target);
				}
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
								if (allVerbs2[k].IsStillUsableBy(this.pawn))
								{
									Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(allVerbs2[k], this.pawn, thingWithComps));
								}
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
								if (allVerbs3[m].IsStillUsableBy(this.pawn))
								{
									Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(allVerbs3[m], this.pawn, apparel));
								}
							}
						}
					}
				}
			}
			foreach (Verb verb in this.pawn.health.hediffSet.GetHediffsVerbs())
			{
				if (verb.IsStillUsableBy(this.pawn))
				{
					Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(verb, this.pawn, null));
				}
			}
			if (this.pawn.Spawned)
			{
				TerrainDef terrainDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
				if (this.terrainVerbs == null || this.terrainVerbs.def != terrainDef)
				{
					this.terrainVerbs = Pawn_MeleeVerbs_TerrainSource.Create(this, terrainDef);
				}
				List<Verb> allVerbs4 = this.terrainVerbs.tracker.AllVerbs;
				for (int n = 0; n < allVerbs4.Count; n++)
				{
					Verb verb2 = allVerbs4[n];
					if (verb2.IsStillUsableBy(this.pawn))
					{
						Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(verb2, this.pawn, null));
					}
				}
			}
			Profiler.EndSample();
			return Pawn_MeleeVerbs.meleeVerbs;
		}

		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		private void SetCurMeleeVerb(Verb v, Thing target)
		{
			this.curMeleeVerb = v;
			this.curMeleeVerbTarget = target;
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
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.curMeleeVerb != null && !this.curMeleeVerb.IsStillUsableBy(this.pawn))
				{
					this.curMeleeVerb = null;
				}
			}
			Scribe_References.Look<Verb>(ref this.curMeleeVerb, "curMeleeVerb", false);
			Scribe_Values.Look<int>(ref this.curMeleeVerbUpdateTick, "curMeleeVerbUpdateTick", 0, false);
			Scribe_Deep.Look<Pawn_MeleeVerbs_TerrainSource>(ref this.terrainVerbs, "terrainVerbs", new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.terrainVerbs != null)
				{
					this.terrainVerbs.parent = this;
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Pawn_MeleeVerbs()
		{
		}

		[CompilerGenerated]
		private sealed class <ChooseMeleeVerb>c__AnonStorey0
		{
			internal Thing target;

			public <ChooseMeleeVerb>c__AnonStorey0()
			{
			}

			internal float <>m__0(VerbEntry ve)
			{
				return ve.GetSelectionWeight(this.target);
			}
		}
	}
}
