using Assets;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    private Vector3 _startingPosition;

    [SerializeField] private Vector3 _movementVector = new Vector3(10, 0, 0);
    [SerializeField] private float _period = 2f; // Time - in seconds - object takes to move back and forth for a complete cycle

    [Range(0, 1)]
    private float _movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        #region Manually
        //if (_movementFactor > 1f && _direction == Direction.Right)
        //{
        //    _direction = Direction.Left;
        //}

        //if (_movementFactor < 0f && _direction == Direction.Left)
        //{
        //    _direction = Direction.Right;
        //}

        //if (_direction == Direction.Right)
        //    _movementFactor = _movementFactor + 0.01f;
        //else _movementFactor = _movementFactor - 0.01f;

        //var offset = _movementVector * _movementFactor;
        //transform.position = _startingPosition + offset;
        #endregion

        if (_period == 0f)
        {
            return;
        }

        var cycles = Time.time / _period;
        var rawSignWave = Mathf.Sin(Constant.Math.Tau * cycles);
        _movementFactor = (rawSignWave / 2f) + 0.5f;
        var offset = _movementVector * _movementFactor;
        transform.position = _startingPosition + offset;
    }
}
