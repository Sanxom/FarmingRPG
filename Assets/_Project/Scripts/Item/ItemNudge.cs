using System.Collections;
using UnityEngine;

public class ItemNudge : MonoBehaviour
{
    [SerializeField] private Transform _objectToRotate;
    [SerializeField] private Vector3 _rotationAmount = new(0f, 0f, 2f);
    [SerializeField] private float _pauseTime = 0.04f;
    [SerializeField] private int _firstRotation = 4;
    [SerializeField] private int _secondRotation = 5;

    private WaitForSeconds _pause;
    private bool _isAnimating;

    public Transform ObjectToRotate { get => _objectToRotate; set => _objectToRotate = value; }

    private void Awake()
    {
        _pause = new WaitForSeconds(_pauseTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isAnimating) return;

        if (transform.position.x < collision.transform.position.x)
            StartCoroutine(RotationCoroutine(true));
        else
            StartCoroutine(RotationCoroutine(false));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isAnimating) return;

        if (transform.position.x > collision.transform.position.x)
            StartCoroutine(RotationCoroutine(true));
        else
            StartCoroutine(RotationCoroutine(false));
    }

    private IEnumerator RotationCoroutine(bool isClockwiseToStart)
    {
        _isAnimating = true;

        for (int i = 0; i < _firstRotation; i++)
        {
            if (isClockwiseToStart)
                ObjectToRotate.Rotate(_rotationAmount);
            else
                ObjectToRotate.Rotate(-_rotationAmount);

            yield return _pause;
        }

        for (int i = 0; i < _secondRotation; i++)
        {
            if (isClockwiseToStart)
                ObjectToRotate.Rotate(-_rotationAmount);
            else
                ObjectToRotate.Rotate(_rotationAmount);

            yield return _pause;
        }

        if (isClockwiseToStart)
            ObjectToRotate.Rotate(_rotationAmount);
        else
            ObjectToRotate.Rotate(-_rotationAmount);

        yield return _pause;
        _isAnimating = false;
    }
}