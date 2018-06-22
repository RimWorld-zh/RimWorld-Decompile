using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000572 RID: 1394
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x06001A7F RID: 6783 RVA: 0x000E4CB4 File Offset: 0x000E30B4
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x000E4CC4 File Offset: 0x000E30C4
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001A81 RID: 6785 RVA: 0x000E4CEC File Offset: 0x000E30EC
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001A82 RID: 6786 RVA: 0x000E4D14 File Offset: 0x000E3114
		// (set) Token: 0x06001A83 RID: 6787 RVA: 0x000E4D34 File Offset: 0x000E3134
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001A84 RID: 6788 RVA: 0x000E4D44 File Offset: 0x000E3144
		// (set) Token: 0x06001A85 RID: 6789 RVA: 0x000E4D64 File Offset: 0x000E3164
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (set) Token: 0x06001A86 RID: 6790 RVA: 0x000E4D73 File Offset: 0x000E3173
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x000E4D90 File Offset: 0x000E3190
		// (set) Token: 0x06001A88 RID: 6792 RVA: 0x000E4DB5 File Offset: 0x000E31B5
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000E4DCC File Offset: 0x000E31CC
		// (set) Token: 0x06001A8A RID: 6794 RVA: 0x000E4DF1 File Offset: 0x000E31F1
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000E4E05 File Offset: 0x000E3205
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000E4E19 File Offset: 0x000E3219
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000E4E2C File Offset: 0x000E322C
		public override void Init()
		{
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = WorldMaterials.FeatureNameRenderQueue;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000E4EFB File Offset: 0x000E32FB
		public override void WrapAroundPlanetSurface()
		{
		}

		// Token: 0x04000F73 RID: 3955
		private TextMesh textMesh;

		// Token: 0x04000F74 RID: 3956
		private const float TextScale = 0.23f;

		// Token: 0x04000F75 RID: 3957
		private const int MinFontSize = 13;

		// Token: 0x04000F76 RID: 3958
		private const int MaxFontSize = 40;

		// Token: 0x04000F77 RID: 3959
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
