using UnityEngine;

public class TriggerObscuringItemFader : MonoBehaviour
{
    private ObscuringItemFader[] _obscuringItemFaderArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _obscuringItemFaderArray = collision.GetComponentsInChildren<ObscuringItemFader>();

        if (_obscuringItemFaderArray.Length <= 0) return;

        for (int i = 0; i < _obscuringItemFaderArray.Length; i++)
        {
            _obscuringItemFaderArray[i].FadeOut();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _obscuringItemFaderArray = collision.GetComponentsInChildren<ObscuringItemFader>();

        if (_obscuringItemFaderArray.Length <= 0) return;

        for (int i = 0; i < _obscuringItemFaderArray.Length; i++)
        {
            _obscuringItemFaderArray[i].FadeIn();
        }
    }
}