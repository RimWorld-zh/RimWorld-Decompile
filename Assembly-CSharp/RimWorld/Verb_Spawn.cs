using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DC RID: 2524
	public class Verb_Spawn : Verb
	{
		// Token: 0x06003883 RID: 14467 RVA: 0x001E334C File Offset: 0x001E174C
		protected override bool TryCastShot()
		{
			bool result;
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				result = false;
			}
			else
			{
				GenSpawn.Spawn(this.verbProps.spawnDef, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
				if (this.verbProps.colonyWideTaleDef != null)
				{
					Pawn pawn = this.caster.Map.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
					TaleRecorder.RecordTale(this.verbProps.colonyWideTaleDef, new object[]
					{
						this.caster,
						pawn
					});
				}
				if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
				{
					this.ownerEquipment.Destroy(DestroyMode.Vanish);
				}
				result = true;
			}
			return result;
		}
	}
}
