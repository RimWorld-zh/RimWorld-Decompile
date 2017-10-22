using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class CompEquippable : ThingComp, IVerbOwner
	{
		public VerbTracker verbTracker = null;

		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
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
				return base.parent.def.Verbs;
			}
		}

		public List<Tool> Tools
		{
			get
			{
				return base.parent.def.tools;
			}
		}

		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == base.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[1]
			{
				this
			});
		}

		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		public void Notify_EquipmentLost()
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		public string UniqueVerbOwnerID()
		{
			return base.parent.ThingID;
		}
	}
}
