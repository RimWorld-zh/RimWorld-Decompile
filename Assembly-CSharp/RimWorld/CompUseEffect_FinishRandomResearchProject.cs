using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		public CompUseEffect_FinishRandomResearchProject()
		{
		}

		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			IEnumerable<ResearchProjectDef> availableResearchProjects = this.GetAvailableResearchProjects();
			if (availableResearchProjects.Any<ResearchProjectDef>())
			{
				this.FinishInstantly(availableResearchProjects.RandomElement<ResearchProjectDef>());
			}
			else if (Find.ResearchManager.currentProj != null && !Find.ResearchManager.currentProj.IsFinished)
			{
				this.FinishInstantly(Find.ResearchManager.currentProj);
			}
		}

		private IEnumerable<ResearchProjectDef> GetAvailableResearchProjects()
		{
			List<ResearchProjectDef> researchProjects = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < researchProjects.Count; i++)
			{
				if (researchProjects[i] != Find.ResearchManager.currentProj || Find.ResearchManager.currentProj.ProgressPercent < 0.2f)
				{
					if (!researchProjects[i].IsFinished && researchProjects[i].PrerequisitesCompleted)
					{
						yield return researchProjects[i];
					}
				}
			}
			yield break;
		}

		private void FinishInstantly(ResearchProjectDef proj)
		{
			Find.ResearchManager.InstantFinish(proj, false);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(new object[]
			{
				proj.LabelCap
			}), MessageTypeDefOf.PositiveEvent, true);
		}

		[CompilerGenerated]
		private sealed class <GetAvailableResearchProjects>c__Iterator0 : IEnumerable, IEnumerable<ResearchProjectDef>, IEnumerator, IDisposable, IEnumerator<ResearchProjectDef>
		{
			internal List<ResearchProjectDef> <researchProjects>__0;

			internal int <i>__1;

			internal ResearchProjectDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAvailableResearchProjects>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					researchProjects = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
					i = 0;
					goto IL_EE;
				case 1u:
					break;
				default:
					return false;
				}
				IL_E0:
				i++;
				IL_EE:
				if (i >= researchProjects.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (researchProjects[i] == Find.ResearchManager.currentProj && Find.ResearchManager.currentProj.ProgressPercent >= 0.2f)
					{
						goto IL_E0;
					}
					if (researchProjects[i].IsFinished || !researchProjects[i].PrerequisitesCompleted)
					{
						goto IL_E0;
					}
					this.$current = researchProjects[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			ResearchProjectDef IEnumerator<ResearchProjectDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.ResearchProjectDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ResearchProjectDef> IEnumerable<ResearchProjectDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new CompUseEffect_FinishRandomResearchProject.<GetAvailableResearchProjects>c__Iterator0();
			}
		}
	}
}
