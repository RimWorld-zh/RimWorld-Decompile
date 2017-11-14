using System;

namespace Verse
{
	[Flags]
	public enum MapMeshFlag
	{
		None = 0,
		Things = 1,
		FogOfWar = 2,
		Buildings = 4,
		GroundGlow = 8,
		Terrain = 0x10,
		Roofs = 0x20,
		Snow = 0x40,
		Zone = 0x80,
		PowerGrid = 0x100,
		BuildingsDamage = 0x200
	}
}
