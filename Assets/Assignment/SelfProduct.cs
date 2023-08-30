using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelfProduct : MonoBehaviour
{
    public static Action<ProductType, int> OnSingleProductPicked = delegate { };
    public Slider slider;
    public ProductType productType;

    /// <summary>
    /// Start picking object.
    /// </summary>
    public void StarPickingObject()
    {
        StopAllCoroutines();
        StartCoroutine(PickObject());
        slider.gameObject.SetActive(true);
    }

    /// <summary>
    /// Stop picking object.
    /// </summary>
    public void StopPicking()
    {
        StopAllCoroutines();
        ResetUI();
    }

    /// <summary>
    /// Reset UI.
    /// </summary>
    void ResetUI()
    {
        slider.value = 0;
        slider.gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine for Picking Object.
    /// </summary>
    /// <returns></returns>
    IEnumerator PickObject()
    {
        while (slider.value<=.9)
        {
            slider.value += .01f;
            yield return new WaitForSeconds(.05f);
        }

        slider.value = 1;
        yield return new WaitForSeconds(.1f);
        slider.value = 0;
        OnSingleProductPicked.Invoke(productType, 1);

        //limiting both product count to 5 at a time
        if (productType == ProductType.Red)
        {
            if (CharacterController.productRedCount < 5)
                StarPickingObject();
        }
        else if (productType == ProductType.Blue)
        {
            if (CharacterController.productBlueCount < 5)
                StarPickingObject();
        }
    }
}
