using UnityEngine;

[System.Serializable]
public struct ItemLevelData
{
    public Item itemPrefab;
    public bool isGoal;

    [NaughtyAttributes.ValidateInput("ValidateAmount", "Amount must be multiple of 3 sevgili AMELE!")]
    [NaughtyAttributes.AllowNesting]
    public int amount;

    private bool ValidateAmount()
    {
        return amount % 3 == 0;
    }

}
