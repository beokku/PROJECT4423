using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour {

    [SerializeField] private Slider slider;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue) {
        slider.value = currentValue / maxValue;
    }

    void Start() {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null) {
            camera = cameraObject.GetComponent<Camera>();
        }
    }

    void Update() {
        transform.parent.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}
