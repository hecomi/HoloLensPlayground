using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(TouchInterface))]
public class TouchOscServer : MonoBehaviour
{
    [SerializeField]
    int listenPort = 3333;

    [SerializeField]
    TouchInterface handler;

    UdpClient udpClient_;
    IPEndPoint endPoint_;
    Osc.Parser osc_ = new Osc.Parser();
    
    void Start()
    {
        endPoint_ = new IPEndPoint(IPAddress.Any, listenPort);
        udpClient_ = new UdpClient(endPoint_);
    }

    void Update()
    {
        while (udpClient_.Available > 0) {
            var data = udpClient_.Receive(ref endPoint_);
            osc_.FeedData(data);
        }

        while (osc_.MessageCount > 0) {
            var msg = osc_.PopMessage();
            handler.OnMessage(msg);
        }
    }
}

}