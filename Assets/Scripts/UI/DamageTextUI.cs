using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private float _destroyDelayTime = 3f;
    [SerializeField] private Vector3 _randomVector = new Vector3(0.5f, 0f, 0f);

    private void Start()
    {
        transform.localPosition += 
            new Vector3(Random.Range(-_randomVector.x, _randomVector.x),
                        Random.Range(-_randomVector.y, _randomVector.y),
                        Random.Range(-_randomVector.z, _randomVector.z));

        Destroy(gameObject, _destroyDelayTime);
    }

    public void SetDamageText(float value)
    {
        _damageText.text = value.ToString();
    }
}
