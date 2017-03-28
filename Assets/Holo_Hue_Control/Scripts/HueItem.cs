using UnityEngine;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(Animator))]
public class HueItem : MonoBehaviour, IInputClickHandler
{
    Animator animator_;
    bool selected_ = false;
    float previousEventTime = 0f;

    void Start()
    {
        animator_ = GetComponent<Animator>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (Time.time - previousEventTime < 0.2f) return;
        previousEventTime = Time.time;

        selected_ = !selected_;
        animator_.SetBool("selected", selected_);
    }
}
