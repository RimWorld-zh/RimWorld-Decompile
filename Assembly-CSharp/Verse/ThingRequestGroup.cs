namespace Verse
{
	public enum ThingRequestGroup : byte
	{
		Undefined = 0,
		Nothing = 1,
		Everything = 2,
		HaulableEver = 3,
		HaulableAlways = 4,
		Plant = 5,
		FoodSource = 6,
		FoodSourceNotPlantOrTree = 7,
		Corpse = 8,
		Blueprint = 9,
		BuildingArtificial = 10,
		BuildingFrame = 11,
		Pawn = 12,
		PotentialBillGiver = 13,
		Medicine = 14,
		Filth = 15,
		AttackTarget = 16,
		Weapon = 17,
		Refuelable = 18,
		HaulableEverOrMinifiable = 19,
		Drug = 20,
		Construction = 21,
		HasGUIOverlay = 22,
		Apparel = 23,
		MinifiedThing = 24,
		Grave = 25,
		Art = 26,
		ThisOrAnyCompIsThingHolder = 27,
		ActiveDropPod = 28,
		Transporter = 29,
		LongRangeMineralScanner = 30
	}
}
