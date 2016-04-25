using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(ScoreManager))]
public class ScoreReset : Editor{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		ScoreManager myScript = (ScoreManager)target;
		if(GUILayout.Button("Reset High Scores"))
		{
			myScript.ResetHighScores ();
		}

	}

}

#endif

