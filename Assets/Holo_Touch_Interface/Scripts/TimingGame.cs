using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(TouchOscUI2))]
public class TimingGame : MonoBehaviour 
{
    [SerializeField]
    GameObject notePrefab;

    [SerializeField]
    float interval = 0.5f;

    [SerializeField]
    float height = 1f;

    [SerializeField]
    float speed = 0.1f;

    TouchOscUI2 ui_;

    List<int[]> score_ = new List<int[]>();
    float timer_ = 0f;
    int scoreIndex_ = 0;

    List<Color> colors_ = new List<Color>() {
        Color.green,
        Color.magenta,
        Color.cyan,
        Color.yellow,
    };

    void Add(params int[] note) 
    {
        score_.Add(note);
    }

    void InitScore()
    {
        Add(6);
        Add(7);
        Add(10);
        Add(11);

        Add(2, 14);
        Add(3, 15);
        Add(1, 13);
        Add(4, 16);

        Add(8, 12);
        Add(7, 11);
        Add(6, 10);
        Add(5, 9);

        Add(1, 4, 13, 16);
        Add(2, 3, 14, 15);
        Add(5, 8, 9, 12);
        Add(6, 7, 10, 11);
    }

	void Start() 
	{
        ui_ = GetComponent<TouchOscUI2>();
        InitScore();
	}

    void Update()
    {
        timer_ += Time.deltaTime;
        if (timer_ > interval) {
            timer_ -= interval;
            GenerateNotes();
        }
    }

    void GenerateNotes()
    {
        scoreIndex_ = scoreIndex_ % score_.Count;
        var notes = score_[scoreIndex_];
        for (int i = 0; i < notes.Length; ++i) {
            var note = GenerateNote(notes[i]);
            note.color = colors_[scoreIndex_ % colors_.Count];
        }
        ++scoreIndex_;
    }

    TimingNote GenerateNote(int index)
    {
        var push = ui_.GetPush(index);
        if (!push) return null;

        var child = push.transform.GetChild(0);
        if (!child) return null;

        var meshFilter = child.GetComponent<MeshFilter>();
        if (!meshFilter) return null;

        var ex = meshFilter.sharedMesh.bounds.extents * 2;
        var s = transform.localScale;
        var scale = new Vector3(s.x * ex.x, s.y * ex.y, s.z * ex.z);

        var obj = Instantiate(notePrefab, push.transform);
        obj.transform.localPosition = new Vector3(0f, height, 0f);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = scale;
        obj.transform.SetParent(push.transform.parent);

        var note = obj.GetComponent<TimingNote>();
        Assert.IsNotNull(note);

        note.from = obj.transform.localPosition;
        note.to = new Vector3(note.from.x, 0f, note.from.z);
        note.successColor = Color.green;
        note.failColor = Color.red;
        note.speed = speed;
        note.push = push;

        return note;
    }
}

}