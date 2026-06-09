// using UnityEngine;
// using UnityEngine.InputSystem;

// public class AnimateHandOnInput : MonoBehaviour
// {
//     public InputActionProperty triggerValue;
//     public InputActionProperty gripValue;

//     public Animator handAnimator;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         float trigger = triggerValue.action.ReadValue<float>();
//         float grip = gripValue.action.ReadValue<float>();

//         handAnimator.SetFloat("Trigger", trigger);
//         handAnimator.SetFloat("Grip", grip);
//     }
// }
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    [Header("Input Actions (XR Controller)")]
    [SerializeField] private InputActionProperty triggerValue;
    [SerializeField] private InputActionProperty gripValue;

    [Header("Animator")]
    [SerializeField] private Animator handAnimator;

    [Header("Animator Parameter Names")]
    [SerializeField] private string triggerParam = "Trigger";
    [SerializeField] private string gripParam = "Grip";

    [Header("Realism Tuning")]
    [Tooltip("입력 값의 미세한 떨림을 무시할 구간(0~1). 예: 0.02")]
    [Range(0f, 0.2f)]
    [SerializeField] private float deadZone = 0.02f;

    [Tooltip("손가락이 따라오는 속도. 값이 클수록 더 빨리 따라옴")]
    [Range(1f, 40f)]
    [SerializeField] private float followSpeed = 18f;

    [Tooltip("Trigger를 더 민감/둔감하게 만들기 위한 커브")]
    [SerializeField] private AnimationCurve triggerResponse = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("Grip을 더 민감/둔감하게 만들기 위한 커브")]
    [SerializeField] private AnimationCurve gripResponse = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("Grip이 일정 이상일 때만 Trigger가 의미있게 적용되도록(총 잡을 때 현실감)")]
    [Range(0f, 1f)]
    [SerializeField] private float triggerRequiresGrip = 0.15f;

    [Tooltip("Grip이 약할 때 Trigger 영향력을 얼마나 줄일지 (0=완전 차단, 1=그대로)")]
    [Range(0f, 1f)]
    [SerializeField] private float triggerWhileLooseGrip = 0.25f;

    // internal
    private int _triggerHash;
    private int _gripHash;

    private float _targetTrigger;
    private float _targetGrip;

    private float _currentTrigger;
    private float _currentGrip;

    private void Awake()
    {
        if (handAnimator == null)
            handAnimator = GetComponent<Animator>();

        _triggerHash = Animator.StringToHash(triggerParam);
        _gripHash = Animator.StringToHash(gripParam);
    }

    private void OnEnable()
    {
        // InputActionProperty는 Enable을 명시적으로 해주는 게 안전하다(프로젝트마다 설정 차이로 입력이 튀는 경우 방지)
        if (triggerValue.action != null) triggerValue.action.Enable();
        if (gripValue.action != null) gripValue.action.Enable();
    }

    private void OnDisable()
    {
        if (triggerValue.action != null) triggerValue.action.Disable();
        if (gripValue.action != null) gripValue.action.Disable();
    }

    // VR 손 애니메이션은 보통 LateUpdate가 더 안정적(컨트롤러/리그 업데이트 후)
    private void LateUpdate()
    {
        if (handAnimator == null) return;

        float rawTrigger = Read01(triggerValue);
        float rawGrip = Read01(gripValue);

        // 1) 데드존 처리 (미세 떨림 제거)
        rawTrigger = ApplyDeadZone(rawTrigger, deadZone);
        rawGrip = ApplyDeadZone(rawGrip, deadZone);

        // 2) “손가락 감각” 커브 적용 (초반은 둔하게, 후반은 빠르게 등 조절)
        _targetTrigger = Mathf.Clamp01(triggerResponse.Evaluate(rawTrigger));
        _targetGrip = Mathf.Clamp01(gripResponse.Evaluate(rawGrip));

        // 3) 현실감 규칙: grip이 거의 안 쥐어졌으면 trigger는 덜 먹게
        if (_targetGrip < triggerRequiresGrip)
        {
            _targetTrigger *= triggerWhileLooseGrip;
        }

        // 4) 댐핑(부드럽게 따라가기)
        // followSpeed를 지수감쇠 형태로 적용: 프레임레이트 변화에도 감각이 일정해짐
        float t = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
        _currentGrip = Mathf.Lerp(_currentGrip, _targetGrip, t);
        _currentTrigger = Mathf.Lerp(_currentTrigger, _targetTrigger, t);

        // 5) Animator에 반영 (파라미터가 없다면 Animator 창에서 이름 확인 필요)
        handAnimator.SetFloat(_gripHash, _currentGrip);
        handAnimator.SetFloat(_triggerHash, _currentTrigger);
    }

    private static float Read01(InputActionProperty prop)
    {
        if (prop.action == null) return 0f;

        // 대부분 XR 입력은 float 0~1이지만, 프로젝트에 따라 튈 수 있으니 clamp
        float v = 0f;
        try
        {
            v = prop.action.ReadValue<float>();
        }
        catch
        {
            // 타입이 다르면 여기로 올 수 있음. 그 경우 InputAction 설정을 확인해야 함.
            v = 0f;
        }

        return Mathf.Clamp01(v);
    }

    private static float ApplyDeadZone(float v, float dz)
    {
        if (v <= dz) return 0f;
        // 데드존 이후 구간을 0~1로 다시 매핑
        return Mathf.InverseLerp(dz, 1f, v);
    }
}