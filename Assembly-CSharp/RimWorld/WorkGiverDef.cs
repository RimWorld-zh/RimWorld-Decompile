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
		public Type giverClass = null;

		public WorkTypeDef workType = null;

		public WorkTags workTags = WorkTags.None;

		public int priorityInType = 0;

		[MustTranslate]
		public string verb;

		[MustTranslate]
		public string gerund;

		public bool scanThings = true;

		public bool scanCells = false;

		public bool emergency = false;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool directOrderable = true;

		public bool prioritizeSustains = false;

		public bool canBeDoneByNonColonists = false;

		public JobTag tagToGive = JobTag.MiscWork;

		public WorkGiverEquivalenceGroupDef equivalenceGroup = null;

		public bool canBeDoneWhileDrafted = false;

		public int autoTakeablePriorityDrafted = -1;

		public ThingDef forceMote = null;

		public List<ThingDef> fixedBillGiverDefs = null;

		public bool billGiversAllHumanlikes = false;

		public bool billGiversAllHumanlikesCorpses = false;

		public bool billGiversAllMechanoids = false;

		public bool billGiversAllMechanoidsCorpses = false;

		public bool billGiversAllAnimals = false;

		public bool billGiversAllAnimalsCorpses = false;

		public bool tendToHumanlikesOnly = false;

		public bool tendToAnimalsOnly = false;

		public bool feedHumanlikesOnly = false;

		public bool feedAnimalsOnly = false;

		[Unsaved]
		private WorkGiver workerInt = null;

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
			foreach (string error in this.<ConfigErrors>__BaseCallProxy0())
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
					goto IL_100;
				case 3u:
					goto IL_144;
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
				IL_100:
				if (this.gerund.NullOrEmpty())
				{
					this.$current = this.defName + " lacks a gerund.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_144:
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
