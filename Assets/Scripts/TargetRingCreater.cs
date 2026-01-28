using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetRingCreater : MonoBehaviour
{
    [SerializeField] private GameObject targetRingPrefab; 
    public GameObject targetRingInstance;

    [Header("FadeInOutAnimation")]
    private Material _material; 
    private Color _originalColor;

    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Script")]
    [SerializeField] private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        if (targetRingPrefab != null)
        {
            targetRingInstance = Instantiate(targetRingPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f));
            targetRingInstance.transform.SetParent(transform);

            targetRingInstance.transform.position = new Vector3(transform.position.x, -1f, transform.position.z);

            AdjustTargetRingSize();

            targetRingInstance.SetActive(false);

            DotweenFadedata();
        }
    }

    public void getSelect(bool getselect)
    {
        if(getselect)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }

    }

    private void DotweenFadedata()
    {
        _material = targetRingInstance.GetComponent<Renderer>().material;
        _originalColor = _material.color;
        Color color = _material.color;
        color.a = 0;
        _material.color = color;
    }

    public void FadeIn()
    {
        enemy.Selected = true;
        targetRingInstance.SetActive(true);
        _material.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
    }

    public void FadeOut()
    {
        enemy.Selected = false;

        _material.DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            targetRingInstance.SetActive(false);
        });
    }

    private void AdjustTargetRingSize()
    {
        if (targetRingInstance == null) return;

        Renderer modelRenderer = GetComponentInChildren<Renderer>();
        if (modelRenderer != null)
        {
            Bounds modelBounds = modelRenderer.bounds;

            // Calculate the largest dimension (width or depth) to scale the ring
            float largestDimension = Mathf.Max(modelBounds.size.x, modelBounds.size.z);

            // Adjust the ring's scale based on the largest dimension
            targetRingInstance.transform.localScale = new Vector3(largestDimension, largestDimension, 1f);

            // Adjust the position to stay under the model
            targetRingInstance.transform.localPosition = new Vector3(0f, -0.7f, 0f); // Slightly above ground to avoid Z-fighting
        }
    }

}
