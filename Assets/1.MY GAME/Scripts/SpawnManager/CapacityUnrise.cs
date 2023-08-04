using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CapacityUnrise : MonoBehaviour
{
    public TextMeshProUGUI day;
    private Color colorDay;
    private Color colorPanel;
    public Image panelDay;
    public bool check = false;

    void Start()
    {
        colorDay = Color.white;
        colorPanel = Color.black;
        StartCoroutine(ReriseCapacity());
        StartCoroutine(Deactive());
    }

    // Update is called once per frame
    void Update()
    {
        if(check)
        {
            colorDay.a -= Time.deltaTime * 1f;
            colorPanel.a -= Time.deltaTime * 1f;
            day.color = colorDay;
            panelDay.color = colorPanel;
        }
    }

    public IEnumerator ReriseCapacity()
    {
        yield return new WaitForSeconds(2);
        check = true;
    }
    public IEnumerator Deactive()
    {
        yield return new WaitForSeconds(3f);
        panelDay.gameObject.SetActive(false);
    }

}
