using System;

namespace Verse
{
	public class PawnCapacityDef : Def
	{
		public int listOrder = 0;

		public Type workerClass = typeof(PawnCapacityWorker);

		[MustTranslate]
		public string labelMechanoids = "";

		[MustTranslate]
		public string labelAnimals = "";

		public bool showOnHumanlikes = true;

		public bool showOnAnimals = true;

		public bool showOnMechanoids = true;

		public bool lethalFlesh = false;

		public bool lethalMechanoids = false;

		public float minForCapable = 0f;

		public float minValue = 0f;

		public bool zeroIfCannotBeAwake = false;

		public bool showOnCaravanHealthTab = false;

		[Unsaved]
		private PawnCapacityWorker workerInt;

		public PawnCapacityDef()
		{
		}

		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		public string GetLabelFor(bool isFlesh, bool isHumanlike)
		{
			string label;
			if (isHumanlike)
			{
				label = this.label;
			}
			else if (isFlesh)
			{
				if (!this.labelAnimals.NullOrEmpty())
				{
					label = this.labelAnimals;
				}
				else
				{
					label = this.label;
				}
			}
			else if (!this.labelMechanoids.NullOrEmpty())
			{
				label = this.labelMechanoids;
			}
			else
			{
				label = this.label;
			}
			return label;
		}
	}
}
