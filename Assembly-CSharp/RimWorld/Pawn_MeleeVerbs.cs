using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000513 RID: 1299
	public class Pawn_MeleeVerbs : IExposable
	{
		// Token: 0x04000DDE RID: 3550
		private Pawn pawn;

		// Token: 0x04000DDF RID: 3551
		private Verb curMeleeVerb = null;

		// Token: 0x04000DE0 RID: 3552
		private Thing curMeleeVerbTarget = null;

		// Token: 0x04000DE1 RID: 3553
		private int curMeleeVerbUpdateTick = 0;

		// Token: 0x04000DE2 RID: 3554
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs = null;

		// Token: 0x04000DE3 RID: 3555
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		// Token: 0x04000DE4 RID: 3556
		private const int BestMeleeVerbUpdateInterval = 60;

		// Token: 0x0600177B RID: 6011 RVA: 0x000CDEB7 File Offset: 0x000CC2B7
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x000CDEE4 File Offset: 0x000CC2E4
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x000CDEFF File Offset: 0x000CC2FF
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x000CDF0C File Offset: 0x000CC30C
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000CDF88 File Offset: 0x000CC388
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

		// Token: 0x06001780 RID: 6016 RVA: 0x000CE0BC File Offset: 0x000CC4BC
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

		// Token: 0x06001781 RID: 6017 RVA: 0x000CE194 File Offset: 0x000CC594
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

		// Token: 0x06001782 RID: 6018 RVA: 0x000CE508 File Offset: 0x000CC908
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000CE513 File Offset: 0x000CC913
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x000CE51E File Offset: 0x000CC91E
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

		// Token: 0x06001785 RID: 6021 RVA: 0x000CE558 File Offset: 0x000CC958
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
	}
}
