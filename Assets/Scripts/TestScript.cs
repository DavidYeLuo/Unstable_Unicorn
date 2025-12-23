using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TestScript : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public void AppendMessage(string message)
    {
        buttonText.text += message;
    }
}
