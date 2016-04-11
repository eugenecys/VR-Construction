using UnityEngine;
using System.Collections;

public class MaterialHandler : MonoBehaviour {

    public Material[] defaultMaterials;
    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void loadMaterial(Material mat)
    {
        int matCount = defaultMaterials.Length;
        Material[] mats = new Material[matCount];
        for (int i = 0; i < matCount; i++)
        {
            mats[i] = mat;
        }
        loadMaterials(mats);
    }

    public void loadMaterials(Material[] mat)
    {
        meshRenderer.materials = mat;
    }

    public void loadDefault()
    {
        meshRenderer.materials = defaultMaterials;
    }

    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
