using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000303 RID: 771
	public sealed class ResearchManager : IExposable
	{
		// Token: 0x04000855 RID: 2133
		public ResearchProjectDef currentProj = null;

		// Token: 0x04000856 RID: 2134
		private Dictionary<ResearchProjectDef, float> progress = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x04000857 RID: 2135
		private float GlobalProgressFactor = 0.007f;

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00070A4C File Offset: 0x0006EE4C
		public bool AnyProjectIsAvailable
		{
			get
			{
				return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find((ResearchProjectDef x) => x.CanStartNow) != null;
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00070A8E File Offset: 0x0006EE8E
		public void ExposeData()
		{
			Scribe_Defs.Look<ResearchProjectDef>(ref this.currentProj, "currentProj");
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.progress, "progress", LookMode.Def, LookMode.Value);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00070AB4 File Offset: 0x0006EEB4
		public float GetProgress(ResearchProjectDef proj)
		{
			float num;
			float result;
			if (this.progress.TryGetValue(proj, out num))
			{
				result = num;
			}
			else
			{
				this.progress.Add(proj, 0f);
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00070AFC File Offset: 0x0006EEFC
		public void ResearchPerformed(float amount, Pawn researcher)
		{
			if (this.currentProj == null)
			{
				Log.Error("Researched without having an active project.", false);
			}
			else
			{
				amount *= this.GlobalProgressFactor;
				if (researcher != null && researcher.Faction != null)
				{
					amount /= this.currentProj.CostFactor(researcher.Faction.def.techLevel);
				}
				if (DebugSettings.fastResearch)
				{
					amount *= 500f;
				}
				if (researcher != null)
				{
					researcher.records.AddTo(RecordDefOf.ResearchPointsResearched, amount);
				}
				float num = this.GetProgress(this.currentProj);
				num += amount;
				this.progress[this.currentProj] = num;
				if (this.currentProj.IsFinished)
				{
					this.ReapplyAllMods();
					this.DoCompletionDialog(this.currentProj, researcher);
					if (researcher != null)
					{
						TaleRecorder.RecordTale(TaleDefOf.FinishedResearchProject, new object[]
						{
							researcher,
							this.currentProj
						});
					}
					this.currentProj = null;
				}
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00070BFC File Offset: 0x0006EFFC
		public void ReapplyAllMods()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.IsFinished)
				{
					researchProjectDef.ReapplyAllMods();
				}
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00070C64 File Offset: 0x0006F064
		public void InstantFinish(ResearchProjectDef proj, bool doCompletionDialog = false)
		{
			if (proj.prerequisites != null)
			{
				for (int i = 0; i < proj.prerequisites.Count; i++)
				{
					if (!proj.prerequisites[i].IsFinished)
					{
						this.InstantFinish(proj.prerequisites[i], false);
					}
				}
			}
			this.progress[proj] = proj.baseCost;
			this.ReapplyAllMods();
			if (doCompletionDialog)
			{
				this.DoCompletionDialog(proj, null);
			}
			if (this.currentProj == proj)
			{
				this.currentProj = null;
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00070D00 File Offset: 0x0006F100
		private void DoCompletionDialog(ResearchProjectDef proj, Pawn researcher)
		{
			string text = "ResearchFinished".Translate(new object[]
			{
				this.currentProj.LabelCap
			}) + "\n\n" + this.currentProj.DescriptionDiscovered;
			DiaNode diaNode = new DiaNode(text);
			diaNode.options.Add(DiaOption.DefaultOK);
			DiaOption diaOption = new DiaOption("ResearchScreen".Translate());
			diaOption.resolveTree = true;
			diaOption.action = delegate()
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
			};
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00070DB4 File Offset: 0x0006F1B4
		public void DebugSetAllProjectsFinished()
		{
			this.progress.Clear();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				this.progress.Add(researchProjectDef, researchProjectDef.baseCost);
			}
			this.ReapplyAllMods();
		}
	}
}
