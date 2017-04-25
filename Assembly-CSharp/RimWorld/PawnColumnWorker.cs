using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class PawnColumnWorker
	{
		private const int DefaultCellHeight = 30;

		private const int DefaultHeaderHeight = 30;

		public PawnColumnDef def;

		public abstract void DoHeader(Rect rect, List<Pawn> pawns);

		public abstract void DoCell(Rect rect, Pawn pawn);

		public abstract int GetMinWidth(Pawn pawn);

		public virtual int GetMaxWidth(Pawn pawn)
		{
			return 2147483647;
		}

		public virtual int GetOptimalWidth(List<Pawn> pawns)
		{
			return this.GetMinWidth(pawns);
		}

		public abstract int GetMinHeaderWidth(List<Pawn> pawns);

		public virtual int GetHeight(Pawn pawn)
		{
			return 30;
		}

		public virtual int GetHeaderHeight(List<Pawn> pawns)
		{
			return 30;
		}

		public int GetMinWidth(List<Pawn> pawns)
		{
			int num = this.GetMinHeaderWidth(pawns);
			for (int i = 0; i < pawns.Count; i++)
			{
				num = Mathf.Max(num, this.GetMinWidth(pawns[i]));
			}
			return num;
		}

		public int GetMaxWidth(List<Pawn> pawns)
		{
			int num = 2147483647;
			for (int i = 0; i < pawns.Count; i++)
			{
				num = Mathf.Min(num, this.GetMaxWidth(pawns[i]));
			}
			return num;
		}
	}
}
