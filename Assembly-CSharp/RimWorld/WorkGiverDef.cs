using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiverDef : Def
	{
		public Type giverClass;

		public WorkTypeDef workType;

		public WorkTags workTags;

		public int priorityInType;

		[MustTranslate]
		public string verb;

		[MustTranslate]
		public string gerund;

		public bool scanThings = true;

		public bool scanCells;

		public bool emergency;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool directOrderable = true;

		public bool prioritizeSustains;

		public bool nonColonistsCanDo;

		public JobTag tagToGive = JobTag.MiscWork;

		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		public bool canBeDoneWhileDrafted;

		public int autoTakeablePriorityDrafted = -1;

		public ThingDef forceMote;

		public List<ThingDef> fixedBillGiverDefs;

		public bool billGiversAllHumanlikes;

		public bool billGiversAllHumanlikesCorpses;

		public bool billGiversAllMechanoids;

		public bool billGiversAllMechanoidsCorpses;

		public bool billGiversAllAnimals;

		public bool billGiversAllAnimalsCorpses;

		public bool tendToHumanlikesOnly;

		public bool tendToAnimalsOnly;

		public bool feedHumanlikesOnly;

		public bool feedAnimalsOnly;

		[Unsaved]
		private WorkGiver workerInt;

		public WorkGiverDef()
		{
		}

		public WorkGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in base.ConfigErrors())
			{
				yield return error;
			}
			if (this.verb.NullOrEmpty())
			{
				yield return this.defName + " lacks a verb.";
			}
			if (this.gerund.NullOrEmpty())
			{
				yield return this.defName + " lacks a gerund.";
			}
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <error>__1;

			internal WorkGiverDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_FC;
				case 3u:
					goto IL_140;
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
						error = enumerator.Current;
						this.$current = error;
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
				if (this.verb.NullOrEmpty())
				{
					this.$current = this.defName + " lacks a verb.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_FC:
				if (this.gerund.NullOrEmpty())
				{
					this.$current = this.defName + " lacks a gerund.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_140:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorkGiverDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new WorkGiverDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
