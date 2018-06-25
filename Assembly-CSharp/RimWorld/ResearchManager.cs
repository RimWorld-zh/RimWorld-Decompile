using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public sealed class ResearchManager : IExposable
	{
		public ResearchProjectDef currentProj = null;

		private Dictionary<ResearchProjectDef, float> progress = new Dictionary<ResearchProjectDef, float>();

		private float GlobalProgressFactor = 0.007f;

		[CompilerGenerated]
		private static Predicate<ResearchProjectDef> <>f__am$cache0;

		[CompilerGenerated]
		private static Action <>f__am$cache1;

		public ResearchManager()
		{
		}

		public bool AnyProjectIsAvailable
		{
			get
			{
				return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find((ResearchProjectDef x) => x.CanStartNow) != null;
			}
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<ResearchProjectDef>(ref this.currentProj, "currentProj");
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.progress, "progress", LookMode.Def, LookMode.Value);
		}

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

		public void DebugSetAllProjectsFinished()
		{
			this.progress.Clear();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				this.progress.Add(researchProjectDef, researchProjectDef.baseCost);
			}
			this.ReapplyAllMods();
		}

		[CompilerGenerated]
		private static bool <get_AnyProjectIsAvailable>m__0(ResearchProjectDef x)
		{
			return x.CanStartNow;
		}

		[CompilerGenerated]
		private static void <DoCompletionDialog>m__1()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
		}
	}
}
