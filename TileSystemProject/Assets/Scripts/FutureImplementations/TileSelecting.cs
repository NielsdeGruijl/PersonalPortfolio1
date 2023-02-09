using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelecting : MonoBehaviour
{
    [SerializeField] private GameObject tileDropdown;
    [SerializeField] private List<GameObject> dropdownButtons;
    [SerializeField] private List<GameObject> instantiatedButtons;

    private float yOffset = 0;

    private void Start()
    {
        
    }

    public void ActivateDropdown(Vector3 mousePos)
    {
        if(instantiatedButtons.Count != dropdownButtons.Count)
        {
            foreach (GameObject button in dropdownButtons)
            {
                if (!instantiatedButtons.Contains(button))
                {
                    GameObject buttonObj = Instantiate(button, tileDropdown.transform);
                    instantiatedButtons.Add(buttonObj);

                    RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
                    Vector3 offset = new Vector3(0, yOffset, 0);
                    buttonObj.transform.localPosition = offset;
                    yOffset -= rectTransform.rect.height;
                }
            }
        }

        Vector3 dropdownOffset = new Vector3(1, 1, 0);
        tileDropdown.transform.position = mousePos + dropdownOffset;
        tileDropdown.SetActive(true);
    }

    public void DeactivateDropdown()
    {
        tileDropdown.SetActive(false);
    }
}
