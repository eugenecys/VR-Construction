using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class Deployer : Singleton<Deployer>, Interactable {

    GameManager gameManager;
    public MeshRenderer meshRenderer;
    AssetManager assetManager;
    Rigidbody rb;
    public GameObject cell; 

    public void highlight()
    {
        meshRenderer.material = assetManager.highlightMaterial;
    }

    public void unhighlight()
    {
        meshRenderer.material = assetManager.freeMaterial;
    }

    public void deploy()
    {
        cell.SetActive(false);
        gameManager.play();
    }

    public void undeploy()
    {
        cell.SetActive(true);
        gameManager.build();
    }

    void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        assetManager = AssetManager.Instance;
        gameManager = GameManager.Instance;
        unhighlight();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
