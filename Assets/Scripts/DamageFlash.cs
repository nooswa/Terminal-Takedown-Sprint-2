using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    // color to flash when hit (white by default)
    [SerializeField] private Color _flashColor = Color.white;

    // how long the flash lasts
    [SerializeField] private float _flashTime = 0.25f;

    // stores all sprite renderers on this object and its children
    private SpriteRenderer[] _spriteRenderers;

    // stores the materials from the sprite renderers
    private Material[] _materials;

    // used to track the running coroutine
    private Coroutine _damageFlashCoroutine;

    // runs when the object starts up
    private void Awake()
    {
        // get all sprite renderers in children
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    // sets up the materials array
    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        // assign sprite renderer materials to _materials
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;

            

            // resets the flash amount to 0
            _materials[i].SetFloat("_FlashAmount", 0f);
        }
    }

    // call this to start the flash effect
    public void CallDamageFlash()
    {
        // Stop any ongoing flash coroutine to prevent overlapping effects
        if (_damageFlashCoroutine != null)
        {
            StopCoroutine(_damageFlashCoroutine);
        }

        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    // handles the flash effect over time
    private IEnumerator DamageFlasher()
    {
        // set the color
        SetFlashColor();

        // lerp the flash amount
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            // iterate elapsed time
            elapsedTime += Time.deltaTime;

            // lerp the flash amount
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }

        // ensure flash amount is reset to 0 at the end
        SetFlashAmount(0f);
        _damageFlashCoroutine = null;
    }

    private void SetFlashColor()
    {
        // set the color
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    // apply the flash color to all materials
    private void SetFlashAmount(float amount)
    {
        // set the flash amount
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}