using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIControllerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;  // Reference to the Button Prefab
    [SerializeField] private Transform parentTransform;
    private GameObject instantiatedButton;

    void Start()
    {
        // Check if the button has already been instantiated
        if (instantiatedButton == null)
        {
            // Instantiate the button prefab in the scene
            instantiatedButton = Instantiate(buttonPrefab, transform); 

            // Now, select the instantiated button using EventSystem
            StartCoroutine(SetInitialSelectedUI());
        }
    }

    IEnumerator SetInitialSelectedUI()
    {
        yield return new WaitForEndOfFrame();

        // Select the instantiated button in the UI
        EventSystem.current.SetSelectedGameObject(instantiatedButton);
    }
}

