using RimWorld;
using System;
using Verse.AI;

namespace Verse
{
	public class MentalBreakDef : Def
	{
		public Type workerClass = typeof(MentalBreakWorker);

		public MentalStateDef mentalState;

		public float baseCommonality;

		public MentalBreakIntensity intensity = MentalBreakIntensity.Extreme;

		public TraitDef requiredTrait;

		private MentalBreakWorker workerInt = null;

		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
