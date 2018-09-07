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
	public class PawnColumnDefgenerator
	{
		public PawnColumnDefgenerator()
		{
		}

		public static IEnumerable<PawnColumnDef> ImpliedPawnColumnDefs()
		{
			PawnTableDef animalsTable = PawnTableDefOf.Animals;
			foreach (TrainableDef sourceDef in from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td)
			{
				PawnColumnDef d3 = new PawnColumnDef();
				d3.defName = "Trainable_" + sourceDef.defName;
				d3.trainable = sourceDef;
				d3.headerIcon = sourceDef.icon;
				d3.workerClass = typeof(PawnColumnWorker_Trainable);
				d3.sortable = true;
				d3.headerTip = sourceDef.LabelCap;
				d3.paintable = true;
				d3.modContentPack = sourceDef.modContentPack;
				animalsTable.columns.Insert(animalsTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, d3);
				yield return d3;
			}
			PawnTableDef workTable = PawnTableDefOf.Work;
			bool moveWorkTypeLabelDown = false;
			foreach (WorkTypeDef def in (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where d.visible
			select d).Reverse<WorkTypeDef>())
			{
				moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
				PawnColumnDef d2 = new PawnColumnDef();
				d2.defName = "WorkPriority_" + def.defName;
				d2.workType = def;
				d2.moveWorkTypeLabelDown = moveWorkTypeLabelDown;
				d2.workerClass = typeof(PawnColumnWorker_WorkPriority);
				d2.sortable = true;
				d2.modContentPack = def.modContentPack;
				workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, d2);
				yield return d2;
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ImpliedPawnColumnDefs>c__Iterator0 : IEnumerable, IEnumerable<PawnColumnDef>, IEnumerator, IDisposable, IEnumerator<PawnColumnDef>
		{
			internal PawnTableDef <animalsTable>__0;

			internal IEnumerator<TrainableDef> $locvar0;

			internal TrainableDef <sourceDef>__1;

			internal PawnColumnDef <d>__2;

			internal PawnTableDef <workTable>__0;

			internal bool <moveWorkTypeLabelDown>__0;

			internal IEnumerator<WorkTypeDef> $locvar1;

			internal WorkTypeDef <def>__3;

			internal PawnColumnDef <d>__4;

			internal PawnColumnDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<TrainableDef, float> <>f__am$cache0;

			private static Predicate<PawnColumnDef> <>f__am$cache1;

			private static Func<WorkTypeDef, bool> <>f__am$cache2;

			private static Predicate<PawnColumnDef> <>f__am$cache3;

			[DebuggerHidden]
			public <ImpliedPawnColumnDefs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					animalsTable = PawnTableDefOf.Animals;
					enumerator = (from td in DefDatabase<TrainableDef>.AllDefsListForReading
					orderby td.listPriority descending
					select td).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_217;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						sourceDef = enumerator.Current;
						d = new PawnColumnDef();
						d.defName = "Trainable_" + sourceDef.defName;
						d.trainable = sourceDef;
						d.headerIcon = sourceDef.icon;
						d.workerClass = typeof(PawnColumnWorker_Trainable);
						d.sortable = true;
						d.headerTip = sourceDef.LabelCap;
						d.paintable = true;
						d.modContentPack = sourceDef.modContentPack;
						animalsTable.columns.Insert(animalsTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, d);
						this.$current = d;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				workTable = PawnTableDefOf.Work;
				moveWorkTypeLabelDown = false;
				enumerator2 = (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
				where d.visible
				select d).Reverse<WorkTypeDef>().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_217:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						def = enumerator2.Current;
						moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
						d2 = new PawnColumnDef();
						d2.defName = "WorkPriority_" + def.defName;
						d2.workType = def;
						d2.moveWorkTypeLabelDown = moveWorkTypeLabelDown;
						d2.workerClass = typeof(PawnColumnWorker_WorkPriority);
						d2.sortable = true;
						d2.modContentPack = def.modContentPack;
						workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, d2);
						this.$current = d2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			PawnColumnDef IEnumerator<PawnColumnDef>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.PawnColumnDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<PawnColumnDef> IEnumerable<PawnColumnDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new PawnColumnDefgenerator.<ImpliedPawnColumnDefs>c__Iterator0();
			}

			private static float <>m__0(TrainableDef td)
			{
				return td.listPriority;
			}

			private static bool <>m__1(PawnColumnDef x)
			{
				return x.Worker is PawnColumnWorker_Checkbox;
			}

			private static bool <>m__2(WorkTypeDef d)
			{
				return d.visible;
			}

			private static bool <>m__3(PawnColumnDef x)
			{
				return x.Worker is PawnColumnWorker_CopyPasteWorkPriorities;
			}
		}
	}
}
