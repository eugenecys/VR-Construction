﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class Deployer : Singleton<Deployer>, Interactable {

    GameManager gameManager;
    public MeshRenderer meshRenderer;
    AssetManager assetManager;
    Rigidbody rb;
    public GameObject cell; 
    public Material defaultMaterial;

    public void highlight()
    {
        meshRenderer.material = assetManager.deployMaterial;
    }

    public void unhighlight()
    {
		if (defaultMaterial)
        	meshRenderer.material = defaultMaterial;
    }
    
    public void online()
    {

    }

    public void offline()
    {

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

	void OnTriggerStay(Collider other)
	{
	}

	void OnTriggerExit(Collider other)
	{
		
	}
}
