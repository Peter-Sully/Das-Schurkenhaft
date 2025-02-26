using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; }

    // Resource storage
    public int gold = 0;
    public int silver = 0;
    public int bronze = 0;
    public int paper = 0;
    public int wood = 0;
    public int dye = 0;
    public int ink = 0;

    private void Awake()
    {
        // Ensure only one instance of MaterialManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it alive between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    // Methods to add materials
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold: " + gold);
    }

    public void AddSilver(int amount)
    {
        silver += amount;
        Debug.Log("Silver: " + silver);
    }

    public void AddBronze(int amount)
    {
        bronze += amount;
        Debug.Log("Bronze: " + bronze);
    }

    public void AddPaper(int amount)
    {
        paper += amount;
        Debug.Log("Paper: " + paper);
    }

    public void AddWood(int amount)
    {
        wood += amount;
        Debug.Log("Wood: " + wood);
    }

    public void AddDye(int amount)
    {
        dye += amount;
        Debug.Log("Dye: " + dye);
    }

    public void AddInk(int amount)
    {
        ink += amount;
        Debug.Log("Ink: " + ink);
    }
}
