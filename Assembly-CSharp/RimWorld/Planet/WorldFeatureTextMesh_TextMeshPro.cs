using System;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000577 RID: 1399
	[StaticConstructorOnStartup]
	public class WorldFeatureTextMesh_TextMeshPro : WorldFeatureTextMesh
	{
		// Token: 0x06001A99 RID: 6809 RVA: 0x000E4E6A File Offset: 0x000E326A
		private static void TextScale_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x000E4E78 File Offset: 0x000E3278
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x000E4EA0 File Offset: 0x000E32A0
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x000E4EC8 File Offset: 0x000E32C8
		// (set) Token: 0x06001A9D RID: 6813 RVA: 0x000E4EE8 File Offset: 0x000E32E8
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

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x000E4EF8 File Offset: 0x000E32F8
		// (set) Token: 0x06001A9F RID: 6815 RVA: 0x000E4F18 File Offset: 0x000E3318
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

		// Token: 0x170003D2 RID: 978
		// (set) Token: 0x06001AA0 RID: 6816 RVA: 0x000E4F27 File Offset: 0x000E3327
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = value * WorldFeatureTextMesh_TextMeshPro.TextScale;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x000E4F3C File Offset: 0x000E333C
		// (set) Token: 0x06001AA2 RID: 6818 RVA: 0x000E4F61 File Offset: 0x000E3361
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

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x000E4F78 File Offset: 0x000E3378
		// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x000E4F9D File Offset: 0x000E339D
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

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000E4FB1 File Offset: 0x000E33B1
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000E4FC5 File Offset: 0x000E33C5
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x000E4FD8 File Offset: 0x000E33D8
		public override void Init()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.GetComponent<TextMeshPro>();
			this.Color = new Color(1f, 1f, 1f, 0f);
			Material[] sharedMaterials = this.textMesh.GetComponent<MeshRenderer>().sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i].renderQueue = WorldMaterials.FeatureNameRenderQueue;
			}
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x000E5058 File Offset: 0x000E3458
		public override void WrapAroundPlanetSurface()
		{
			this.textMesh.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount != 0)
			{
				Vector3[] vertices = this.textMesh.mesh.vertices;
				float num = this.textMesh.bounds.extents.x * 2f;
				float num2 = Find.WorldGrid.DistOnSurfaceToAngle(num);
				Matrix4x4 localToWorldMatrix = this.textMesh.transform.localToWorldMatrix;
				Matrix4x4 worldToLocalMatrix = this.textMesh.transform.worldToLocalMatrix;
				for (int i = 0; i < characterCount; i++)
				{
					TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
					if (tmp_CharacterInfo.isVisible)
					{
						int vertexIndex = tmp_CharacterInfo.vertexIndex;
						Vector3 vector = vertices[vertexIndex] + vertices[vertexIndex + 1] + vertices[vertexIndex + 2] + vertices[vertexIndex + 3];
						vector /= 4f;
						float num3 = vector.x / (num / 2f);
						bool flag = num3 >= 0f;
						num3 = Mathf.Abs(num3);
						float num4 = num2 / 2f * num3;
						float num5 = (180f - num4) / 2f;
						float num6 = 200f * Mathf.Tan(num4 / 2f * 0.0174532924f);
						Vector3 vector2 = new Vector3(Mathf.Sin(num5 * 0.0174532924f) * num6 * ((!flag) ? -1f : 1f), vector.y, Mathf.Cos(num5 * 0.0174532924f) * num6);
						Vector3 b = vector2 - vector;
						Vector3 vector3 = vertices[vertexIndex] + b;
						Vector3 vector4 = vertices[vertexIndex + 1] + b;
						Vector3 vector5 = vertices[vertexIndex + 2] + b;
						Vector3 vector6 = vertices[vertexIndex + 3] + b;
						Quaternion rotation = Quaternion.Euler(0f, num4 * ((!flag) ? 1f : -1f), 0f);
						vector3 = rotation * (vector3 - vector2) + vector2;
						vector4 = rotation * (vector4 - vector2) + vector2;
						vector5 = rotation * (vector5 - vector2) + vector2;
						vector6 = rotation * (vector6 - vector2) + vector2;
						vector3 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector3).normalized * 100.2f);
						vector4 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector4).normalized * 100.2f);
						vector5 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector5).normalized * 100.2f);
						vector6 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector6).normalized * 100.2f);
						vertices[vertexIndex] = vector3;
						vertices[vertexIndex + 1] = vector4;
						vertices[vertexIndex + 2] = vector5;
						vertices[vertexIndex + 3] = vector6;
					}
				}
				this.textMesh.mesh.vertices = vertices;
			}
		}

		// Token: 0x04000F7B RID: 3963
		private TextMeshPro textMesh;

		// Token: 0x04000F7C RID: 3964
		public static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		// Token: 0x04000F7D RID: 3965
		[TweakValue("Interface.World", 0f, 5f)]
		private static float TextScale = 1f;
	}
}
