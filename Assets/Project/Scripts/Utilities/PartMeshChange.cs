using System;
using UnityEngine;

public static class PartMeshChange
{
	public static SkinnedMeshRenderer UpdateMeshRenderer(SkinnedMeshRenderer baseMeshRenderer, SkinnedMeshRenderer newMeshRenderer, Transform baseObject)
	{
		// update mesh
		baseMeshRenderer.sharedMesh = newMeshRenderer.sharedMesh;

		Transform[] childrens = baseObject.GetComponentsInChildren<Transform>(true);

		// sort bones.
		Transform[] bones = new Transform[newMeshRenderer.bones.Length];
		for (int boneOrder = 0; boneOrder < newMeshRenderer.bones.Length; boneOrder++)
		{
			bones[boneOrder] = Array.Find<Transform>(childrens, c => c.name == newMeshRenderer.bones[boneOrder].name);
		}
		baseMeshRenderer.bones = bones;

		return baseMeshRenderer;
	}
}
