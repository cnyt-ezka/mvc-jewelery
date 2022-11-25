using DG.Tweening;
using MVC.Base.Runtime.Abstract.View;
using MVC.Base.Runtime.Extensions;
using Runtime.Data.UnityObject;
using Runtime.Data.UnityObject.Joystick;
using Runtime.Data.ValueObject.Joystick;
using Runtime.Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Views.JoystickModule
{
    public class JoystickModuleView : MVCView
    {
	    public CD_Joystick JoystickConfig;
	    public JoystickID ID;
	    public RectTransform FillArea;
	    public RectTransform Handle;
	    [DisableInEditorMode][InfoBox("change values via JoystickSettings")]
	    public JoystickVO VO;

	    public event UnityAction JoystickStart;
	    public event UnityAction<JoystickParam> JoystickMove;
	    public event UnityAction JoystickStop;

	    [ShowInInspector][DisableInEditorMode]
	    private float _fillAreaSize = 350;

	    private bool _canMove;
	    private Vector2 _firstTouchPoint;
        private Vector2 _canvasSize;
        private float _canvasScaleFactor;
        private UnityEngine.Camera _camera;
        private RD_GameStatus _gameStatus;
        
        protected override void Awake()
        {
	        base.Awake();
	        VO = JoystickConfig.All[ID];
	        _camera = UnityEngine.Camera.main;
	        _canvasSize = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.size;
	        _canvasScaleFactor = 1 / GetComponentInParent<Canvas>().scaleFactor;
	        _fillAreaSize = FillArea.sizeDelta.x;
	        ResetPos(true);
        }

        public void Setup(RD_GameStatus gameStatus)
        {
	        _gameStatus = gameStatus;
        }
        
        private void Update()
        {
	        if (_gameStatus.Value.HasFlag(GameStatus.Blocked))
		        return;
	        if (Input.GetMouseButtonDown(0))
	        {
		        if (!CheckBounds()) return;

		        if (!VO.IsDelta && !VO.IsInvisible)
		        {
			        Handle.gameObject.SetActive(true);
			        FillArea.gameObject.SetActive(true);
		        }
		        
		        _firstTouchPoint = Input.mousePosition * _canvasScaleFactor;
		        
		        if (VO.SmoothStop)
		        {
			        FillArea.DOKill();
			        Handle.DOKill();
		        }
			    FillArea.anchoredPosition = _firstTouchPoint;
			    Handle.anchoredPosition = _firstTouchPoint;
		        
		        JoystickStart?.Invoke();
	        }
	        else if (_canMove && Input.GetMouseButton(0))
	        {
		        Move();
	        }
	        else if (Input.GetMouseButtonUp(0))
	        {
		        ResetPos();
		        JoystickStop?.Invoke();
	        }
        }

        private void Move()
        {
	        var inputPoint = (Vector2) Input.mousePosition * _canvasScaleFactor;
	        var diff = inputPoint - _firstTouchPoint;
	        var direction = diff.normalized;
	        var joystickMagnitude = Mathf.Clamp01(diff.magnitude / (_fillAreaSize * .5f));

	        if (!VO.IsDelta && Vector2.Distance(inputPoint, _firstTouchPoint) > _fillAreaSize * .5f)
	        {
		        if (VO.IsFixed)
		        {
			        inputPoint = _firstTouchPoint + direction * (_fillAreaSize * .5f);
		        }
		        else
		        {
			        _firstTouchPoint = inputPoint - direction * (_fillAreaSize * .5f);
			        
			        if(VO.SmoothStop)
						FillArea.DOKill();
			        
			        FillArea.anchoredPosition = _firstTouchPoint;
		        }
	        }
	        
	        if(VO.SmoothStop)
		        Handle.DOKill();
		    Handle.anchoredPosition = inputPoint;
	        
	        var joystickParam = new JoystickParam
	        {
		        Magnitude = joystickMagnitude,
		        DirectionVector2 = direction,
		        DirectionVector3 = direction.ToVector3OnXZ(),
		        DirectionQuaternion = (direction == Vector2.zero)
			        ? Quaternion.identity
			        : Quaternion.LookRotation(direction.ToVector3OnXZ(), Vector3.up)
	        };
	        if (VO.ConsiderCameraAngle)
	        {
		        joystickParam.DirectionQuaternion = (direction == Vector2.zero)
			        ? Quaternion.identity
			        : Quaternion.LookRotation(direction.ToVector3OnXZ(), Vector3.up) *
			          Quaternion.LookRotation(_camera.transform.forward.WithY(0), Vector3.up);
		        joystickParam.DirectionVector3 = joystickParam.DirectionQuaternion * Vector3.forward ;
		        joystickParam.DirectionVector2 = joystickParam.DirectionVector3.ToVector2ByXZ();
	        }

	        JoystickMove?.Invoke(joystickParam);
	        
	        if (VO.IsDelta)
		        _firstTouchPoint = inputPoint;
        }

        private void ResetPos(bool instant = false)
        {
	        if (VO.IsHidden || VO.IsDelta || VO.IsInvisible)
	        {
		        Handle.gameObject.SetActive(false);
		        FillArea.gameObject.SetActive(false);
	        }
	        Handle.DOKill();
	        FillArea.DOKill();
	        var duration = VO.SmoothStop ? VO.StopAnimDuration : 0;
	        var pos = new Vector2(VO.AnchorPosX * _canvasSize.x, VO.AnchorPosY * _canvasSize.y);
	        FillArea.DOAnchorPos(pos, instant ? 0 : duration);
	        Handle.DOAnchorPos(pos, instant ? 0 : duration);
        }

        private bool CheckBounds()
        {
	        if (!VO.Bounds)
	        {
		        _canMove = true;
		        return true;
	        }

	        if (Input.mousePosition.x * _canvasScaleFactor < _canvasSize.x * VO.HorizontalBounds.x ||
	            Input.mousePosition.x * _canvasScaleFactor > _canvasSize.x * VO.HorizontalBounds.y ||
				Input.mousePosition.y * _canvasScaleFactor < _canvasSize.y * VO.VerticalBounds.x ||
		        Input.mousePosition.y * _canvasScaleFactor > _canvasSize.y * VO.VerticalBounds.y)
	        {
		        _canMove = false;
		        return false;
	        }		        
	        _canMove = true;
	        return true;
        }
        public void Hide()
        {
	        ResetPos();
	        JoystickStop?.Invoke();
        }
    }
}
