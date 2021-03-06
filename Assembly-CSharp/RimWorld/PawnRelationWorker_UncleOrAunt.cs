﻿using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		public PawnRelationWorker_UncleOrAunt()
		{
		}

		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
