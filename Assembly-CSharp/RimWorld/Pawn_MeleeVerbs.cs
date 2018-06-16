using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000515 RID: 1301
	public class Pawn_MeleeVerbs : IExposable
	{
		// Token: 0x0600177F RID: 6015 RVA: 0x000CDD1B File Offset: 0x000CC11B
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x000CDD48 File Offset: 0x000CC148
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000CDD63 File Offset: 0x000CC163
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x000CDD70 File Offset: 0x000CC170
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000CDDEC File Offset: 0x000CC1EC
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

		// Token: 0x06001784 RID: 6020 RVA: 0x000CDF20 File Offset: 0x000CC320
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

		// Token: 0x06001785 RID: 6021 RVA: 0x000CDFF8 File Offset: 0x000CC3F8
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

		// Token: 0x06001786 RID: 6022 RVA: 0x000CE36C File Offset: 0x000CC76C
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x000CE377 File Offset: 0x000CC777
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x000CE382 File Offset: 0x000CC782
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

		// Token: 0x06001789 RID: 6025 RVA: 0x000CE3BC File Offset: 0x000CC7BC
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

		// Token: 0x04000DE1 RID: 3553
		private Pawn pawn;

		// Token: 0x04000DE2 RID: 3554
		private Verb curMeleeVerb = null;

		// Token: 0x04000DE3 RID: 3555
		private Thing curMeleeVerbTarget = null;

		// Token: 0x04000DE4 RID: 3556
		private int curMeleeVerbUpdateTick = 0;

		// Token: 0x04000DE5 RID: 3557
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs = null;

		// Token: 0x04000DE6 RID: 3558
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		// Token: 0x04000DE7 RID: 3559
		private const int BestMeleeVerbUpdateInterval = 60;
	}
}
