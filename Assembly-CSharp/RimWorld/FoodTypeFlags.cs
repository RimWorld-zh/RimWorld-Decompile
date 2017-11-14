using System;

namespace RimWorld
{
	[Flags]
	public enum FoodTypeFlags
	{
		None = 0,
		VegetableOrFruit = 1,
		Meat = 2,
		Fluid = 4,
		Corpse = 8,
		Seed = 0x10,
		AnimalProduct = 0x20,
		Plant = 0x40,
		Tree = 0x80,
		Meal = 0x100,
		Processed = 0x200,
		Liquor = 0x400,
		Kibble = 0x800,
		VegetarianAnimal = 3857,
		VegetarianRoughAnimal = 3921,
		CarnivoreAnimal = 2826,
		CarnivoreAnimalStrict = 10,
		OmnivoreAnimal = 3867,
		OmnivoreRoughAnimal = 3931,
		DendrovoreAnimal = 2705,
		OvivoreAnimal = 2848,
		OmnivoreHuman = 3903
	}
}
