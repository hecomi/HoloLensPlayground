using UnityEngine;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity.InputModule;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(HandDraggable))]
public class WorldAnchorAttacher : MonoBehaviour 
{
	void OnEnable() 
	{
		var draggable = GetComponent<HandDraggable>();
        if (!draggable) return;
        draggable.StartedDragging += OnStartDragging;
        draggable.StoppedDragging += OnStopDragging;
	}

    void OnDisable()
    {
		var draggable = GetComponent<HandDraggable>();
        if (!draggable) return;
        draggable.StartedDragging -= OnStartDragging;
        draggable.StoppedDragging -= OnStopDragging;
    }

    void OnStartDragging()
    {
        var anchor = GetComponent<WorldAnchor>();
        if (anchor) {
            DestroyImmediate(anchor);
        }
    }

    void OnStopDragging()
    {
        var anchor = GetComponent<WorldAnchor>();
        if (!anchor) {
            gameObject.AddComponent<WorldAnchor>();
        }
    }
}

}