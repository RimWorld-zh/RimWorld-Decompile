using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Slaughter : Designator
	{
		private List<Pawn> justDesignated = new List<Pawn>();

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Slaughter()
		{
			this.defaultLabel = "DesignatorSlaughter".Translate();
			this.defaultDesc = "DesignatorSlaughterDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Slaughter", true);
			this.soundDragSustain = SoundDefOf.DesignateDragStandard;
			this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.DesignateHunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.SlaughterablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateSlaughterable".Translate();
			}
			return true;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn current in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(current);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.def.race.Animal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) == null && !pawn.InAggroMentalState)
			{
				return true;
			}
			return false;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Slaughter));
			this.justDesignated.Add((Pawn)t);
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		[DebuggerHidden]
		private IEnumerable<Pawn> SlaughterablesInCell(IntVec3 c)
		{
			Designator_Slaughter.<SlaughterablesInCell>c__Iterator191 <SlaughterablesInCell>c__Iterator = new Designator_Slaughter.<SlaughterablesInCell>c__Iterator191();
			<SlaughterablesInCell>c__Iterator.c = c;
			<SlaughterablesInCell>c__Iterator.<$>c = c;
			<SlaughterablesInCell>c__Iterator.<>f__this = this;
			Designator_Slaughter.<SlaughterablesInCell>c__Iterator191 expr_1C = <SlaughterablesInCell>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
