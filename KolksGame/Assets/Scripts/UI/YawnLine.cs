using UnityEngine;
using System;

public class YawnLine : MonoBehaviour
{
    public event Action<YawnLine> OnRemoveYawnLine;

    public SpriteRenderer spriteRenderer;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _distance;
    private Orientation _orientation;

    public float lineDuration;
    public float lineCompletionTime;
    public float fadeDuration;

    [SerializeField]
    private float _lineScale;
    [SerializeField]
    private int _dotCount;
    private float _lineTimer;
    private int _currentDots;

    void Start()
    {
        _lineTimer = 0f;
        _currentDots = -10;
    }
    
    public void SetPoints(Vector2 p_start, Vector2 p_end, Orientation p_ori)
    {
        _orientation = p_ori;
        if (p_ori == Orientation.RIGHT)
        {
            _startPosition = p_start + (Vector2.right * 0.4f) + (Vector2.down * 0.15f);
            _endPosition = p_end + (Vector2.left * 0.4f) + (Vector2.down * 0.15f);
            _dotCount = GetDotCount(p_start.x, p_end.x);
        }
        else if (p_ori == Orientation.LEFT)
        {
            _startPosition = p_start + (Vector2.left * 0.4f) + (Vector2.down * 0.15f);
            _endPosition = p_end + (Vector2.right * 0.4f) + (Vector2.down * 0.15f);
            _dotCount = GetDotCount(p_start.x, p_end.x);
        }
        else if (p_ori == Orientation.UP)
        {
            _startPosition = p_start + (Vector2.up * 0.4f);
            _endPosition = p_end + (Vector2.down * 0.4f);
            _dotCount = GetDotCount(p_start.y, p_end.y);
            transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (p_ori == Orientation.DOWN)
        {
            _startPosition = p_start + (Vector2.down * 0.4f);
            _endPosition = p_end + (Vector2.up * 0.4f);
            
            _dotCount = GetDotCount(p_start.y, p_end.y);
            transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }
        _lineScale = _dotCount * 0.4f;
        _distance = _endPosition - _startPosition;
    }

    private int GetDotCount(float p_value1, float p_value2)
    {
        return (((Mathf.Abs(Mathf.RoundToInt(p_value1 - p_value2)) / 2) - 1) * 5) + 2;
    }

    void Update ()
    {
        _lineTimer += Time.deltaTime;
        
        int __i = Mathf.RoundToInt(_lineTimer * (float)_dotCount / (lineCompletionTime));

        //Update Dots
        if (__i != _currentDots && __i <= _dotCount)
        {
            _currentDots = __i;
            transform.position = (_startPosition +
                                    (_startPosition + (_distance * ((float)__i / (float)_dotCount)))) / 2f;
            transform.localScale = Vector2.Lerp(new Vector2(0f, 0.5f), new Vector2(_lineScale, 0.5f),
                (float)__i / (float)_dotCount);
            spriteRenderer.material.SetFloat("RepeatX", __i);
        }
        //Fade
        if (_lineTimer > lineDuration - fadeDuration)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f,
                1f - ((_lineTimer - (lineDuration - fadeDuration)) / fadeDuration));
        }
        //Remove Line
        if (_lineTimer > lineDuration)
            OnRemoveYawnLine(this);
    }
}
