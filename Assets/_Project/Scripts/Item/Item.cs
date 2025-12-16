using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer _sr;

    [field: SerializeField] public int ItemCode { get; set; }

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (ItemCode == 0) return;

        Init(ItemCode);
    }

    public void Init(int itemCode)
    {

    }
}