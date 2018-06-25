using System;
using Verse;

namespace RimWorld
{
	public class PawnsArrivalModeDef : Def
	{
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		public SimpleCurve selectionWeightCurve;

		public float pointsFactor = 1f;

		public TechLevel minTechLevel = TechLevel.Undefined;

		public bool forQuickMilitaryAid;

		[MustTranslate]
		public string textEnemy;

		[MustTranslate]
		public string textFriendly;

		[Unsaved]
		private PawnsArrivalModeWorker workerInt;

		public PawnsArrivalModeDef()
		{
		}

		public PawnsArrivalModeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnsArrivalModeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
