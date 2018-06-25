using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF2 RID: 3314
	public class PawnRenderer
	{
		// Token: 0x0400317C RID: 12668
		private Pawn pawn;

		// Token: 0x0400317D RID: 12669
		public PawnGraphicSet graphics;

		// Token: 0x0400317E RID: 12670
		public PawnDownedWiggler wiggler;

		// Token: 0x0400317F RID: 12671
		private PawnHeadOverlays statusOverlays;

		// Token: 0x04003180 RID: 12672
		private PawnStatusEffecters effecters;

		// Token: 0x04003181 RID: 12673
		private PawnWoundDrawer woundOverlays;

		// Token: 0x04003182 RID: 12674
		private Graphic_Shadow shadowGraphic;

		// Token: 0x04003183 RID: 12675
		private const float CarriedThingDrawAngle = 16f;

		// Token: 0x04003184 RID: 12676
		private const float SubInterval = 0.00390625f;

		// Token: 0x04003185 RID: 12677
		private const float YOffset_PrimaryEquipmentUnder = 0f;

		// Token: 0x04003186 RID: 12678
		private const float YOffset_Behind = 0.00390625f;

		// Token: 0x04003187 RID: 12679
		private const float YOffset_Body = 0.0078125f;

		// Token: 0x04003188 RID: 12680
		private const float YOffsetInterval_Clothes = 0.00390625f;

		// Token: 0x04003189 RID: 12681
		private const float YOffset_Wounds = 0.01953125f;

		// Token: 0x0400318A RID: 12682
		private const float YOffset_Shell = 0.0234375f;

		// Token: 0x0400318B RID: 12683
		private const float YOffset_Head = 0.02734375f;

		// Token: 0x0400318C RID: 12684
		private const float YOffset_OnHead = 0.03125f;

		// Token: 0x0400318D RID: 12685
		private const float YOffset_PostHead = 0.03515625f;

		// Token: 0x0400318E RID: 12686
		private const float YOffset_CarriedThing = 0.0390625f;

		// Token: 0x0400318F RID: 12687
		private const float YOffset_PrimaryEquipmentOver = 0.0390625f;

		// Token: 0x04003190 RID: 12688
		private const float YOffset_Status = 0.04296875f;

		// Token: 0x060048F1 RID: 18673 RVA: 0x00264980 File Offset: 0x00262D80
		public PawnRenderer(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggler = new PawnDownedWiggler(pawn);
			this.statusOverlays = new PawnHeadOverlays(pawn);
			this.woundOverlays = new PawnWoundDrawer(pawn);
			this.graphics = new PawnGraphicSet(pawn);
			this.effecters = new PawnStatusEffecters(pawn);
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060048F2 RID: 18674 RVA: 0x002649D8 File Offset: 0x00262DD8
		private RotDrawMode CurRotDrawMode
		{
			get
			{
				RotDrawMode result;
				if (this.pawn.Dead && this.pawn.Corpse != null)
				{
					result = this.pawn.Corpse.CurRotDrawMode;
				}
				else
				{
					result = RotDrawMode.Fresh;
				}
				return result;
			}
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x00264A24 File Offset: 0x00262E24
		public void RenderPawnAt(Vector3 drawLoc)
		{
			this.RenderPawnAt(drawLoc, this.CurRotDrawMode, !this.pawn.health.hediffSet.HasHead);
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x00264A4C File Offset: 0x00262E4C
		public void RenderPawnAt(Vector3 drawLoc, RotDrawMode bodyDrawType, bool headStump)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				this.RenderPawnInternal(drawLoc, Quaternion.identity, true, bodyDrawType, headStump);
				if (this.pawn.carryTracker != null)
				{
					Thing carriedThing = this.pawn.carryTracker.CarriedThing;
					if (carriedThing != null)
					{
						Vector3 vector = drawLoc;
						bool flag = false;
						bool flip = false;
						if (this.pawn.CurJob == null || !this.pawn.jobs.curDriver.ModifyCarriedThingDrawPos(ref vector, ref flag, ref flip))
						{
							if (carriedThing is Pawn || carriedThing is Corpse)
							{
								vector += new Vector3(0.44f, 0f, 0f);
							}
							else
							{
								vector += new Vector3(0.18f, 0f, 0.05f);
							}
						}
						if (flag)
						{
							vector.y -= 0.0390625f;
						}
						else
						{
							vector.y += 0.0390625f;
						}
						carriedThing.DrawAt(vector, flip);
					}
				}
				if (this.pawn.def.race.specialShadowData != null)
				{
					if (this.shadowGraphic == null)
					{
						this.shadowGraphic = new Graphic_Shadow(this.pawn.def.race.specialShadowData);
					}
					this.shadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
				}
				if (this.graphics.nakedGraphic != null && this.graphics.nakedGraphic.ShadowGraphic != null)
				{
					this.graphics.nakedGraphic.ShadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
				}
			}
			else
			{
				Rot4 rot = this.LayingFacing();
				Building_Bed building_Bed = this.pawn.CurrentBed();
				bool renderBody;
				Quaternion quat;
				Vector3 rootLoc;
				if (building_Bed != null && this.pawn.RaceProps.Humanlike)
				{
					renderBody = building_Bed.def.building.bed_showSleeperBody;
					Rot4 rotation = building_Bed.Rotation;
					rotation.AsInt += 2;
					quat = rotation.AsQuat;
					AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)building_Bed.def.altitudeLayer, 15);
					Vector3 vector2 = this.pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
					Vector3 vector3 = vector2;
					vector3.y += 0.02734375f;
					float d = -this.BaseHeadOffsetAt(Rot4.South).z;
					Vector3 a = rotation.FacingCell.ToVector3();
					rootLoc = vector2 + a * d;
					rootLoc.y += 0.0078125f;
				}
				else
				{
					renderBody = true;
					rootLoc = drawLoc;
					if (!this.pawn.Dead && this.pawn.CarriedBy == null)
					{
						rootLoc.y = AltitudeLayer.LayingPawn.AltitudeFor() + 0.0078125f;
					}
					if (this.pawn.Downed || this.pawn.Dead)
					{
						quat = Quaternion.AngleAxis(this.wiggler.downedAngle, Vector3.up);
					}
					else if (this.pawn.RaceProps.Humanlike)
					{
						quat = rot.AsQuat;
					}
					else
					{
						Rot4 rot2 = Rot4.West;
						int num = this.pawn.thingIDNumber % 2;
						if (num != 0)
						{
							if (num == 1)
							{
								rot2 = Rot4.East;
							}
						}
						else
						{
							rot2 = Rot4.West;
						}
						quat = rot2.AsQuat;
					}
				}
				this.RenderPawnInternal(rootLoc, quat, renderBody, rot, rot, bodyDrawType, false, headStump);
			}
			if (this.pawn.Spawned && !this.pawn.Dead)
			{
				this.pawn.stances.StanceTrackerDraw();
				this.pawn.pather.PatherDraw();
			}
			this.DrawDebug();
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x00264E88 File Offset: 0x00263288
		public void RenderPortrait()
		{
			Vector3 zero = Vector3.zero;
			Quaternion quat;
			if (this.pawn.Dead || this.pawn.Downed)
			{
				quat = Quaternion.Euler(0f, 85f, 0f);
				zero.x -= 0.18f;
				zero.z -= 0.18f;
			}
			else
			{
				quat = Quaternion.identity;
			}
			this.RenderPawnInternal(zero, quat, true, Rot4.South, Rot4.South, this.CurRotDrawMode, true, !this.pawn.health.hediffSet.HasHead);
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x00264F38 File Offset: 0x00263338
		private void RenderPawnInternal(Vector3 rootLoc, Quaternion quat, bool renderBody, RotDrawMode draw, bool headStump)
		{
			this.RenderPawnInternal(rootLoc, quat, renderBody, this.pawn.Rotation, this.pawn.Rotation, draw, false, headStump);
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x00264F6C File Offset: 0x0026336C
		private void RenderPawnInternal(Vector3 rootLoc, Quaternion quat, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, bool portrait, bool headStump)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			Mesh mesh = null;
			if (renderBody)
			{
				Vector3 loc = rootLoc;
				loc.y += 0.0078125f;
				if (bodyDrawType == RotDrawMode.Dessicated && !this.pawn.RaceProps.Humanlike && this.graphics.dessicatedGraphic != null && !portrait)
				{
					this.graphics.dessicatedGraphic.Draw(loc, bodyFacing, this.pawn, 0f);
				}
				else
				{
					if (this.pawn.RaceProps.Humanlike)
					{
						mesh = MeshPool.humanlikeBodySet.MeshAt(bodyFacing);
					}
					else
					{
						mesh = this.graphics.nakedGraphic.MeshAt(bodyFacing);
					}
					List<Material> list = this.graphics.MatsBodyBaseAt(bodyFacing, bodyDrawType);
					for (int i = 0; i < list.Count; i++)
					{
						Material damagedMat = this.graphics.flasher.GetDamagedMat(list[i]);
						GenDraw.DrawMeshNowOrLater(mesh, loc, quat, damagedMat, portrait);
						loc.y += 0.00390625f;
					}
					if (bodyDrawType == RotDrawMode.Fresh)
					{
						Vector3 drawLoc = rootLoc;
						drawLoc.y += 0.01953125f;
						this.woundOverlays.RenderOverBody(drawLoc, mesh, quat, portrait);
					}
				}
			}
			Vector3 vector = rootLoc;
			Vector3 a = rootLoc;
			if (bodyFacing != Rot4.North)
			{
				a.y += 0.02734375f;
				vector.y += 0.0234375f;
			}
			else
			{
				a.y += 0.0234375f;
				vector.y += 0.02734375f;
			}
			if (this.graphics.headGraphic != null)
			{
				Vector3 b = quat * this.BaseHeadOffsetAt(headFacing);
				Material material = this.graphics.HeadMatAt(headFacing, bodyDrawType, headStump);
				if (material != null)
				{
					Mesh mesh2 = MeshPool.humanlikeHeadSet.MeshAt(headFacing);
					GenDraw.DrawMeshNowOrLater(mesh2, a + b, quat, material, portrait);
				}
				Vector3 loc2 = rootLoc + b;
				loc2.y += 0.03125f;
				bool flag = false;
				if (!portrait || !Prefs.HatsOnlyOnMap)
				{
					Mesh mesh3 = this.graphics.HairMeshSet.MeshAt(headFacing);
					List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
					for (int j = 0; j < apparelGraphics.Count; j++)
					{
						if (apparelGraphics[j].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
						{
							if (!apparelGraphics[j].sourceApparel.def.apparel.hatRenderedFrontOfFace)
							{
								flag = true;
								Material material2 = apparelGraphics[j].graphic.MatAt(bodyFacing, null);
								material2 = this.graphics.flasher.GetDamagedMat(material2);
								GenDraw.DrawMeshNowOrLater(mesh3, loc2, quat, material2, portrait);
							}
							else
							{
								Material material3 = apparelGraphics[j].graphic.MatAt(bodyFacing, null);
								material3 = this.graphics.flasher.GetDamagedMat(material3);
								Vector3 loc3 = rootLoc + b;
								loc3.y += ((!(bodyFacing == Rot4.North)) ? 0.03515625f : 0.00390625f);
								GenDraw.DrawMeshNowOrLater(mesh3, loc3, quat, material3, portrait);
							}
						}
					}
				}
				if (!flag && bodyDrawType != RotDrawMode.Dessicated && !headStump)
				{
					Mesh mesh4 = this.graphics.HairMeshSet.MeshAt(headFacing);
					Material mat = this.graphics.HairMatAt(headFacing);
					GenDraw.DrawMeshNowOrLater(mesh4, loc2, quat, mat, portrait);
				}
			}
			if (renderBody)
			{
				for (int k = 0; k < this.graphics.apparelGraphics.Count; k++)
				{
					ApparelGraphicRecord apparelGraphicRecord = this.graphics.apparelGraphics[k];
					if (apparelGraphicRecord.sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell)
					{
						Material material4 = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
						material4 = this.graphics.flasher.GetDamagedMat(material4);
						GenDraw.DrawMeshNowOrLater(mesh, vector, quat, material4, portrait);
					}
				}
			}
			if (!portrait && this.pawn.RaceProps.Animal && this.pawn.inventory != null && this.pawn.inventory.innerContainer.Count > 0 && this.graphics.packGraphic != null)
			{
				Graphics.DrawMesh(mesh, vector, quat, this.graphics.packGraphic.MatAt(bodyFacing, null), 0);
			}
			if (!portrait)
			{
				this.DrawEquipment(rootLoc);
				if (this.pawn.apparel != null)
				{
					List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
					for (int l = 0; l < wornApparel.Count; l++)
					{
						wornApparel[l].DrawWornExtras();
					}
				}
				Vector3 bodyLoc = rootLoc;
				bodyLoc.y += 0.04296875f;
				this.statusOverlays.RenderStatusOverlays(bodyLoc, quat, MeshPool.humanlikeHeadSet.MeshAt(headFacing));
			}
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0026551C File Offset: 0x0026391C
		private void DrawEquipment(Vector3 rootLoc)
		{
			if (!this.pawn.Dead && this.pawn.Spawned)
			{
				if (this.pawn.equipment != null && this.pawn.equipment.Primary != null)
				{
					if (this.pawn.CurJob == null || !this.pawn.CurJob.def.neverShowWeapon)
					{
						Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
						if (stance_Busy != null && !stance_Busy.neverAimWeapon && stance_Busy.focusTarg.IsValid)
						{
							Vector3 a;
							if (stance_Busy.focusTarg.HasThing)
							{
								a = stance_Busy.focusTarg.Thing.DrawPos;
							}
							else
							{
								a = stance_Busy.focusTarg.Cell.ToVector3Shifted();
							}
							float num = 0f;
							if ((a - this.pawn.DrawPos).MagnitudeHorizontalSquared() > 0.001f)
							{
								num = (a - this.pawn.DrawPos).AngleFlat();
							}
							Vector3 drawLoc = rootLoc + new Vector3(0f, 0f, 0.4f).RotatedBy(num);
							drawLoc.y += 0.0390625f;
							this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc, num);
						}
						else if (this.CarryWeaponOpenly())
						{
							if (this.pawn.Rotation == Rot4.South)
							{
								Vector3 drawLoc2 = rootLoc + new Vector3(0f, 0f, -0.22f);
								drawLoc2.y += 0.0390625f;
								this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc2, 143f);
							}
							else if (this.pawn.Rotation == Rot4.North)
							{
								Vector3 drawLoc3 = rootLoc + new Vector3(0f, 0f, -0.11f);
								drawLoc3.y = drawLoc3.y;
								this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc3, 143f);
							}
							else if (this.pawn.Rotation == Rot4.East)
							{
								Vector3 drawLoc4 = rootLoc + new Vector3(0.2f, 0f, -0.22f);
								drawLoc4.y += 0.0390625f;
								this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc4, 143f);
							}
							else if (this.pawn.Rotation == Rot4.West)
							{
								Vector3 drawLoc5 = rootLoc + new Vector3(-0.2f, 0f, -0.22f);
								drawLoc5.y += 0.0390625f;
								this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc5, 217f);
							}
						}
					}
				}
			}
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x00265864 File Offset: 0x00263C64
		public void DrawEquipmentAiming(Thing eq, Vector3 drawLoc, float aimAngle)
		{
			float num = aimAngle - 90f;
			Mesh mesh;
			if (aimAngle > 20f && aimAngle < 160f)
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			else if (aimAngle > 200f && aimAngle < 340f)
			{
				mesh = MeshPool.plane10Flip;
				num -= 180f;
				num -= eq.def.equippedAngleOffset;
			}
			else
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			num %= 360f;
			Graphic_StackCount graphic_StackCount = eq.Graphic as Graphic_StackCount;
			Material matSingle;
			if (graphic_StackCount != null)
			{
				matSingle = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle;
			}
			else
			{
				matSingle = eq.Graphic.MatSingle;
			}
			Graphics.DrawMesh(mesh, drawLoc, Quaternion.AngleAxis(num, Vector3.up), matSingle, 0);
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x00265950 File Offset: 0x00263D50
		private bool CarryWeaponOpenly()
		{
			bool result;
			if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
			{
				result = false;
			}
			else if (this.pawn.Drafted)
			{
				result = true;
			}
			else if (this.pawn.CurJob != null && this.pawn.CurJob.def.alwaysShowWeapon)
			{
				result = true;
			}
			else
			{
				if (this.pawn.mindState.duty != null)
				{
					if (this.pawn.mindState.duty.def.alwaysShowWeapon)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x00265A18 File Offset: 0x00263E18
		private Rot4 LayingFacing()
		{
			Rot4 result;
			if (this.pawn.GetPosture() == PawnPosture.LayingOnGroundFaceUp)
			{
				result = Rot4.South;
			}
			else
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					switch (this.pawn.thingIDNumber % 4)
					{
					case 0:
						return Rot4.South;
					case 1:
						return Rot4.South;
					case 2:
						return Rot4.East;
					case 3:
						return Rot4.West;
					}
				}
				else
				{
					switch (this.pawn.thingIDNumber % 4)
					{
					case 0:
						return Rot4.South;
					case 1:
						return Rot4.East;
					case 2:
						return Rot4.West;
					case 3:
						return Rot4.West;
					}
				}
				result = Rot4.Random;
			}
			return result;
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x00265B18 File Offset: 0x00263F18
		public Vector3 BaseHeadOffsetAt(Rot4 rotation)
		{
			Vector2 headOffset = this.pawn.story.bodyType.headOffset;
			Vector3 result;
			switch (rotation.AsInt)
			{
			case 0:
				result = new Vector3(0f, 0f, headOffset.y);
				break;
			case 1:
				result = new Vector3(headOffset.x, 0f, headOffset.y);
				break;
			case 2:
				result = new Vector3(0f, 0f, headOffset.y);
				break;
			case 3:
				result = new Vector3(-headOffset.x, 0f, headOffset.y);
				break;
			default:
				Log.Error("BaseHeadOffsetAt error in " + this.pawn, false);
				result = Vector3.zero;
				break;
			}
			return result;
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x00265BF6 File Offset: 0x00263FF6
		public void Notify_DamageApplied(DamageInfo dam)
		{
			this.graphics.flasher.Notify_DamageApplied(dam);
			this.wiggler.Notify_DamageApplied(dam);
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x00265C16 File Offset: 0x00264016
		public void RendererTick()
		{
			this.wiggler.WigglerTick();
			this.effecters.EffectersTick();
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x00265C30 File Offset: 0x00264030
		private void DrawDebug()
		{
			if (DebugViewSettings.drawDuties && Find.Selector.IsSelected(this.pawn) && this.pawn.mindState != null && this.pawn.mindState.duty != null)
			{
				this.pawn.mindState.duty.DrawDebug(this.pawn);
			}
		}
	}
}
