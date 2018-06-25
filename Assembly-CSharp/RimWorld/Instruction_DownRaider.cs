using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BF RID: 2239
	public class Instruction_DownRaider : Lesson_Instruction
	{
		// Token: 0x04001B91 RID: 7057
		private List<IntVec3> coverCells;

		// Token: 0x06003336 RID: 13110 RVA: 0x001B88D2 File Offset: 0x001B6CD2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.coverCells, "coverCells", LookMode.Undefined, new object[0]);
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x001B88F4 File Offset: 0x001B6CF4
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = Find.TutorialState.sandbagsRect.ContractedBy(1);
			this.coverCells = new List<IntVec3>();
			foreach (IntVec3 item in cellRect.EdgeCells)
			{
				if (item.x != cellRect.CenterCell.x && item.z != cellRect.CenterCell.z)
				{
					this.coverCells.Add(item);
				}
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = base.Map;
			incidentParms.points = PawnKindDefOf.Drifter.combatPower;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			incidentParms.raidForceOneIncap = true;
			incidentParms.raidNeverFleeIndividual = true;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x001B8A10 File Offset: 0x001B6E10
		private bool AllColonistsInCover()
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				if (!this.coverCells.Contains(pawn.Position))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001B8A98 File Offset: 0x001B6E98
		public override void LessonOnGUI()
		{
			if (!this.AllColonistsInCover())
			{
				TutorUtility.DrawCellRectOnGUI(Find.TutorialState.sandbagsRect, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x001B8AC8 File Offset: 0x001B6EC8
		public override void LessonUpdate()
		{
			if (!this.AllColonistsInCover())
			{
				for (int i = 0; i < this.coverCells.Count; i++)
				{
					Vector3 position = this.coverCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
					Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
				}
			}
			IEnumerable<Pawn> source = base.Map.mapPawns.PawnsInFaction(Faction.OfPlayer);
			if (source.Any((Pawn p) => p.Downed))
			{
				foreach (Pawn pawn in base.Map.mapPawns.AllPawns)
				{
					if (pawn.HostileTo(Faction.OfPlayer))
					{
						HealthUtility.DamageUntilDowned(pawn);
					}
				}
			}
			if ((from p in base.Map.mapPawns.AllPawnsSpawned
			where p.HostileTo(Faction.OfPlayer) && !p.Downed
			select p).Count<Pawn>() == 0)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
