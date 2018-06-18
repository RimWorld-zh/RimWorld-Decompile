using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C1 RID: 2241
	public class Instruction_DownRaider : Lesson_Instruction
	{
		// Token: 0x06003339 RID: 13113 RVA: 0x001B85AA File Offset: 0x001B69AA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.coverCells, "coverCells", LookMode.Undefined, new object[0]);
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x001B85CC File Offset: 0x001B69CC
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

		// Token: 0x0600333B RID: 13115 RVA: 0x001B86E8 File Offset: 0x001B6AE8
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

		// Token: 0x0600333C RID: 13116 RVA: 0x001B8770 File Offset: 0x001B6B70
		public override void LessonOnGUI()
		{
			if (!this.AllColonistsInCover())
			{
				TutorUtility.DrawCellRectOnGUI(Find.TutorialState.sandbagsRect, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x001B87A0 File Offset: 0x001B6BA0
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

		// Token: 0x04001B93 RID: 7059
		private List<IntVec3> coverCells;
	}
}
