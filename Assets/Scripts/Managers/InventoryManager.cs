using UnityEngine;
using UnityEngine.UI; // Required for Image
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Counts")]
    public int schizophreniaCount = 0;
    public int insomniaCount = 0;
    public int lactoseCount = 0;

    [Header("UI Text References")]
    public TextMeshProUGUI schizoText;
    public TextMeshProUGUI insomniaText;
    public TextMeshProUGUI lactoseText;

    [Header("UI Image References (Drag the Icons here)")]
    public Image schizoImage;
    public Image insomniaImage;
    public Image lactoseImage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddItem(string type, int amount)
    {
        if (type == "Schizophrenia") schizophreniaCount += amount;
        else if (type == "Insomnia") insomniaCount += amount;
        else if (type == "Lactose") lactoseCount += amount;

        UpdateUI(); // Updates immediately when item is added
    }

    public bool HasItem(string type)
    {
        if (type == "Schizophrenia") return schizophreniaCount > 0;
        if (type == "Insomnia") return insomniaCount > 0;
        if (type == "Lactose") return lactoseCount > 0;
        return false;
    }

    public void UseItem(string type)
    {
        if (type == "Schizophrenia") schizophreniaCount--;
        else if (type == "Insomnia") insomniaCount--;
        else if (type == "Lactose") lactoseCount--;

        // Clamp to 0
        if (schizophreniaCount < 0) schizophreniaCount = 0;
        if (insomniaCount < 0) insomniaCount = 0;
        if (lactoseCount < 0) lactoseCount = 0;

        UpdateUI();
    }

    private void UpdateUI()
    {
        // 1. Update The Numbers
        if (schizoText != null) schizoText.text = "x" + schizophreniaCount.ToString();
        if (insomniaText != null) insomniaText.text = "x" + insomniaCount.ToString();
        if (lactoseText != null) lactoseText.text = "x" + lactoseCount.ToString();

        // 2. Update The Dimming
        UpdateImageAlpha(schizoImage, schizophreniaCount);
        UpdateImageAlpha(insomniaImage, insomniaCount);
        UpdateImageAlpha(lactoseImage, lactoseCount);
    }

    // Helper function to handle the transparency
    private void UpdateImageAlpha(Image img, int count)
    {
        if (img == null) return;

        Color c = img.color;
        if (count > 0)
        {
            c.a = 1f; // Full Visibility
        }
        else
        {
            c.a = 0.3f; // Dimmed (30% visible)
        }
        img.color = c;
    }
}