using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnColumnDef : Def
	{
		private Type workerClass = typeof(PawnColumnWorker);

		public float order;

		public List<string> tables = new List<string>();

		public int widthPriority;

		[Unsaved]
		private PawnColumnWorker workerInt;

		public PawnColumnWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnColumnWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
