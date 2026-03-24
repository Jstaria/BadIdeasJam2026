using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class MyButton : MonoBehaviour
{
    [SerializeField] private float buttonTimeoutDuration = 1;

    [Header("Component References")]
    [SerializeField] private Transform button;
    [SerializeField] private Transform casing;
    [SerializeField] private SwapMaterial swapMaterial;

    [Space]
    [Header("Spring Var")]
    [SerializeField] private float angularFrequency;
    [SerializeField] private float dampingRatio;

    [Space]
    [Header("On Use Stretch")]
    [SerializeField] private float PressHeight;
    [SerializeField] private float PressWidth;
    [SerializeField] private float CasingWidth;

    private float defaultButtonHeight;
    private float defaultButtonWidth;
    private float defaultCasingWidth;

    private Dictionary<string, Spring> springs;
    private float pressTimer;

    public bool isDisabled;

    public UnityEvent OnButtonPress;

    private void Awake()
    {
        defaultButtonHeight = button.localScale.y;
        defaultButtonWidth = button.localScale.x;
        defaultCasingWidth = casing.localScale.x;

        Interactable interactable = GetComponent<Interactable>();
        interactable.OnInteract.AddListener(OnInteract);

        springs = new Dictionary<string, Spring>();

        springs.Add("ButtonHeight", new Spring(angularFrequency, dampingRatio, 1, true));
        springs.Add("ButtonStretch", new Spring(angularFrequency, dampingRatio, 1, true));
        springs.Add("CasingStretch", new Spring(angularFrequency, dampingRatio, 1, true));
    }

    private void Update()
    {
        foreach (Spring spring in springs.Values)
        {
            spring.Update();
            //spring.SetValues(angularFrequency, dampingRatio);

            if (pressTimer < 0)
                spring.RestPosition = 1;
        }

        Vector3 buttonTransform = button.transform.localScale;
        Vector3 casingTransform = casing.transform.localScale;

        buttonTransform.y = defaultButtonHeight * springs["ButtonHeight"].Position;
        buttonTransform.x = defaultButtonWidth * springs["ButtonStretch"].Position;
        buttonTransform.z = defaultButtonWidth * springs["ButtonStretch"].Position;
        casingTransform.x = defaultCasingWidth * springs["CasingStretch"].Position;
        casingTransform.z = defaultCasingWidth * springs["CasingStretch"].Position;

        button.transform.localScale = buttonTransform;
        casing.transform.localScale = casingTransform;

        pressTimer -= Time.deltaTime;
    }

    public void OnInteract()
    {
        pressTimer = .1f;
        springs["ButtonHeight"].RestPosition = PressHeight;
        springs["ButtonStretch"].RestPosition = PressWidth;
        springs["CasingStretch"].RestPosition = CasingWidth;

        if (isDisabled) return;

        StartCoroutine(OnButtonDisable());

        OnButtonPress?.Invoke();
    }

    private IEnumerator OnButtonDisable()
    {
        if (swapMaterial != null)
            swapMaterial.Switch(0);

        Disable();

        yield return new WaitForSeconds(buttonTimeoutDuration);
        
        swapMaterial.Switch(1);
        Enable();
    }

    public void Disable() => isDisabled = true;
    public void Enable() => isDisabled = false;
}
