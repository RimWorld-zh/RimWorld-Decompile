using System;
using System.Collections.Generic;

namespace Verse
{
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		public VerbTracker verbTracker = null;

		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
			}
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		public string UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID();
		}
	}
}
