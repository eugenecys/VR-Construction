using UnityEngine;
using System.Collections;

public class BuildingGenerator : MonoBehaviour {
    public Texture2D map;
    public GameObject building;
    public Transform room;
	// Use this for initialization
	void Start () {
        Generator();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void Generator() {
        for (int i = 0; i < 64; ++i) {
            for (int j = 0; j < 64; ++j) {
                Color c = map.GetPixel(i, j);
                if (c == Color.red) {
                    Instantiate(building, room.position + new Vector3((j - 32) * 0.015f, 0.01f, (32 - i) * 0.015f), Quaternion.identity);
                }
            }
        }
    }
}
