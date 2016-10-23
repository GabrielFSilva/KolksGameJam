using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour 
{
	public event Action<Orientation>	OnSwipe;
    public event Action<Orientation>    OnClick;
    public BoxCollider2D	inputCollider;

	public Player	player;

	private Vector2 _firstPressPos;
	private Vector2 _secondPressPos;
	private Vector2 _currentSwipe;

	void Update()
	{
		if(Input.touches.Length > 0)
		{
			Touch __touch = Input.GetTouch(0);
			if(__touch.phase == TouchPhase.Began)
			{
				_firstPressPos = new Vector2(__touch.position.x,__touch.position.y);
			}
			else if(__touch.phase == TouchPhase.Ended)
			{
				_secondPressPos = new Vector2(__touch.position.x,__touch.position.y);
				_currentSwipe = new Vector3(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);

				if (_currentSwipe.magnitude < 100f)
                {
                    Vector3 __pos = __touch.position;
                    __pos.z = player.transform.position.z;
                    float __angle = AngleBetweenVector2(player.transform.position, Camera.main.ScreenToWorldPoint(__pos));
                    if (__angle > -45f && __angle <= 45f)
                        OnClick(Orientation.RIGHT);
                    else if (__angle > 45f && __angle <= 135f)
                        OnClick(Orientation.UP);
                    else if (__angle > 135f || __angle <= -135f)
                        OnClick(Orientation.LEFT);
                    else /*if (__angle > -135f && __angle <= -45f)*/
                        OnClick(Orientation.DOWN);
                    return;
                }

				_currentSwipe.Normalize();

                if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    OnSwipe(Orientation.UP);
				else if(_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
					OnSwipe (Orientation.DOWN);
                else if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
					OnSwipe (Orientation.LEFT);
                else /*if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)*/
					OnSwipe (Orientation.RIGHT);
			}
		}
	}

	private float AngleBetweenVector2(Vector2 p_vec1, Vector2 p_vec2)
	{
		Vector2 __difference = p_vec2 - p_vec1;
		float __sign = (p_vec2.y < p_vec1.y) ? -1f : 1f;
		return Vector2.Angle (Vector2.right, __difference) * __sign;
	}
}

