using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HoloToolkit.Unity.InputModule;

public class HueColorChanger : MonoBehaviour, IInputClickHandler 
{
    public string apiUrlTemplate = "{server}/hue/{id}/{api}/{color}";
    public string server = "http://192.168.1.2:3000";
    public int id = 0;
    public string api = "rgb";
    public Color32 color = Color.white;

    private string apiUrl
    {
        get 
        { 
            var colorStr = string.Format("{0},{1},{2}", color.r, color.g, color.b);
            return apiUrlTemplate
                .Replace("{server}", server)
                .Replace("{id}", id.ToString())
                .Replace("{api}", api)
                .Replace("{color}", colorStr);
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        StartCoroutine(CallApi());
    }

    IEnumerator CallApi()
    {
        var req = UnityWebRequest.Get(apiUrl);
        yield return req.Send();

        if (req.isError) {
            Debug.LogError(req.error);
        }
    }
}
