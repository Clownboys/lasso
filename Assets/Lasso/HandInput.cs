using UnityEngine;
using UnityEngine.InputSystem;

public class HandInput : MonoBehaviour
{
    private Grabber ropeGrabber;
    private Router ropeRouter;

	[SerializeField] private InputAction grabAction;

	private void Awake()
	{
		ropeGrabber = GetComponent<Grabber>();
		ropeRouter = GetComponent<Router>();

		grabAction.performed += SetGrab;
	}

	private void SetGrab(InputAction.CallbackContext callbackContext)
	{
		UpdateInput();
	}

	private void Update()
	{
		UpdateInput();
	}

	private void UpdateInput()
	{
		bool pressed = grabAction.ReadValue<float>() > 0.5f;

		ropeGrabber.enabled = pressed;
		ropeRouter.enabled = !pressed;
	}

	private void OnEnable()
	{
		grabAction.Enable();
	}

	private void OnDisable()
	{
		grabAction.Disable();
	}

}
